using PM.Api.Constants;
using PM.Api.Exceptions;
using PM.Api.MemoryCacher;
using System.Dynamic;
using System.Net;

namespace PM.Api.Middleware;

public class ErrorResponse
{
    public string ErrorMessage { get; set; }
    public ErrorResponse(string _errorMessage)
    {
        ErrorMessage = _errorMessage;
    }
}

public class PMApiMiddleware
{
    private readonly Logger.ILogger _logger;
    private readonly RequestDelegate _next;
    private readonly IMemoryCacher _memoryCacher;

    public PMApiMiddleware(Logger.ILogger logger, RequestDelegate next, IMemoryCacher memoryCacher)
    {
        _next = next;
        _logger = logger;
        _memoryCacher = memoryCacher;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            string pathString = httpContext.Request.Path.Value ?? string.Empty;
            string httpMethod = httpContext.Request.Method;
            string id = httpContext.Request.Query.ContainsKey(PMConstants.RedisObjectCachingAccessor) 
                ? httpContext.Request.Query[PMConstants.RedisObjectCachingAccessor] 
                : string.Empty;

            if(httpMethod == PMConstants.CachingHttpMethod && PMConstants.MemoryCacherDictionary.ContainsKey(pathString) && !string.IsNullOrWhiteSpace(id))
            {
                Type type = PMConstants.MemoryCacherDictionary[pathString];
                string memoryCacherAccessor = $"{PMConstants.ObjectCachePrefix}:{type.Name}:{id}";
                string jsonStringResult = await _memoryCacher.GetStringAsync(memoryCacherAccessor);

                if(!string.IsNullOrWhiteSpace(jsonStringResult))
                {
                    var dataObject = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(jsonStringResult);
                    await httpContext.Response.WriteAsJsonAsync(dataObject);
                }
                else
                {
                    Stream originalBody = httpContext.Response.Body;

                    try
                    {
                        using (var memStream = new MemoryStream())
                        {
                            httpContext.Response.Body = memStream;

                            await _next(httpContext);

                            memStream.Position = 0;
                            string responseBody = new StreamReader(memStream).ReadToEnd();

                            _memoryCacher.SetString(memoryCacherAccessor, responseBody);
        
                            memStream.Position = 0;
                            await memStream.CopyToAsync(originalBody);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        httpContext.Response.Body = originalBody;
                    }

                }
            }
            else
            {
                await _next(httpContext);
            }

        }
        catch (ValidateException ex)
        {
            httpContext.Response.ContentType = PMConstants.JsonContentType;
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(
                new ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            httpContext.Response.ContentType = PMConstants.JsonContentType;
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync(ex.StackTrace ?? ex.ToString());
        }
    }
}


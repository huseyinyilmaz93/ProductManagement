using PM.Api.Models;

namespace PM.Api.Constants;

public static class PMConstants
{
    public static TimeSpan DefaultRedisExpirity = TimeSpan.FromMinutes(5);
    public const string ObjectCachePrefix = "ObjectCaching";
    public const string DefaultDatabase = "PM";
    public const string RedisConfigurationAccessor = "Redis:ConnectionString";
    public const string MongoDbConfigurationAccessor = "ConnectionStrings:MongoConnectionString";
    public const string CachingHttpMethod = "GET";

    //TODO: Lazy load implementation. Read from database or read it from appsetting.json etc.
    public static Dictionary<string, Type> MemoryCacherDictionary = new Dictionary<string, Type>() { { "/api/products", typeof(Product) } };

}


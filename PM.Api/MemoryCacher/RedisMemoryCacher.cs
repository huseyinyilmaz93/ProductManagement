using PM.Api.Constants;
using PM.Api.Managers;
using StackExchange.Redis;

namespace PM.Api.MemoryCacher;


//TODO: Retry and circuit breaker implementation with Polly.
//TODO: Lazy load implementation. If there is no redis server available, waits for redis connection down after multiple trying.
public class RedisMemoryCacher : AbstractRemoteConnectionManager, IMemoryCacher
{
    private readonly Logger.ILogger _logger;
    private readonly IDatabase _redisDatabase;

    private const string deleteAllLuaScript = @"
        local function DELETEALL(keys)
            local sum = 0
            for _,key in ipairs(keys) do
                redis.call('DEL', key)
            end
        end
        return DELETEALL(KEYS)";

    public RedisMemoryCacher(Logger.ILogger logger, IConfiguration configuration)
    {
        _logger = logger;
        try
        {
            ConnectionMultiplexer redisConnection = ConnectionMultiplexer
                .Connect(configuration.GetValue<string>(PMConstants.RedisConfigurationAccessor));
            _redisDatabase = redisConnection.GetDatabase();

            base.SetAliveStatus(true);
        }
        catch (Exception ex)
        {
            _logger.Log(ex.ToString());
        }
    }

    public void SetString(string key, string value)
    {
        try
        {
            if(base.GetAliveStatus())
                Task.Run(()
                    => _redisDatabase.StringSet(key, value, PMConstants.DefaultRedisExpirity));
        }
        catch (RedisConnectionException ex)
        {
            base.SetAliveStatus(false);
            _logger.Log(ex.ToString());
        }
        catch (Exception ex)
        {
            _logger.Log(ex.ToString());
        }
    }

    public async Task<string> GetStringAsync(string key)
    {
        try
        {
            if(base.GetAliveStatus())
                return await _redisDatabase.StringGetAsync(key);
        }
        catch (RedisConnectionException ex) 
        {
            base.SetAliveStatus(false);
            _logger.Log(ex.ToString());
        }
        catch (Exception ex)
        {
            _logger.Log(ex.ToString());
        }
        return default;
    }

    public void DeleteAll(string[] keys)
    {
        try
        {
            if (base.GetAliveStatus())
                Task.Run(()
                => _redisDatabase.ScriptEvaluate(deleteAllLuaScript, keys.Select((key) => new RedisKey(key)).ToArray()));
        }
        catch (RedisConnectionException ex)
        {
            base.SetAliveStatus(false);
            _logger.Log(ex.ToString());
        }
        catch (Exception ex)
        {
            _logger.Log(ex.ToString());
        }
    }
}


using PM.Api.Constants;
using PM.Api.Helpers;
using StackExchange.Redis;

namespace PM.Api.MemoryCacher;

public interface IMemoryCacher
{
    void SetString(string key, string value);
    void DeleteAll(string[] keys);
    Task<string> GetStringAsync(string key);
}

//TODO: Retry and circuit breaker implementation with Polly.
public class RedisMemoryCacher : IMemoryCacher
{
    private readonly IDatabase _redisDatabase;

    private const string deleteAllLuaScript = @"
        local function DELETEALL(keys)
            local sum = 0
            for _,key in ipairs(keys) do
                redis.call('DEL', key)
            end
        end
        return DELETEALL(KEYS)";

    public RedisMemoryCacher(IConfiguration configuration)
    {
        ConnectionMultiplexer redisConnection = ConnectionMultiplexer
            .Connect(configuration.GetValue<string>(PMConstants.RedisConfigurationAccessor));
        _redisDatabase = redisConnection.GetDatabase();
    }

    public void SetString(string key, string value)
    {
        //TODO: Logging and exception management
        Task.Run(() 
            => _redisDatabase.StringSet(key, value, PMConstants.DefaultRedisExpirity));
    }

    public async Task<string> GetStringAsync(string key)
    {
        return await _redisDatabase.StringGetAsync(key);
    }

    public void DeleteAll(string[] keys)
    {
        //TODO: Logging and exception management
        Task.Run(() 
            => _redisDatabase.ScriptEvaluate(deleteAllLuaScript, keys.Select((key) => new RedisKey(key)).ToArray()));
    }
}


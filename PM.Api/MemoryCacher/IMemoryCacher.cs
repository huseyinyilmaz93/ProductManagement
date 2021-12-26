namespace PM.Api.MemoryCacher;

public interface IMemoryCacher
{
    void SetString(string key, string value);
    void DeleteAll(string[] keys);
    Task<string> GetStringAsync(string key);
}

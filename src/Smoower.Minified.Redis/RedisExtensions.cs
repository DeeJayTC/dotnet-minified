using System.Text.Json;
using StackExchange.Redis;

namespace Smoower.Minified.Redis;

// Compact StackExchange.Redis helpers. Thin wrappers over IDatabase.
public static class RedisExtensions
{
    public static Task<RedisValue> get(this IDatabase db, string key)
        => db.StringGetAsync(key);

    public static Task<bool> set(this IDatabase db, string key, RedisValue value, TimeSpan? ttl = null)
        => db.StringSetAsync(key, value, ttl);

    public static Task<bool> del(this IDatabase db, string key)
        => db.KeyDeleteAsync(key);

    public static Task<long> incr(this IDatabase db, string key, long by = 1)
        => db.StringIncrementAsync(key, by);

    public static async Task<T?> getJson<T>(this IDatabase db, string key)
    {
        var v = await db.StringGetAsync(key);
        return v.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(v.ToString());
    }

    public static Task<bool> setJson<T>(this IDatabase db, string key, T value, TimeSpan? ttl = null)
        => db.StringSetAsync(key, JsonSerializer.Serialize(value), ttl);
}

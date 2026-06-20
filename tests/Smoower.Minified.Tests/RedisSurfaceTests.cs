using StackExchange.Redis;
using Smoower.Minified.Redis;

namespace Smoower.Minified.Tests;

// Redis helpers wrap a live IDatabase, so they aren't exercised against a real
// server here. This guards the public surface (names + that they extend
// IDatabase) so refactors can't silently change the compact API.
public class RedisSurfaceTests
{
    [Th]
    [In("get")]
    [In("set")]
    [In("del")]
    [In("incr")]
    [In("getJson")]
    [In("setJson")]
    public void Helper_Exists_AsExtensionOnIDatabase(string name)
    {
        var method = typeof(RedisExtensions).GetMethod(name);
        method.notNul();
        method!.IsStatic.tru();
        method.GetParameters()[0].ParameterType.eq(typeof(IDatabase));
    }
}

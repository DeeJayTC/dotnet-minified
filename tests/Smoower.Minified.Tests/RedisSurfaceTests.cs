using StackExchange.Redis;
using Smoower.Minified.Redis;
using Xunit;

namespace Smoower.Minified.Tests;

// Redis helpers wrap a live IDatabase, so they aren't exercised against a real
// server here. This guards the public surface (names + that they extend
// IDatabase) so refactors can't silently change the compact API.
public class RedisSurfaceTests
{
    [Theory]
    [InlineData("get")]
    [InlineData("set")]
    [InlineData("del")]
    [InlineData("incr")]
    [InlineData("getJson")]
    [InlineData("setJson")]
    public void Helper_Exists_AsExtensionOnIDatabase(string name)
    {
        var method = typeof(RedisExtensions).GetMethod(name);
        Assert.NotNull(method);
        Assert.True(method!.IsStatic);
        Assert.Equal(typeof(IDatabase), method.GetParameters()[0].ParameterType);
    }
}

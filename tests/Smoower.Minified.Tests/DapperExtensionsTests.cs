using System.Data;
using Smoower.Minified.Dapper;
using Xunit;

namespace Smoower.Minified.Tests;

// Dapper helpers wrap a live IDbConnection. Rather than pull in a native SQLite
// engine (and its advisories) just to exercise five pass-through one-liners, we
// guard the public surface - names + that they extend IDbConnection - the same
// way RedisSurfaceTests does. The bodies are direct Dapper calls.
public class DapperExtensionsTests
{
    [Theory]
    [InlineData("q")]
    [InlineData("q1")]
    [InlineData("qs")]
    [InlineData("ex")]
    [InlineData("scalar")]
    public void Helper_Exists_AsExtensionOnIDbConnection(string name)
    {
        var method = typeof(DapperExtensions).GetMethod(name);
        Assert.NotNull(method);
        Assert.True(method!.IsStatic);
        Assert.Equal(typeof(IDbConnection), method.GetParameters()[0].ParameterType);
    }
}

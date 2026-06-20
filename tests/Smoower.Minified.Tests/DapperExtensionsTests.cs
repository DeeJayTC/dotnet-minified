using System.Data;
using Smoower.Minified.Dapper;

namespace Smoower.Minified.Tests;

// Dapper helpers wrap a live IDbConnection. Rather than pull in a native SQLite
// engine (and its advisories) just to exercise five pass-through one-liners, we
// guard the public surface - names + that they extend IDbConnection - the same
// way RedisSurfaceTests does. The bodies are direct Dapper calls.
public class DapperExtensionsTests
{
    [Th]
    [In("q")]
    [In("q1")]
    [In("qs")]
    [In("ex")]
    [In("scalar")]
    public void Helper_Exists_AsExtensionOnIDbConnection(string name)
    {
        var method = typeof(DapperExtensions).GetMethod(name);
        method.notNul();
        method!.IsStatic.tru();
        method.GetParameters()[0].ParameterType.eq(typeof(IDbConnection));
    }
}

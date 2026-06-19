using Microsoft.Data.Sqlite;
using Smoower.Minified.Dapper;
using Xunit;

namespace Smoower.Minified.Tests;

public class DapperExtensionsTests
{
    public record Row(long Id, string Name);

    private static SqliteConnection Open()
    {
        // In-memory SQLite lives only while the connection is open.
        var c = new SqliteConnection("Data Source=:memory:");
        c.Open();
        return c;
    }

    [Fact]
    public async Task Ex_Q_Q1_Qs_Scalar_RoundTrip()
    {
        using var c = Open();
        await c.ex("create table t(Id integer primary key, Name text)");

        var inserted = await c.ex("insert into t(Name) values(@Name)", new { Name = "ada" });
        Assert.Equal(1, inserted);

        var all = await c.q<Row>("select Id, Name from t");
        Assert.Single(all);

        var hit = await c.q1<Row>("select Id, Name from t where Id=@Id", new { Id = 1 });
        Assert.Equal("ada", hit!.Name);

        var miss = await c.q1<Row>("select Id, Name from t where Id=@Id", new { Id = 99 });
        Assert.Null(miss);

        var single = await c.qs<Row>("select Id, Name from t where Id=1");
        Assert.NotNull(single);

        var count = await c.scalar<long>("select count(*) from t");
        Assert.Equal(1L, count!.Value);
    }
}

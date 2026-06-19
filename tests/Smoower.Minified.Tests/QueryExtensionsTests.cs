using Smoower.Minified.EFCore;
using Xunit;

namespace Smoower.Minified.Tests;

public class QueryExtensionsTests
{
    private static async Task<TestDb> Seed(string name)
    {
        var db = TestDbFactory.Create(name);
        db.Things.AddRange(
            new Thing { Name = "b", Rank = 2 },
            new Thing { Name = "a", Rank = 1 },
            new Thing { Name = "c", Rank = 3 });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task W_FiltersRows()
    {
        var db = await Seed(nameof(W_FiltersRows));
        Assert.Equal(2, (await db.Things.w(t => t.Rank >= 2).lst()).Count);
    }

    [Fact]
    public async Task S_Projects()
    {
        var db = await Seed(nameof(S_Projects));
        Assert.Equal(new[] { "a", "b", "c" }, await db.Things.ob(t => t.Rank).s(t => t.Name).lst());
    }

    [Fact]
    public async Task ObdAndPaging_Work()
    {
        var db = await Seed(nameof(ObdAndPaging_Work));
        Assert.Equal(new[] { "b" }, await db.Things.obd(t => t.Rank).sk(1).tk(1).s(t => t.Name).lst());
    }

    [Fact]
    public async Task ObThenBy_Work()
    {
        var db = await Seed(nameof(ObThenBy_Work));
        Assert.Equal(new[] { "a", "b", "c" }, await db.Things.ob(t => t.Rank).tb(t => t.Name).s(t => t.Name).lst());
    }

    [Fact]
    public async Task One_ReturnsMatchOrNull()
    {
        var db = await Seed(nameof(One_ReturnsMatchOrNull));
        Assert.NotNull(await db.Things.w(t => t.Name == "a").one());
        Assert.Null(await db.Things.w(t => t.Name == "zzz").one());
    }

    [Fact]
    public async Task Single_ReturnsMatch()
    {
        var db = await Seed(nameof(Single_ReturnsMatch));
        Assert.Equal("c", (await db.Things.w(t => t.Rank == 3).single())!.Name);
    }

    [Fact]
    public async Task AnyAndCount_Work()
    {
        var db = await Seed(nameof(AnyAndCount_Work));
        Assert.True(await db.Things.any());
        Assert.False(await db.Things.w(t => t.Rank > 99).any());
        Assert.Equal(3, await db.Things.cnt());
    }

    [Fact]
    public async Task Nt_DoesNotTrack()
    {
        var name = nameof(Nt_DoesNotTrack);
        (await Seed(name)).Dispose();
        var db = TestDbFactory.Create(name);
        Assert.NotNull(await db.Things.nt().w(t => t.Name == "a").one());
        Assert.Empty(db.ChangeTracker.Entries());
    }
}

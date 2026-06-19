using Smoower.Minified.EFCore;
using Xunit;

namespace Smoower.Minified.Tests;

public class WriteExtensionsTests
{
    [Fact]
    public async Task Add_PersistsAndReturnsEntity()
    {
        var db = TestDbFactory.Create(nameof(Add_PersistsAndReturnsEntity));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        Assert.True(saved.Id > 0);
        Assert.Equal(1, await db.Things.cnt());
    }

    [Fact]
    public async Task Id_FindsByKey()
    {
        var db = TestDbFactory.Create(nameof(Id_FindsByKey));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        Assert.Equal("x", (await db.Things.id(saved.Id))!.Name);
    }

    [Fact]
    public async Task Upd_PersistsChanges()
    {
        var db = TestDbFactory.Create(nameof(Upd_PersistsChanges));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        saved.Name = "y";
        await db.upd(saved);
        Assert.Equal("y", (await db.Things.id(saved.Id))!.Name);
    }

    [Fact]
    public async Task Del_RemovesEntity()
    {
        var db = TestDbFactory.Create(nameof(Del_RemovesEntity));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        await db.del(saved);
        Assert.Equal(0, await db.Things.cnt());
    }

    [Fact]
    public async Task Save_ReturnsAffectedCount()
    {
        var db = TestDbFactory.Create(nameof(Save_ReturnsAffectedCount));
        db.Things.Add(new Thing { Name = "x", Rank = 1 });
        Assert.Equal(1, await db.save());
    }
}

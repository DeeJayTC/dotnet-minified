using Smoower.Minified.EFCore;
using Xunit;

namespace Smoower.Minified.Tests;

public class SyncExtensionsTests
{
    private static TestDb Seed(string name)
    {
        var db = TestDbFactory.Create(name);
        db.Things.AddRange(new Thing { Name = "a", Rank = 1 }, new Thing { Name = "b", Rank = 2 });
        db.SaveChanges();
        return db;
    }

    [Fact]
    public void QuerySyncTerminators_Work()
    {
        var db = Seed(nameof(QuerySyncTerminators_Work));
        Assert.Equal(2, db.Things.lstS().Count);
        Assert.Equal("a", db.Things.w(t => t.Rank == 1).oneS()!.Name);
        Assert.Equal("b", db.Things.w(t => t.Rank == 2).singleS()!.Name);
        Assert.True(db.Things.anyS());
        Assert.Equal(2, db.Things.cntS());
    }

    [Fact]
    public void AddS_PersistsAndReturns()
    {
        var db = TestDbFactory.Create(nameof(AddS_PersistsAndReturns));
        var saved = db.addS(new Thing { Name = "x", Rank = 1 });
        Assert.True(saved.Id > 0);
        Assert.Equal("x", db.Things.idS(saved.Id)!.Name);
    }

    [Fact]
    public void UpdS_And_DelS_Work()
    {
        var db = TestDbFactory.Create(nameof(UpdS_And_DelS_Work));
        var saved = db.addS(new Thing { Name = "x", Rank = 1 });
        saved.Name = "y";
        db.updS(saved);
        Assert.Equal("y", db.Things.idS(saved.Id)!.Name);
        db.delS(saved);
        Assert.Equal(0, db.Things.cntS());
    }

    [Fact]
    public void SaveS_ReturnsAffectedCount()
    {
        var db = TestDbFactory.Create(nameof(SaveS_ReturnsAffectedCount));
        db.Things.Add(new Thing { Name = "x", Rank = 1 });
        Assert.Equal(1, db.saveS());
    }
}

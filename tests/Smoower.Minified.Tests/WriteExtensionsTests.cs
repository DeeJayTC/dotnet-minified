using Smoower.Minified.EFCore;

namespace Smoower.Minified.Tests;

public class WriteExtensionsTests
{
    [F]
    public async Task Add_PersistsAndReturnsEntity()
    {
        var db = TestDbFactory.Create(nameof(Add_PersistsAndReturnsEntity));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        (saved.Id > 0).tru();
        (await db.Things.cnt()).eq(1);
    }

    [F]
    public async Task Id_FindsByKey()
    {
        var db = TestDbFactory.Create(nameof(Id_FindsByKey));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        (await db.Things.id(saved.Id))!.Name.eq("x");
    }

    [F]
    public async Task Upd_PersistsChanges()
    {
        var db = TestDbFactory.Create(nameof(Upd_PersistsChanges));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        saved.Name = "y";
        await db.upd(saved);
        (await db.Things.id(saved.Id))!.Name.eq("y");
    }

    [F]
    public async Task Del_RemovesEntity()
    {
        var db = TestDbFactory.Create(nameof(Del_RemovesEntity));
        var saved = await db.add(new Thing { Name = "x", Rank = 1 });
        await db.del(saved);
        (await db.Things.cnt()).eq(0);
    }

    [F]
    public async Task Save_ReturnsAffectedCount()
    {
        var db = TestDbFactory.Create(nameof(Save_ReturnsAffectedCount));
        db.Things.Add(new Thing { Name = "x", Rank = 1 });
        (await db.save()).eq(1);
    }
}

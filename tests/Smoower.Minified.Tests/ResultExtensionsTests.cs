using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;
using Smoower.Minified.EFCore;

namespace Smoower.Minified.Tests;

public class ResultExtensionsTests
{
    private static async Task<TestDb> Seed(string name)
    {
        var db = TestDbFactory.Create(name);
        db.Things.AddRange(new Thing { Name = "a", Rank = 1 }, new Thing { Name = "b", Rank = 2 });
        await db.SaveChangesAsync();
        return db;
    }

    [F]
    public async Task Ok1_OkWhenFound()
        => (await (await Seed(nameof(Ok1_OkWhenFound))).Things.w(t => t.Name == "a").ok1()).isType<OkObjectResult>();

    [F]
    public async Task Ok1_NotFoundWhenMissing()
        => (await (await Seed(nameof(Ok1_NotFoundWhenMissing))).Things.w(t => t.Name == "zzz").ok1()).isType<NotFoundResult>();

    [F]
    public async Task Okl_OkWithList()
    {
        var ok = (await (await Seed(nameof(Okl_OkWithList))).Things.okl()).isType<OkObjectResult>();
        ok.Value.isAssignable<IEnumerable<Thing>>().Count().eq(2);
    }

    [F]
    public async Task Okc_OkWithCount()
    {
        var ok = (await (await Seed(nameof(Okc_OkWithCount))).Things.okc()).isType<OkObjectResult>();
        ok.Value.eq(2);
    }

    [F]
    public async Task OkId_OkThenNotFound()
    {
        var db = await Seed(nameof(OkId_OkThenNotFound));
        (await db.Things.okId(1)).isType<OkObjectResult>();
        (await db.Things.okId(9999)).isType<NotFoundResult>();
    }

    [F]
    public async Task OkAdd_PersistsAndReturnsOk()
    {
        var db = TestDbFactory.Create(nameof(OkAdd_PersistsAndReturnsOk));
        (await db.okAdd(new Thing { Name = "x", Rank = 1 })).isType<OkObjectResult>();
        (await db.Things.cnt()).eq(1);
    }

    [F]
    public async Task DelById_NoContentThenRemoved()
    {
        var db = await Seed(nameof(DelById_NoContentThenRemoved));
        (await db.delById<Thing>(1)).isType<NoContentResult>();
        (await db.Things.cnt()).eq(1);
    }

    [F]
    public async Task DelById_NotFoundWhenMissing()
        => (await (await Seed(nameof(DelById_NotFoundWhenMissing))).delById<Thing>(9999)).isType<NotFoundResult>();
}

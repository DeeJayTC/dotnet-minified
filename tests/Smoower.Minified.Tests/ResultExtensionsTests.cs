using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;
using Smoower.Minified.EFCore;
using Xunit;

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

    [Fact]
    public async Task Ok1_OkWhenFound()
        => Assert.IsType<OkObjectResult>(await (await Seed(nameof(Ok1_OkWhenFound))).Things.w(t => t.Name == "a").ok1());

    [Fact]
    public async Task Ok1_NotFoundWhenMissing()
        => Assert.IsType<NotFoundResult>(await (await Seed(nameof(Ok1_NotFoundWhenMissing))).Things.w(t => t.Name == "zzz").ok1());

    [Fact]
    public async Task Okl_OkWithList()
    {
        var ok = Assert.IsType<OkObjectResult>(await (await Seed(nameof(Okl_OkWithList))).Things.okl());
        Assert.Equal(2, Assert.IsAssignableFrom<IEnumerable<Thing>>(ok.Value).Count());
    }

    [Fact]
    public async Task Okc_OkWithCount()
    {
        var ok = Assert.IsType<OkObjectResult>(await (await Seed(nameof(Okc_OkWithCount))).Things.okc());
        Assert.Equal(2, ok.Value);
    }

    [Fact]
    public async Task OkId_OkThenNotFound()
    {
        var db = await Seed(nameof(OkId_OkThenNotFound));
        Assert.IsType<OkObjectResult>(await db.Things.okId(1));
        Assert.IsType<NotFoundResult>(await db.Things.okId(9999));
    }

    [Fact]
    public async Task OkAdd_PersistsAndReturnsOk()
    {
        var db = TestDbFactory.Create(nameof(OkAdd_PersistsAndReturnsOk));
        Assert.IsType<OkObjectResult>(await db.okAdd(new Thing { Name = "x", Rank = 1 }));
        Assert.Equal(1, await db.Things.cnt());
    }

    [Fact]
    public async Task DelById_NoContentThenRemoved()
    {
        var db = await Seed(nameof(DelById_NoContentThenRemoved));
        Assert.IsType<NoContentResult>(await db.delById<Thing>(1));
        Assert.Equal(1, await db.Things.cnt());
    }

    [Fact]
    public async Task DelById_NotFoundWhenMissing()
        => Assert.IsType<NotFoundResult>(await (await Seed(nameof(DelById_NotFoundWhenMissing))).delById<Thing>(9999));
}

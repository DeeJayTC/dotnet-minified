using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;
using Smoower.Minified.EFCore;

namespace Smoower.Minified.Tests;

public class PagingExtensionsTests
{
    private static async Task<TestDb> Seed(string name, int n = 25)
    {
        var db = TestDbFactory.Create(name);
        for (var i = 1; i <= n; i++) db.Things.Add(new Thing { Name = $"t{i:00}", Rank = i });
        await db.SaveChangesAsync();
        return db;
    }

    [F]
    public async Task WhereIf_AppliesPredicateWhenTrue()
        => (await (await Seed(nameof(WhereIf_AppliesPredicateWhenTrue))).Things.whereIf(true, t => t.Rank <= 5).cnt()).eq(5);

    [F]
    public async Task WhereIf_NoOpWhenFalse()
        => (await (await Seed(nameof(WhereIf_NoOpWhenFalse))).Things.whereIf(false, t => t.Rank <= 5).cnt()).eq(25);

    [F]
    public async Task Paged_ReturnsEnvelopeAndTakesPage()
    {
        var db = await Seed(nameof(Paged_ReturnsEnvelopeAndTakesPage));
        var ok = (await db.Things.ob(t => t.Rank).paged(2, 10)).isType<OkObjectResult>();
        var p = ok.Value.isType<PagedResult<Thing>>();
        p.Total.eq(25);
        p.Page.eq(2);
        p.PageSize.eq(10);
        p.Items.Count.eq(10);
        p.Items[0].Rank.eq(11);
    }

    [F]
    public async Task Paged_Projects()
    {
        var db = await Seed(nameof(Paged_Projects));
        var ok = (await db.Things.ob(t => t.Rank).paged(1, 3, t => t.Name)).isType<OkObjectResult>();
        var p = ok.Value.isType<PagedResult<string>>();
        p.Items.eqSeq(new[] { "t01", "t02", "t03" });
        p.Total.eq(25);
    }

    [F]
    public async Task Paged_ClampsPageSizeToMaxAndPageToOne()
    {
        var db = await Seed(nameof(Paged_ClampsPageSizeToMaxAndPageToOne));
        var ok = (await db.Things.ob(t => t.Rank).paged(0, 9999, CancellationToken.None, max: 20)).isType<OkObjectResult>();
        var p = ok.Value.isType<PagedResult<Thing>>();
        p.Page.eq(1);
        p.PageSize.eq(20);
        p.Items.Count.eq(20);
    }

    [F]
    public async Task WhereIf_ChainsIntoPaged()
    {
        var db = await Seed(nameof(WhereIf_ChainsIntoPaged));
        DateTime? since = null;
        int floor = 20;
        var ok = (await db.Things.whereIf(since.HasValue, t => t.Rank >= 0).whereIf(true, t => t.Rank >= floor).ob(t => t.Rank).paged(1, 50)).isType<OkObjectResult>();
        var p = ok.Value.isType<PagedResult<Thing>>();
        p.Total.eq(6);
    }
}

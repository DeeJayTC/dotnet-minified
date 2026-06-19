using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Smoower.Minified.AspNetCore;

// Result-fusing terminators (the former "Ultra" layer): run the query / EF op
// AND wrap the outcome in an IActionResult, so a controller action collapses to
// a single expression with no async/await/return/Ok/NotFound.
//
// TRADE-OFF: this couples your data calls to ASP.NET Core MVC result types and
// hides the HTTP status behind the method name (e.g. `ok1` returns 404 when the
// row is missing). That is the readability price of the extra token savings.
public static class ResultExtensions
{
    // First-or-default -> 200 with the row, or 404 when missing.
    public static async Task<IActionResult> ok1<T>(this IQueryable<T> q, CancellationToken ct = default)
        => await q.FirstOrDefaultAsync(ct) is { } v ? new OkObjectResult(v) : new NotFoundResult();

    // 201 Created with the value as the body (no Location header). Replaces the
    // verbose CreatedAtAction(nameof(Get), new { id = x.Id }, x) ceremony.
    public static IActionResult created<T>(this T value)
        => new ObjectResult(value) { StatusCode = 201 };

    // Add + save -> 201 Created with the saved entity (the usual POST happy path).
    public static async Task<IActionResult> okNew<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Add(entity);
        await db.SaveChangesAsync(ct);
        return new ObjectResult(entity) { StatusCode = 201 };
    }

    // 200 with the full list.
    public static async Task<IActionResult> okl<T>(this IQueryable<T> q, CancellationToken ct = default)
        => new OkObjectResult(await q.ToListAsync(ct));

    // 200 with the count.
    public static async Task<IActionResult> okc<T>(this IQueryable<T> q, CancellationToken ct = default)
        => new OkObjectResult(await q.CountAsync(ct));

    // FindAsync by key -> 200 with the entity, or 404 when missing.
    public static async Task<IActionResult> okId<T>(this DbSet<T> set, params object?[] keyValues) where T : class
        => await set.FindAsync(keyValues) is { } v ? new OkObjectResult(v) : new NotFoundResult();

    // Add + save -> 200 with the saved entity.
    public static async Task<IActionResult> okAdd<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Add(entity);
        await db.SaveChangesAsync(ct);
        return new OkObjectResult(entity);
    }

    // Find by key -> remove + save -> 204, or 404 when missing.
    public static async Task<IActionResult> delById<T>(this DbContext db, object key, CancellationToken ct = default) where T : class
    {
        if (await db.Set<T>().FindAsync([key], ct) is not { } v)
            return new NotFoundResult();
        db.Remove(v);
        await db.SaveChangesAsync(ct);
        return new NoContentResult();
    }
}

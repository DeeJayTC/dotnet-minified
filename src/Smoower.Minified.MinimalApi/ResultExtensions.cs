using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Smoower.Minified.MinimalApi;

// Result-fusing terminators for Minimal APIs: run the query / EF op AND wrap the
// outcome in an IResult, so a route handler collapses to a single expression with
// no async/await/return/Results.Ok/NotFound. These mirror the controller
// terminators in Smoower.Minified.AspNetCore but return IResult (via TypedResults)
// instead of IActionResult. Import this namespace OR the AspNetCore one in a given
// file, not both — the names are identical on purpose.
//
// TRADE-OFF: this couples your data calls to ASP.NET Core result types and hides
// the HTTP status behind the method name (e.g. `ok1` returns 404 when missing).
public static class ResultExtensions
{
    // First-or-default -> 200 with the row, or 404 when missing.
    public static async Task<IResult> ok1<T>(this IQueryable<T> q, CancellationToken ct = default)
        => await q.FirstOrDefaultAsync(ct) is { } v ? TypedResults.Ok(v) : TypedResults.NotFound();

    // 201 Created with the value as the body (no Location header).
    public static IResult created<T>(this T value)
        => TypedResults.Created((string?)null, value);

    // Add + save -> 201 Created with the saved entity (the usual POST happy path).
    public static async Task<IResult> okNew<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Add(entity);
        await db.SaveChangesAsync(ct);
        return TypedResults.Created((string?)null, entity);
    }

    // 200 with the full list.
    public static async Task<IResult> okl<T>(this IQueryable<T> q, CancellationToken ct = default)
        => TypedResults.Ok(await q.ToListAsync(ct));

    // 200 with the count.
    public static async Task<IResult> okc<T>(this IQueryable<T> q, CancellationToken ct = default)
        => TypedResults.Ok(await q.CountAsync(ct));

    // FindAsync by key -> 200 with the entity, or 404 when missing.
    public static async Task<IResult> okId<T>(this DbSet<T> set, params object?[] keyValues) where T : class
        => await set.FindAsync(keyValues) is { } v ? TypedResults.Ok(v) : TypedResults.NotFound();

    // Add + save -> 200 with the saved entity.
    public static async Task<IResult> okAdd<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Add(entity);
        await db.SaveChangesAsync(ct);
        return TypedResults.Ok(entity);
    }

    // Find by key -> remove + save -> 204, or 404 when missing.
    public static async Task<IResult> delById<T>(this DbContext db, object key, CancellationToken ct = default) where T : class
    {
        if (await db.Set<T>().FindAsync([key], ct) is not { } v)
            return TypedResults.NotFound();
        db.Remove(v);
        await db.SaveChangesAsync(ct);
        return TypedResults.NoContent();
    }
}

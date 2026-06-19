using Microsoft.EntityFrameworkCore;

namespace Smoower.Minified.EFCore;

// Small, predictable wrappers over the most common EF Core write operations.
public static class WriteExtensions
{
    public static ValueTask<T?> id<T>(this DbSet<T> set, params object?[] keyValues) where T : class
        => set.FindAsync(keyValues);

    public static Task<int> save(this DbContext db, CancellationToken ct = default)
        => db.SaveChangesAsync(ct);

    public static async Task<T> add<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity;
    }

    public static async Task<T> upd<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Update(entity);
        await db.SaveChangesAsync(ct);
        return entity;
    }

    public static async Task del<T>(this DbContext db, T entity, CancellationToken ct = default) where T : class
    {
        db.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    // Synchronous variants (suffix S). Async is the unmarked default because it
    // is the hot path AND where the token savings live (the dropped "Async").
    // The sync names below don't materially cut tokens - they're for the
    // minority that needs sync, and for API symmetry.
    public static T? idS<T>(this DbSet<T> set, params object?[] keyValues) where T : class
        => set.Find(keyValues);

    public static int saveS(this DbContext db)
        => db.SaveChanges();

    public static T addS<T>(this DbContext db, T entity) where T : class
    {
        db.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static T updS<T>(this DbContext db, T entity) where T : class
    {
        db.Update(entity);
        db.SaveChanges();
        return entity;
    }

    public static void delS<T>(this DbContext db, T entity) where T : class
    {
        db.Remove(entity);
        db.SaveChanges();
    }
}

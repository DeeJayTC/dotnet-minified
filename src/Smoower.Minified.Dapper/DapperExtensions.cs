using System.Data;
using Dapper;

namespace Smoower.Minified.Dapper;

// Compact Dapper helpers over IDbConnection. Thin wrappers; the parameter object
// is Dapper's usual anonymous-type / DynamicParameters bag.
public static class DapperExtensions
{
    public static Task<IEnumerable<T>> q<T>(this IDbConnection c, string sql, object? param = null)
        => c.QueryAsync<T>(sql, param);

    public static Task<T?> q1<T>(this IDbConnection c, string sql, object? param = null)
        => c.QueryFirstOrDefaultAsync<T?>(sql, param);

    public static Task<T?> qs<T>(this IDbConnection c, string sql, object? param = null)
        => c.QuerySingleOrDefaultAsync<T?>(sql, param);

    public static Task<int> ex(this IDbConnection c, string sql, object? param = null)
        => c.ExecuteAsync(sql, param);

    public static Task<T?> scalar<T>(this IDbConnection c, string sql, object? param = null)
        => c.ExecuteScalarAsync<T?>(sql, param);
}

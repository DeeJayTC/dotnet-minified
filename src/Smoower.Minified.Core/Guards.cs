namespace Smoower.Minified.Core;

// String / collection guards.
public static class Guards
{
    public static bool nil(this string? s) => string.IsNullOrWhiteSpace(s);

    public static bool emp(this string? s) => string.IsNullOrEmpty(s);

    public static bool none<T>(this IEnumerable<T> source) => !source.Any();
}

using Microsoft.Extensions.Logging;

namespace Smoower.Minified.Logging;

// Compact ILogger helpers. Declared on the non-generic ILogger base, so they
// apply equally to ILogger and ILogger<T> (the latter cannot be aliased, since
// C# has no open-generic using aliases - inject ILogger<T> and call these).
public static class LogExtensions
{
    public static void inf(this ILogger log, string message, params object?[] args)
        => log.LogInformation(message, args);

    public static void wrn(this ILogger log, string message, params object?[] args)
        => log.LogWarning(message, args);

    public static void err(this ILogger log, string message, params object?[] args)
        => log.LogError(message, args);

    public static void err(this ILogger log, Exception ex, string message, params object?[] args)
        => log.LogError(ex, message, args);

    public static void dbg(this ILogger log, string message, params object?[] args)
        => log.LogDebug(message, args);
}

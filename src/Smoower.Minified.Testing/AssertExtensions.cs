using System.Collections;
using Xunit;

namespace Smoower.Minified.Testing;

// Fluent, actual-first assertion shorteners over xUnit. `value.eq(2)` reads
// left-to-right and drops the `Assert.` prefix and the long method name that an
// assistant otherwise re-types on every assertion. Each one calls the matching
// Xunit.Assert under the hood, so the behavior and failure messages are identical.
//
// Note xUnit's Assert.Equal takes (expected, actual); the extension form flips to
// the more natural actual-first order and forwards correctly. The value-returning
// helpers return their input so assertions can chain: x.notNul().eq(expected).
public static class AssertExtensions
{
    public static T eq<T>(this T actual, T expected) { Assert.Equal(expected, actual); return actual; }
    public static T neq<T>(this T actual, T expected) { Assert.NotEqual(expected, actual); return actual; }

    // Element-wise sequence equality (xUnit's IEnumerable overload), for when the
    // two collection types differ (e.g. List<T> actual vs T[] expected).
    public static IEnumerable<T> eqSeq<T>(this IEnumerable<T> actual, IEnumerable<T> expected) { Assert.Equal(expected, actual); return actual; }

    public static bool tru(this bool actual) { Assert.True(actual); return actual; }
    public static bool fls(this bool actual) { Assert.False(actual); return actual; }
    public static bool tru(this bool actual, string because) { Assert.True(actual, because); return actual; }
    public static bool fls(this bool actual, string because) { Assert.False(actual, because); return actual; }

    public static void nul(this object? actual) => Assert.Null(actual);
    public static T notNul<T>(this T? actual) { Assert.NotNull(actual); return actual!; }

    public static T isType<T>(this object? actual) => Assert.IsType<T>(actual);
    public static T isAssignable<T>(this object? actual) => Assert.IsAssignableFrom<T>(actual);

    public static T same<T>(this T actual, T expected) where T : class? { Assert.Same(expected, actual); return actual; }
    public static T notSame<T>(this T actual, T expected) where T : class? { Assert.NotSame(expected, actual); return actual; }

    public static T empty<T>(this T actual) where T : IEnumerable { Assert.Empty(actual); return actual; }
    public static T notEmpty<T>(this T actual) where T : IEnumerable { Assert.NotEmpty(actual); return actual; }
    public static IEnumerable<T> len<T>(this IEnumerable<T> actual, int n) { var l = actual as ICollection<T> ?? [.. actual]; Assert.Equal(n, l.Count); return l; }
    public static T sole<T>(this IEnumerable<T> actual) => Assert.Single(actual);

    public static IEnumerable<T> contains<T>(this IEnumerable<T> actual, T expected) { Assert.Contains(expected, actual); return actual; }
    public static IEnumerable<T> has<T>(this IEnumerable<T> actual, Predicate<T> match) { Assert.Contains(actual, match); return actual; }
    public static string hasText(this string actual, string expected) { Assert.Contains(expected, actual); return actual; }

    public static T inRange<T>(this T actual, T low, T high) { Assert.InRange(actual, low, high, Comparer<T>.Default); return actual; }

    public static TEx throws<TEx>(this Action act) where TEx : Exception => Assert.Throws<TEx>(act);
    public static async Task<TEx> throwsAsync<TEx>(this Func<Task> act) where TEx : Exception => await Assert.ThrowsAsync<TEx>(act);
}

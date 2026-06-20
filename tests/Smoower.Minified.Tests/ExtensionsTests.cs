using Smoower.Minified.Extensions;

namespace Smoower.Minified.Tests;

public class ExtensionsTests
{
    [F]
    public void IntFactories_BuildTimeSpans()
    {
        250.ms().eq(TimeSpan.FromMilliseconds(250));
        30.secs().eq(TimeSpan.FromSeconds(30));
        5.mins().eq(TimeSpan.FromMinutes(5));
        2.hrs().eq(TimeSpan.FromHours(2));
        7.days().eq(TimeSpan.FromDays(7));
    }

    [F]
    public void Clock_ReflectsNow()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        var c = new Clock();
        c.utc.inRange(before, DateTime.UtcNow.AddSeconds(1));
        (c.unix > 0).tru();
        c.today.eq(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [F]
    public void Clk_StaticMirror()
    {
        (Clk.unix > 0).tru();
        Clk.today.eq(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [F]
    public void Env_RoundTrips()
    {
        Env.set("SMOOWER_TEST_VAR", "hello");
        Env.get("SMOOWER_TEST_VAR").eq("hello");
    }

    [F]
    public void DateExtensions_MatchBcl()
    {
        var dt = new DateTime(2026, 6, 19, 13, 45, 0, DateTimeKind.Utc);
        dt.sd().eq(dt.ToShortDateString());
        dt.ld().eq(dt.ToLongDateString());
        dt.st().eq(dt.ToShortTimeString());
        dt.lt().eq(dt.ToLongTimeString());
        dt.utc().eq(dt.ToUniversalTime());
    }
}

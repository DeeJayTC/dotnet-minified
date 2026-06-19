using Microsoft.Extensions.Logging;
using Smoower.Minified.Logging;
using Xunit;

namespace Smoower.Minified.Tests;

public class LogExtensionsTests
{
    private sealed class CapturingLogger : ILogger
    {
        public readonly List<(LogLevel Level, string Message)> Entries = [];
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => null!;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel level, EventId id, TState state, Exception? ex, Func<TState, Exception?, string> formatter)
            => Entries.Add((level, formatter(state, ex)));
    }

    [Fact]
    public void Inf_LogsInformationWithFormattedMessage()
    {
        var log = new CapturingLogger();
        log.inf("hello {Name}", "ada");
        Assert.Single(log.Entries);
        Assert.Equal(LogLevel.Information, log.Entries[0].Level);
        Assert.Equal("hello ada", log.Entries[0].Message);
    }

    [Fact]
    public void WrnErrDbg_LogAtExpectedLevels()
    {
        var log = new CapturingLogger();
        log.wrn("w");
        log.err("e");
        log.err(new InvalidOperationException("boom"), "e2");
        log.dbg("d");
        Assert.Equal(
            new[] { LogLevel.Warning, LogLevel.Error, LogLevel.Error, LogLevel.Debug },
            log.Entries.Select(e => e.Level));
    }
}

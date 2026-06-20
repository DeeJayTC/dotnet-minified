using Microsoft.Extensions.Logging;
using Smoower.Minified.Logging;

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

    [F]
    public void Inf_LogsInformationWithFormattedMessage()
    {
        var log = new CapturingLogger();
        log.inf("hello {Name}", "ada");
        log.Entries.sole();
        log.Entries[0].Level.eq(LogLevel.Information);
        log.Entries[0].Message.eq("hello ada");
    }

    [F]
    public void WrnErrDbg_LogAtExpectedLevels()
    {
        var log = new CapturingLogger();
        log.wrn("w");
        log.err("e");
        log.err(new InvalidOperationException("boom"), "e2");
        log.dbg("d");
        log.Entries.Select(e => e.Level)
            .eqSeq(new[] { LogLevel.Warning, LogLevel.Error, LogLevel.Error, LogLevel.Debug });
    }
}

﻿using Serilog.Core;
using Serilog.Events;

namespace Serilog.Tests.Support;

public class DelegatingSink : ILogEventSink
{
    readonly Action<LogEvent>? _write;

    public DelegatingSink(Action<LogEvent>? write)
    {
        _write = write ?? throw new ArgumentNullException(nameof(write));
    }

    public void Emit(LogEvent logEvent)
    {
        _write?.Invoke(logEvent);
    }

    public static LogEvent? GetLogEvent(Action<ILogger> writeAction)
    {
        LogEvent? result = null;
        var l = new LoggerConfiguration()
            .WriteTo.Sink(new DelegatingSink(le => result = le))
            .CreateLogger();

        writeAction(l);
        return result;
    }
}
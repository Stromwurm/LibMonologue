using System.Runtime.CompilerServices;

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace CmdNET.Monologue;

public static class MonologueContext
{
    private static IMonologueFormatter? _formatter;
    public static IMonologueFormatter? Formatter => _formatter;

    private static LogLevel _defaultSeverity = LogLevel.Information;
    public static LogLevel DefaultSeverity => _defaultSeverity;

    private const string _settingVariableFromToText = "Setting '{0}' from '{1}' to '{2}'.";
    public static string SettingVariableFromToText => _settingVariableFromToText;

    private static ILogger? _logger;
    public static ILogger? Logger => _logger;

    private static LoggingLevelSwitch _levelSwitch = new();
    public static LogLevel MinLoggableLevel => TranslateLogLevel(_levelSwitch.MinimumLevel);

    private static LogLevel _minValidLogLevel = LogLevel.Verbose;
    public static LogLevel MinLevel => _minValidLogLevel;

    /// <summary>
    /// Fires, when an <see cref="IMonologue"/> is committed, but the context has no <see cref="Logger"/>.
    /// </summary>
    public static EventHandler NoLogger;

    /// <summary>
    /// Fires, when an <see cref="IMonologue"/> was passed to <see cref="Logger"/>. This only happens if the context has an <see cref="ILogger"/>.
    /// </summary>
    public static EventHandler<IMonologue> MonologueCommited;

    public static void SetFormatter(IMonologueFormatter formatter)
    {
        _formatter = formatter;
    }
    public static LogEventLevel TranslateLogLevel(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Verbose:
                return LogEventLevel.Verbose;
            case LogLevel.Debug:
                return LogEventLevel.Debug;
            case LogLevel.Information:
                return LogEventLevel.Information;
            case LogLevel.Warning:
                return LogEventLevel.Warning;
            case LogLevel.Error:
                return LogEventLevel.Error;
            case LogLevel.Fatal:
                return LogEventLevel.Fatal;
            default:
                throw new NotImplementedException();
        }
    }
    public static LogLevel TranslateLogLevel(LogEventLevel level)
    {
        switch (level)
        {
            case LogEventLevel.Verbose:
                return LogLevel.Verbose;
            case LogEventLevel.Debug:
                return LogLevel.Debug;
            case LogEventLevel.Information:
                return LogLevel.Information;
            case LogEventLevel.Warning:
                return LogLevel.Warning;
            case LogEventLevel.Error:
                return LogLevel.Error;
            case LogEventLevel.Fatal:
                return LogLevel.Fatal;
            default:
                throw new NotImplementedException();
        }
    }
    public static void CreateFileLogger(string logfile)
    {
        _logger = new LoggerConfiguration()
            .WriteTo.File(logfile)
            .MinimumLevel.ControlledBy(_levelSwitch)
            .CreateLogger();
    }
    public static IMonologue CommitMonologue(IMonologue m, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0)
    {
        m.SetCaller(cfp, cmn, cln);

        if (_logger is null)
        {
            NoLogger?.Invoke(null, EventArgs.Empty);
            return m;
        }

        var level = TranslateLogLevel(m.Severity);

        if (_formatter is null)
        {
            _logger.Write(level, m.Message);

        }
        else
        {
            string s = _formatter.Format(m);
            _logger.Write(level, s);
        }

            MonologueCommited?.Invoke(null, m);

        return m;
    }
    public static IMonologue Commit(this IMonologue m, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0)
    {
        return CommitMonologue(m, cfp, cmn, cln);

    }
    public static void SetMinLogLevel(LogLevel level)
    {
        _levelSwitch.MinimumLevel = TranslateLogLevel(level);
    }
}

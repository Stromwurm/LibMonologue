using System.Runtime.CompilerServices;

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Monologue;

/// <summary>
/// The context for all operations that are not individual log messages.
/// </summary>
public static class MonologContext
{
    private static IMonologFormatter? _formatter;

    /// <summary>
    /// The <see cref="IMonologFormatter"/> to use for all logging operations.
    /// </summary>
    public static IMonologFormatter? Formatter => _formatter;

    private static LogLevel _defaultSeverity = LogLevel.Information;

    /// <summary>
    /// The default severity for all <see cref="IMonolog"/>.
    /// </summary>
    public static LogLevel DefaultSeverity => _defaultSeverity;

    private const string _settingVariableFromToText = "Setting '{0}' from '{1}' to '{2}'.";
    public static string SettingVariableFromToText => _settingVariableFromToText;

    private static ILogger? _logger;

    /// <summary>
    /// The logger used. Call <see cref="CreateFileLogger(string)"/> before calling <see cref="CommitMonolog(IMonolog, string, string, int)"/> or <see cref="Commit(IMonolog, string, string, int)"/>.
    /// </summary>
    public static ILogger? Logger => _logger;

    private static LoggingLevelSwitch _levelSwitch = new();

    /// <summary>
    /// The current log level that all <see cref="IMonolog"/> have to have to be written to log.
    /// </summary>
    public static LogLevel CurrentMinLogLevel => TranslateLogLevel(_levelSwitch.MinimumLevel);

    private static LogLevel _minValidLogLevel = LogLevel.Verbose;

    /// <summary>
    /// The lowest <see cref="LogLevel"/> possible.
    /// </summary>
    public static LogLevel MinLevel => _minValidLogLevel;

    /// <summary>
    /// Fires, when an <see cref="IMonolog"/> is committed, but the context has no <see cref="Logger"/>.
    /// </summary>
    public static EventHandler NoLogger;

    /// <summary>
    /// Fires, when an <see cref="IMonolog"/> was passed to <see cref="Logger"/>. This only happens if the context has an <see cref="ILogger"/>.
    /// </summary>
    public static EventHandler<IMonolog> MonologCommited;

    /// <summary>
    /// Sets <see cref="Formatter"/>.
    /// </summary>
    /// <param name="formatter"></param>
    public static void SetFormatter(IMonologFormatter formatter)
    {
        _formatter = formatter;
    }

    /// <summary>
    /// Translate between <see cref="LogLevel"/> and <see cref="LogEventLevel"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <returns><see cref="LogEventLevel"/> equivalent of <paramref name="level"/>.</returns>
    /// <exception cref="NotImplementedException"></exception>
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

    /// <summary>
    /// Translate between <see cref="LogEventLevel"/> and <see cref="LogLevel"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <returns><see cref="LogLevel"/> equivalent of <paramref name="level"/>.</returns>
    /// <exception cref="NotImplementedException"></exception>
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

    /// <summary>
    /// Create a logger that writes to file using <paramref name="logfile"/>.
    /// </summary>
    /// <param name="logfile"></param>
    public static void CreateFileLogger(string logfile)
    {
        _logger = new LoggerConfiguration()
            .WriteTo.File(logfile)
            .MinimumLevel.ControlledBy(_levelSwitch)
            .CreateLogger();
    }

    /// <summary>
    /// Commit <paramref name="m"/> to be logged. Sets <see cref="IMonolog.CallerFilePath"/>, <see cref="IMonolog.CallerMemberName"/> and <see cref="IMonolog.CallerLineNumber"/>.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="cfp"></param>
    /// <param name="cmn"></param>
    /// <param name="cln"></param>
    /// <returns>The modified <paramref name="m"/></returns>.
    public static IMonolog CommitMonolog(IMonolog m, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0)
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

            MonologCommited?.Invoke(null, m);

        return m;
    }

    /// <summary>
    /// Overload, see <see cref="CommitMonolog(IMonolog, string, string, int)"/> for details.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="cfp"></param>
    /// <param name="cmn"></param>
    /// <param name="cln"></param>
    /// <returns></returns>
    public static IMonolog Commit(this IMonolog m, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0)
    {
        return CommitMonolog(m, cfp, cmn, cln);

    }

    /// <summary>
    /// Sets <see cref="CurrentMinLogLevel"/>. Effective immediately.
    /// </summary>
    /// <param name="level"></param>
    public static void SetMinLogLevel(LogLevel level)
    {
        _levelSwitch.MinimumLevel = TranslateLogLevel(level);
    }
}

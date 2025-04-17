using System.Runtime.CompilerServices;

namespace CmdNET.Monologue;

public class Monolog : IMonolog
{
    private string _callerFilePath = string.Empty;
    public string CallerFilePath => _callerFilePath;

    private int _callerLineNumber = 0;
    public int CallerLineNumber => _callerLineNumber;

    private string _callerMemberName = string.Empty;
    public string CallerMemberName => _callerMemberName;

    private Guid _id = Guid.NewGuid();
    public Guid Id => _id;

    private Guid? _childOf;
    public Guid? ChildOf => _childOf;

    private string _message = string.Empty;
    public string Message => _message;

    private LogLevel _severity = MonologueContext.DefaultSeverity;
    public LogLevel Severity => _severity;

    public IMonolog LinkWith(IMonolog other)
    {
        _childOf = other.Id;
        return this;
    }

    public IMonolog SetCaller([CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0)
    {
        _callerFilePath = cfp;
        _callerLineNumber = cln;
        _callerMemberName = cmn;
        return this;
    }

    public IMonolog SetMessage(string message)
    {
        _message = message;
        return this;
    }

    public IMonolog SetMessage(string message, params object[] args)
    {
        string s = string.Format(message, args);
        _message = s;
        return this;
    }

    public IMonolog SetSeverity(LogLevel severity)
    {
        _severity = severity;
        return this;
    }
}

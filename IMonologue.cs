using System.Runtime.CompilerServices;

namespace CmdNET.Monologue
{
    public interface IMonologue
    {
        string CallerFilePath { get; }
        int CallerLineNumber { get; }
        string CallerMemberName { get; }
        Guid Id { get; }
        Guid? ChildOf { get; }
        string Message { get; }
        LogLevel Severity { get; }
        IMonologue LinkWith(IMonologue other);
        IMonologue SetCaller([CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0);
        IMonologue SetMessage(string message);
        IMonologue SetMessage(string message, params object[] args);
        IMonologue SetSeverity(LogLevel severity);
    }
}
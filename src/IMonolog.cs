using System.Runtime.CompilerServices;

namespace Monologue
{
    public interface IMonolog
    {
        string CallerFilePath { get; }
        int CallerLineNumber { get; }
        string CallerMemberName { get; }
        Guid Id { get; }
        Guid? ChildOf { get; }
        string Message { get; }
        LogLevel Severity { get; }
        IMonolog LinkWith(IMonolog other);
        IMonolog SetCaller([CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0);
        IMonolog SetMessage(string message);
        IMonolog SetMessage(string message, params object[] args);
        IMonolog SetSeverity(LogLevel severity);
    }
}
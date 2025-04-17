using System.Runtime.CompilerServices;

namespace Monologue;

/// <summary>
/// A single log message. Call the overload <see cref="MonologContext.Commit(Monologue.IMonolog, string, string, int)"/> without parameters to log.
/// </summary>
public interface IMonolog
{
    /// <summary>
    /// The assembly file where  <see cref="SetCaller"/> was called in. <see cref="MonologContext.Commit"/> calls <see cref="SetCaller"/> so you do not need to call it explicitly.
    /// </summary>
    string CallerFilePath { get; }

    /// <summary>
    /// The line number where  <see cref="SetCaller"/> was called in. <see cref="MonologContext.Commit"/> calls <see cref="SetCaller"/> so you do not need to call it explicitly.
    /// </summary>
    int CallerLineNumber { get; }

    /// <summary>
    /// The method name where  <see cref="SetCaller"/> was called in. <see cref="MonologContext.Commit"/> calls <see cref="SetCaller"/> so you do not need to call it explicitly.
    /// </summary>
    string CallerMemberName { get; }

    /// <summary>
    /// A unique id.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The unique <see cref="Id"/> of another <see cref="IMonolog"/>.
    /// </summary>
    Guid? ChildOf { get; }

    /// <summary>
    /// The message.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// The severity.
    /// </summary>
    LogLevel Severity { get; }

    /// <summary>
    /// Sets <see cref="ChildOf"/> to the <see cref="Id"/> of another <see cref="IMonolog"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    IMonolog LinkWith(IMonolog other);

    /// <summary>
    /// Gets the assembly info.
    /// </summary>
    /// <param name="cfp"></param>
    /// <param name="cmn"></param>
    /// <param name="cln"></param>
    /// <returns></returns>
    IMonolog SetCaller([CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0);

    /// <summary>
    /// Set the message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    IMonolog SetMessage(string message);

    /// <summary>
    /// Set the message using <see cref="args"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    IMonolog SetMessage(string message, params object[] args);

    /// <summary>
    /// Set the severity.
    /// </summary>
    /// <param name="severity"></param>
    /// <returns></returns>
    IMonolog SetSeverity(LogLevel severity);
}
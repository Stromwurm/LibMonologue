namespace Monologue;

/// <summary>
/// Default implementation of <see cref="IMonologFormatter"/>. Made to work with <see cref="Monolog"/>.
/// </summary>
public class MonologFormatter : IMonologFormatter
{
    public string Format(IMonolog m)
    {
        string s = "";

        if (m.ChildOf is not null)
            s = $"ID: {m.Id} LinkedTo: {m.ChildOf} ['{m.CallerFilePath} - {m.CallerMemberName}' at {m.CallerLineNumber}] {m.Message}";
        else
            s = $"ID: {m.Id} ['{m.CallerFilePath} - {m.CallerMemberName}' at {m.CallerLineNumber}] {m.Message}";
        return s;
    }
}

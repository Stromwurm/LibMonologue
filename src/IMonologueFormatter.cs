namespace Monologue;

/// <summary>
/// Formatter for <see cref="IMonolog"/>.
/// </summary>
public interface IMonologFormatter
{
    /// <summary>
    /// Format the <see cref="IMonolog"/> object data.
    /// </summary>
    /// <param name="m"></param>
    /// <returns>The formatted log string.</returns>
    string Format(IMonolog m);
}

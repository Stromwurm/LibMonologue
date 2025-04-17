namespace Monologue;

public interface ITalkative
{
    public event EventHandler<IMonolog> LogMessage;
}

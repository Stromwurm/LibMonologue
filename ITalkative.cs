namespace CmdNET.Monologue;

public interface ITalkative
{
    public event EventHandler<IMonologue> LogMessage;
}

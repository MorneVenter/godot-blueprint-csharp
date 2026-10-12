namespace Game.Core.Managers.SignalBus;

public partial class SignalBus : Node
{
    [Signal]
    public delegate void SampleSignalEventHandler(string prop);
}

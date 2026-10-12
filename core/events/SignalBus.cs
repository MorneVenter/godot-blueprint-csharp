namespace Core.Events;

public partial class SignalBus : Node
{
    [Signal]
    public delegate void SampleSignalEventHandler(string prop);
}

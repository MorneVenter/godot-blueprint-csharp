using Godot;

namespace Project.Assets.Scripts.Managers
{
    public partial class EventManager : Node
    {
        //  All global signals are registered here

        [Signal]
        public delegate void ExampleSignalEventHandler();
    }
}

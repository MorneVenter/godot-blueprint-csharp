using Godot;
using Project.Assets.Scripts.Managers;

namespace Project.Assets
{
    public static class AutoLoadExtension
    {
        public static SaveManager GetSaveManager(this Node node)
        {
            var saveManager = node.GetNode<SaveManager>("/root/SaveManager");
            return saveManager;
        }

        public static EventManager GetEventManager(this Node node)
        {
            var eventManager = node.GetNode<EventManager>("/root/EventManager");
            return eventManager;
        }
    }
}

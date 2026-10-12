using Game.Core.Managers.SaveManager;
using Game.Core.Managers.SignalBus;
using Game.Core.Managers.UIHolder;

namespace Game.Core.Extensions.AutoloadExtension;

public static class AutoLoadExtension
{
    public static SaveManager GetSaveManager(this Node node)
    {
        var saveManager = node.GetNode<SaveManager>("/root/SaveManager");
        return saveManager;
    }

    public static UIHolder GetUIHolder(this Node node)
    {
        var uiHolder = node.GetNode<UIHolder>("/root/UIHolder");
        return uiHolder;
    }

    public static SignalBus GetSignalBus(this Node node)
    {
        var signalBus = node.GetNode<SignalBus>("/root/SignalBus");
        return signalBus;
    }
}

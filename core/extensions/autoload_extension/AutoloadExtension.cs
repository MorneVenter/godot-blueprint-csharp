using Game.Core.Managers.FpsCounter;
using Game.Core.Managers.SaveManager;
using Game.Core.Managers.SignalBus;
using Game.Core.Managers.UIHolder;

namespace Game.Core.Extensions.AutoloadExtension;

public static class AutoloadExtension
{
    public static FpsCounter GetFpsCounter(this Node node) =>
        node.GetNode<FpsCounter>("/root/FpsCounter");

    public static SaveManager GetSaveManager(this Node node) =>
        node.GetNode<SaveManager>("/root/SaveManager");

    public static SignalBus GetSignalBus(this Node node) =>
        node.GetNode<SignalBus>("/root/SignalBus");

    public static UIHolder GetUIHolder(this Node node) => node.GetNode<UIHolder>("/root/UIHolder");
}

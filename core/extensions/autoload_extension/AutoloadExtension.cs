using Game.Core.Managers.FpsCounter;
using Game.Core.Managers.SaveManager;
using Game.Core.Managers.SignalBus;
using Game.Core.Managers.UIHolder;

namespace Game.Core.Extensions.AutoloadExtension;

public static class AutoloadExtension
{
    private static readonly NodePath FpsCounterPath = "/root/FpsCounter";
    private static readonly NodePath SaveManagerPath = "/root/SaveManager";
    private static readonly NodePath SignalBusPath = "/root/SignalBus";
    private static readonly NodePath UIHolderPath = "/root/UIHolder";

    public static FpsCounter GetFpsCounter(this Node node) =>
        node.GetNode<FpsCounter>(FpsCounterPath);

    public static SaveManager GetSaveManager(this Node node) =>
        node.GetNode<SaveManager>(SaveManagerPath);

    public static SignalBus GetSignalBus(this Node node) => node.GetNode<SignalBus>(SignalBusPath);

    public static UIHolder GetUIHolder(this Node node) => node.GetNode<UIHolder>(UIHolderPath);
}

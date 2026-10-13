using Game.Core.Configuration.GameConfig;

namespace Game.Core.Extensions.GameConfigExtension;

public static class AutoLoadExtension
{
    private const string ConfigPath = "res://config.tres";

    public static GameConfig GetConfig(this Node node) =>
        GD.Load<GameConfig>(ConfigPath)
        ?? throw new InvalidOperationException(
            $"Failed to load configuration resource: {ConfigPath}"
        );
}

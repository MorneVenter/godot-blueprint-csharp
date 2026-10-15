using Game.Core.Configuration.GameConfig;

namespace Game.Core.Extensions.GameConfigExtension;

public static class GameConfigExtension
{
    private const string ConfigUid = "uid://baclkaals8cyx";

    public static GameConfig GetConfig(this Node node) =>
        GD.Load<GameConfig>(ConfigUid)
        ?? throw new InvalidOperationException(
            $"Failed to load configuration resource: {ConfigUid}"
        );
}

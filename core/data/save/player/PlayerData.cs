namespace Game.Core.Data.Save.Player;

public sealed class PlayerData
{
    public string Name { get; set; } = string.Empty;

    public string CurrentScene { get; set; } = string.Empty;

    public bool TutorialCompleted { get; set; }
}

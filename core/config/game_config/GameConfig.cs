namespace Game.Core.Config.GameConfig;

[GlobalClass]
public partial class GameConfig : Resource
{
    [ExportGroup("Debug")]
    [Export]
    public bool ShowFPS { get; set; } = false;

    [ExportGroup("Save Files")]
    [Export]
    public string SaveFileEncryptionKey { get; set; } = "some_key_here";
}

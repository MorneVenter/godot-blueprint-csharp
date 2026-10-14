namespace Game.Core.Configuration.GameConfig;

[GlobalClass]
public partial class GameConfig : Resource
{
    [ExportGroup("Debug")]
    [Export]
    public bool ShowFps { get; set; }

    [ExportGroup("Save Files")]
    [Export]
    public string SaveFileEncryptionKey { get; set; } = "some_key_here";
}

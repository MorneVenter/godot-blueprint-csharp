namespace Game.Core.Configuration.GameConfig;

[GlobalClass]
public partial class GameConfig : Resource
{
    [ExportGroup("FpsCounter")]
    [Export]
    public bool ShowFps { get; set; }

    [ExportGroup("SaveManager")]
    [Export]
    public string SaveFileEncryptionKey { get; set; } = "some_key_here";
}

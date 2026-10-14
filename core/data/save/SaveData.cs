using Game.Core.Data.Save.Player;

namespace Game.Core.Data.Save;

/// <summary>
/// Game state stored in a save slot. Each section is a class in its own folder under
/// <c>core/data/save/</c>, added here initialised with <c>new()</c> so saves written before the
/// section existed still load. Use <c>{ get; set; }</c>, not <c>init</c>: the JSON source generator
/// sets missing init-only properties to null instead of keeping their initialiser.
/// </summary>
public sealed class SaveData
{
    public PlayerData Player { get; set; } = new();
}

using Game.Core.Data.Save;

namespace Game.Core.Managers.SaveManager.Serialization;

/// <summary>
/// What is written to disk: the game's <see cref="SaveData"/> plus metadata the save system owns.
/// </summary>
internal sealed record SaveFile
{
    public DateTimeOffset? LastWriteTime { get; set; }

    public SaveData Data { get; set; } = new();
}

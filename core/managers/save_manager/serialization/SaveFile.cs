namespace Game.Core.Managers.SaveManager.Serialization;

/// <summary>
/// What is written to disk: the game's data plus metadata the save system owns.
/// </summary>
public sealed record SaveFile<TData>
    where TData : class, new()
{
    public DateTimeOffset? LastWriteTime { get; set; }

    public TData Data { get; set; } = new();
}

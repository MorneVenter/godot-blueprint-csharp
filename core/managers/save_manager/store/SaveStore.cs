using Game.Core.Data.Save;
using Game.Core.Managers.SaveManager.Serialization;
using Game.Core.Managers.SaveManager.Storage;

namespace Game.Core.Managers.SaveManager.Store;

public sealed class SaveStore(ISaveStorage storage, TimeProvider timeProvider)
{
    private (int Slot, SaveFile File)? _loaded;

    public int? Slot => _loaded?.Slot;

    public SaveData Data => LoadedFile.Data;

    public DateTimeOffset? LastWriteTime => LoadedFile.LastWriteTime;

    private SaveFile LoadedFile => (_loaded ?? throw NotLoaded()).File;

    public bool SaveExists(int slot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(slot);

        return storage.Exists(slot);
    }

    /// <summary>
    /// Loads a slot, starting with empty data if it was never saved. An unreadable slot is set
    /// aside rather than lost, and also starts empty.
    /// </summary>
    public SaveLoadResult LoadSave(int slot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(slot);

        var (file, result) = storage.Exists(slot)
            ? ReadOrSetAside(slot)
            : (new SaveFile(), SaveLoadResult.New);

        _loaded = (slot, file);

        return result;
    }

    /// <summary>
    /// Writes the loaded slot to storage.
    /// </summary>
    public void Save()
    {
        var (slot, file) = _loaded ?? throw NotLoaded();
        var saved = file with { LastWriteTime = timeProvider.GetUtcNow() };

        storage.Write(slot, SaveSerializer.Serialize(saved));
        _loaded = (slot, saved);
    }

    /// <summary>
    /// Deletes a slot. Deleting the loaded slot unloads it: call LoadSave before saving again.
    /// </summary>
    public void DeleteSave(int slot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(slot);

        storage.Delete(slot);

        if (slot == Slot)
            _loaded = null;
    }

    private (SaveFile File, SaveLoadResult Result) ReadOrSetAside(int slot)
    {
        try
        {
            return (SaveSerializer.Deserialize(storage.Read(slot)), SaveLoadResult.Loaded);
        }
        catch (InvalidDataException)
        {
            storage.SetAsideBroken(slot, timeProvider.GetUtcNow());

            return (new SaveFile(), SaveLoadResult.BrokenSetAside);
        }
    }

    private static InvalidOperationException NotLoaded() =>
        new("No save slot loaded. Call LoadSave first.");
}

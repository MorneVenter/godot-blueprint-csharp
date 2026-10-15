using Game.Core.Data.Save;
using Game.Core.Managers.SaveManager.Serialization;
using Game.Core.Managers.SaveManager.Storage;
using Game.Core.Managers.SaveManager.Storage.FileSystem;
using Game.Core.Managers.SaveManager.Store;

namespace Game.Core.Managers.SaveManager;

public partial class SaveManager : Node
{
    private SaveStore<SaveData>? _store;

    public int? Slot => Store.Slot;

    public SaveData Data => Store.Data;

    public DateTimeOffset? LastWriteTime => Store.LastWriteTime;

    private SaveStore<SaveData> Store =>
        _store ?? throw new InvalidOperationException("SaveManager has not entered the tree.");

    public override void _EnterTree()
    {
        var fileSystem = new GodotFileSystem(this.GetConfig().SaveFileEncryptionKey);
        _store = new SaveStore<SaveData>(
            new FileSaveStorage(fileSystem),
            TimeProvider.System,
            SaveJsonContext.Default.SaveFileSaveData
        );
    }

    public bool SaveExists(int slot) => Store.SaveExists(slot);

    /// <inheritdoc cref="SaveStore{TData}.LoadSave"/>
    public SaveLoadResult LoadSave(int slot)
    {
        var result = Store.LoadSave(slot);

        if (result == SaveLoadResult.BrokenSetAside)
            GD.PushWarning(
                $"Save slot {slot} was unreadable; set aside as *_BROKEN_* and started empty."
            );

        return result;
    }

    /// <inheritdoc cref="SaveStore{TData}.Save"/>
    public void Save() => Store.Save();

    /// <inheritdoc cref="SaveStore{TData}.DeleteSave"/>
    public void DeleteSave(int slot) => Store.DeleteSave(slot);
}

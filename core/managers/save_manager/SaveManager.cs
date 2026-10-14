using Game.Core.Data.Save;
using Game.Core.Managers.SaveManager.Storage;
using Game.Core.Managers.SaveManager.Storage.FileSystem;
using Game.Core.Managers.SaveManager.Store;

namespace Game.Core.Managers.SaveManager;

/// <summary>
/// Autoload exposing <see cref="SaveStore"/> backed by encrypted files.
/// </summary>
public partial class SaveManager : Node
{
    private const int DefaultSlot = 1;

    private SaveStore? _store;

    public int? Slot => Store.Slot;

    public SaveData Data => Store.Data;

    public DateTimeOffset? LastWriteTime => Store.LastWriteTime;

    private SaveStore Store =>
        _store ?? throw new InvalidOperationException("SaveManager has not entered the tree.");

    public override void _EnterTree()
    {
        var fileSystem = new GodotEncryptedFileSystem(this.GetConfig().SaveFileEncryptionKey);
        _store = new SaveStore(new FileSaveStorage(fileSystem), TimeProvider.System);
        LoadSave(DefaultSlot);
    }

    public bool SaveExists(int slot) => Store.SaveExists(slot);

    /// <inheritdoc cref="SaveStore.LoadSave"/>
    public SaveLoadResult LoadSave(int slot)
    {
        var result = Store.LoadSave(slot);

        if (result == SaveLoadResult.BrokenSetAside)
            GD.PushWarning(
                $"Save slot {slot} was unreadable; set aside as *_BROKEN_* and started empty."
            );

        return result;
    }

    /// <inheritdoc cref="SaveStore.Save"/>
    public void Save() => Store.Save();

    /// <inheritdoc cref="SaveStore.DeleteSave"/>
    public void DeleteSave(int slot) => Store.DeleteSave(slot);
}

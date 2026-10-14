using System.Globalization;
using Godot.Collections;
using FileAccess = Godot.FileAccess;

namespace Game.Core.Managers.SaveManager;

public partial class SaveManager : Node
{
    private const string UserFolder = "user://";
    private const string SaveFolder = "savedata";
    private const string DateKey = "LAST_WRITE_DATE_TIME";
    private const int DefaultSlot = 1;

    private string _key = string.Empty;
    private int? _slot;
    private Dictionary _data = [];

    public override void _EnterTree()
    {
        _key = this.GetConfig().SaveFileEncryptionKey;
        LoadSave(DefaultSlot);
    }

    private static string GetSavePath(int slot) =>
        $"{UserFolder}{SaveFolder}/save_{slot.ToString("D3", CultureInfo.InvariantCulture)}.save";

    private void CreateSaveIfNotExists(string path)
    {
        if (FileAccess.FileExists(path))
            return;

        using var directory =
            DirAccess.Open(UserFolder)
            ?? throw new IOException($"Could not open save directory: {UserFolder}");

        var directoryError = directory.MakeDirRecursive(SaveFolder);

        if (directoryError != Error.Ok)
            throw new IOException(
                $"Could not create save directory '{SaveFolder}': {directoryError}"
            );

        WriteSaveFile(path, []);
    }

    private void WriteSaveFile(string path, Dictionary data)
    {
        using var saveFile =
            FileAccess.OpenEncryptedWithPass(path, FileAccess.ModeFlags.Write, _key)
            ?? throw new IOException($"Could not write save file '{path}'.");

        saveFile.StoreVar(data);
    }

    private Dictionary ReadSaveFile(string path)
    {
        using var saveFile =
            FileAccess.OpenEncryptedWithPass(path, FileAccess.ModeFlags.Read, _key)
            ?? throw new IOException(
                $"Could not open save file '{path}'. The encryption key may be invalid."
            );

        var loadedData = saveFile.GetVar();

        if (loadedData.VariantType != Variant.Type.Dictionary)
            throw new InvalidDataException($"Save file contains invalid data: {path}");

        return loadedData.AsGodotDictionary();
    }

    /// <summary>
    /// Loads a save slot, creating the save file if it does not exist.
    /// </summary>
    public void LoadSave(int slot)
    {
        var savePath = GetSavePath(slot);
        CreateSaveIfNotExists(savePath);

        _data = ReadSaveFile(savePath);
        _slot = slot;
    }

    /// <summary>
    /// Deletes a save slot. Deleting the loaded slot unloads it: call LoadSave before saving again.
    /// </summary>
    public void DeleteSave(int slot)
    {
        var savePath = GetSavePath(slot);
        var error = DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(savePath));

        if (error != Error.Ok && error != Error.FileNotFound)
            throw new IOException($"Could not delete save file '{savePath}': {error}");

        if (slot != _slot)
            return;

        _slot = null;
        _data = [];
    }

    /// <summary>
    /// Saves a value and writes the slot to disk. Nil values are ignored.
    /// </summary>
    public void SaveValue(string key, Variant value)
    {
        if (value.VariantType == Variant.Type.Nil)
            return;

        var slot =
            _slot
            ?? throw new InvalidOperationException("No save slot loaded. Call LoadSave first.");

        _data[key] = value;
        _data[DateKey] = Time.GetDatetimeStringFromSystem();
        WriteSaveFile(GetSavePath(slot), _data);
    }

    /// <summary>
    /// Loads a value by key, returning the default when missing or nil.
    /// </summary>
    public Variant LoadValue(string key, Variant defaultValue = default)
    {
        if (!_data.TryGetValue(key, out var value))
            return defaultValue;

        return value.VariantType == Variant.Type.Nil ? defaultValue : value;
    }

    /// <summary>
    /// Gets the last-write timestamp (YYYY-MM-DDTHH:MM:SS), or an empty string if the slot was never written.
    /// </summary>
    public string GetLastWriteDate() => LoadValue(DateKey, string.Empty).As<string>();
}

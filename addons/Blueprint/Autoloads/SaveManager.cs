using System.Globalization;

namespace Blueprint;

public partial class SaveManager : Node
{
    private const string UserFolder = "user://";
    private const string SaveFolder = "savedata";
    private const string DateKey = "LAST_WRITE_DATE_TIME";

    private string _key = string.Empty;
    private int _saveId = 1;

    private readonly Dictionary _defaultValues = new() { [DateKey] = "-" };

    private Dictionary _data = [];

    public override void _EnterTree()
    {
        _key = ProjectSettings.GetSetting(Blueprint.EncryptionKeySetting).As<string>();
        LoadSaveData();
    }

    private static string GetSavePath(int id) =>
        $"{UserFolder}{SaveFolder}/save_{id.ToString("D3", CultureInfo.InvariantCulture)}.save";

    private string GetSavePath() => GetSavePath(_saveId);

    private void CreateSaveIfNotExists(string path)
    {
        if (!FileAccess.FileExists(path))
            InitializeNewSaveFile(path);
    }

    private void InitializeNewSaveFile(string path)
    {
        using var directory =
            DirAccess.Open(UserFolder)
            ?? throw new System.Exception($"Could not open save directory: {UserFolder}");

        var directoryError = directory.MakeDirRecursive(SaveFolder);

        if (directoryError != Error.Ok)
            throw new System.Exception(
                $"Could not create save directory '{SaveFolder}': {directoryError}"
            );

        using var saveFile =
            FileAccess.OpenEncryptedWithPass(path, FileAccess.ModeFlags.Write, _key)
            ?? throw new System.Exception($"Could not create encrypted save file: {path}");

        saveFile.StoreVar(_defaultValues);
    }

    private void LoadSaveData()
    {
        var savePath = GetSavePath();
        CreateSaveIfNotExists(savePath);

        using var saveFile =
            FileAccess.OpenEncryptedWithPass(savePath, FileAccess.ModeFlags.Read, _key)
            ?? throw new InvalidOperationException(
                $"Could not open save file '{savePath}'. The encryption key may be invalid."
            );

        var loadedData = saveFile.GetVar();

        if (loadedData.VariantType != Variant.Type.Dictionary)
            throw new System.IO.InvalidDataException(
                $"Save file contains invalid data: {savePath}"
            );

        _data = loadedData.AsGodotDictionary();
    }

    private void UpdateSaveFile()
    {
        var savePath = GetSavePath();
        CreateSaveIfNotExists(savePath);

        using var saveFile =
            FileAccess.OpenEncryptedWithPass(savePath, FileAccess.ModeFlags.Write, _key)
            ?? throw new InvalidOperationException(
                $"Could not open save file '{savePath}'. The encryption key may be invalid."
            );

        saveFile.StoreVar(_data);
    }

    private static void DeleteSaveData(int id)
    {
        var savePath = GetSavePath(id);
        var absolutePath = ProjectSettings.GlobalizePath(savePath);
        var error = DirAccess.RemoveAbsolute(absolutePath);

        if (error != Error.Ok && error != Error.FileNotFound)
            throw new System.IO.IOException($"Could not delete save file '{savePath}': {error}");
    }

    private void UnloadAndReset()
    {
        _data = [];
        _saveId = 1;
    }

    /// <summary>
    /// Deletes a save slot. If it is active, the loaded data is cleared.
    /// Call LoadSave to load a save file again.
    /// </summary>
    public void DeleteSave(int slot)
    {
        DeleteSaveData(slot);

        if (slot == _saveId)
            UnloadAndReset();
    }

    /// <summary>
    /// Loads a save slot, creating the save file if it does not exist.
    /// </summary>
    public void LoadSave(int slot)
    {
        _saveId = slot;
        LoadSaveData();
    }

    /// <summary>
    /// Saves a value under a unique key and updates the last-write timestamp.
    /// Nil values are ignored.
    /// </summary>
    public void SaveValue(string key, Variant value)
    {
        if (value.VariantType == Variant.Type.Nil)
            return;

        _data[key] = value;
        _data[DateKey] = Time.GetDatetimeStringFromSystem();
        UpdateSaveFile();
    }

    /// <summary>
    /// Loads a value by key, returning the default when missing or nil.
    /// </summary>
    public Variant LoadValue(string key, Variant defaultValue = default)
    {
        if (!_data.ContainsKey(key))
            return defaultValue;

        var value = _data[key];

        return value.VariantType == Variant.Type.Nil ? defaultValue : value;
    }

    /// <summary>
    /// Gets the last-write timestamp in YYYY-MM-DDTHH:MM:SS format.
    /// </summary>
    public string GetLastWriteDate() => LoadValue(DateKey, string.Empty).As<string>();
}

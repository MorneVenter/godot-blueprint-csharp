using System.Globalization;
using Game.Core.Managers.SaveManager.Storage.FileSystem;

namespace Game.Core.Managers.SaveManager.Storage;

/// <summary>
/// One file per slot at <c>user://savedata/save_NNN.save</c>. Writes never overwrite a save in
/// place: the new contents go to a <c>.tmp</c> file that is swapped in, so a crash at any point
/// leaves either the old or the new save, and the next access finishes or rolls back the swap.
/// </summary>
public sealed class FileSaveStorage(IFileSystem fileSystem) : ISaveStorage
{
    private const string Folder = "user://savedata";
    private const string Extension = ".save";
    private const string NewSuffix = ".tmp";
    private const string OldSuffix = ".old";
    private const string BrokenMarker = "_BROKEN_";
    private const string BrokenTimeFormat = "yyyyMMdd'T'HHmmss'Z'";

    public bool Exists(int slot)
    {
        Recover(slot);

        return fileSystem.Exists(SavePath(slot));
    }

    public string Read(int slot)
    {
        Recover(slot);

        return fileSystem.Read(SavePath(slot));
    }

    public void Write(int slot, string contents)
    {
        Recover(slot);

        var (path, newPath, oldPath) = Paths(slot);
        var hadSave = fileSystem.Exists(path);

        fileSystem.Write(newPath, contents);
        VerifyWritten(newPath, contents);

        if (hadSave)
            fileSystem.Move(path, oldPath);

        fileSystem.Move(newPath, path);

        if (hadSave)
            fileSystem.Delete(oldPath);
    }

    public void Delete(int slot)
    {
        Recover(slot);

        var path = SavePath(slot);

        if (fileSystem.Exists(path))
            fileSystem.Delete(path);
    }

    public void SetAsideBroken(int slot, DateTimeOffset brokenAt)
    {
        Recover(slot);

        var time = brokenAt.UtcDateTime.ToString(BrokenTimeFormat, CultureInfo.InvariantCulture);
        var target = BrokenPath(slot, time, copy: 1);

        for (var copy = 2; fileSystem.Exists(target); copy++)
            target = BrokenPath(slot, time, copy);

        fileSystem.Move(SavePath(slot), target);
    }

    private void VerifyWritten(string path, string contents)
    {
        try
        {
            if (string.Equals(fileSystem.Read(path), contents, StringComparison.Ordinal))
                return;
        }
        catch (InvalidDataException exception)
        {
            throw new IOException($"Save did not write correctly: {path}", exception);
        }

        throw new IOException($"Save did not write correctly: {path}");
    }

    private void Recover(int slot)
    {
        var (path, newPath, oldPath) = Paths(slot);

        if (!fileSystem.Exists(path) && fileSystem.Exists(oldPath))
            fileSystem.Move(fileSystem.Exists(newPath) ? newPath : oldPath, path);

        if (fileSystem.Exists(newPath))
            fileSystem.Delete(newPath);

        if (fileSystem.Exists(oldPath))
            fileSystem.Delete(oldPath);
    }

    private static string SavePath(int slot) => $"{Folder}/{SlotName(slot)}{Extension}";

    private static (string Save, string New, string Old) Paths(int slot)
    {
        var path = SavePath(slot);

        return (path, path + NewSuffix, path + OldSuffix);
    }

    private static string BrokenPath(int slot, string time, int copy)
    {
        var copySuffix = copy == 1 ? "" : "_" + copy.ToString(CultureInfo.InvariantCulture);

        return $"{Folder}/{SlotName(slot)}{BrokenMarker}{time}{copySuffix}{Extension}";
    }

    private static string SlotName(int slot) =>
        $"save_{slot.ToString("D3", CultureInfo.InvariantCulture)}";
}

using FileAccess = Godot.FileAccess;

namespace Game.Core.Managers.SaveManager.Storage.FileSystem;

public sealed class GodotEncryptedFileSystem(string encryptionKey) : IFileSystem
{
    public bool Exists(string path) => FileAccess.FileExists(path);

    public string Read(string path)
    {
        using var file = FileAccess.OpenEncryptedWithPass(
            path,
            FileAccess.ModeFlags.Read,
            encryptionKey
        );

        if (file is not null)
            return file.GetAsText();

        var error = FileAccess.GetOpenError();
        var message = $"Could not open '{path}': {error}";

        throw error is Error.FileCorrupt or Error.FileUnrecognized
            ? new InvalidDataException(message)
            : new IOException(message);
    }

    public void Write(string path, string contents)
    {
        var folder = path.GetBaseDir();
        ThrowOnError(DirAccess.MakeDirRecursiveAbsolute(folder), $"create folder '{folder}'");

        using var file =
            FileAccess.OpenEncryptedWithPass(path, FileAccess.ModeFlags.Write, encryptionKey)
            ?? throw new IOException($"Could not write '{path}': {FileAccess.GetOpenError()}");

        if (!file.StoreString(contents))
            throw new IOException($"Could not write '{path}': {file.GetError()}");

        file.Close();
    }

    public void Move(string from, string to) =>
        ThrowOnError(DirAccess.RenameAbsolute(from, to), $"move '{from}' to '{to}'");

    public void Delete(string path) =>
        ThrowOnError(DirAccess.RemoveAbsolute(path), $"delete '{path}'");

    private static void ThrowOnError(Error error, string action)
    {
        if (error != Error.Ok)
            throw new IOException($"Could not {action}: {error}");
    }
}

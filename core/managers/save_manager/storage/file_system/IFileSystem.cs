namespace Game.Core.Managers.SaveManager.Storage.FileSystem;

public interface IFileSystem
{
    bool Exists(string path);

    /// <summary>
    /// Reads a whole file. Throws <see cref="InvalidDataException"/> if its contents are corrupt.
    /// </summary>
    string Read(string path);

    /// <summary>
    /// Creates or replaces a file. May not report every failure: read it back to be sure.
    /// </summary>
    void Write(string path, string contents);

    /// <summary>
    /// Renames a file. The destination must not exist.
    /// </summary>
    void Move(string from, string to);

    void Delete(string path);
}

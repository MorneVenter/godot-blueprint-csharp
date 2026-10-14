using Game.Core.Managers.SaveManager.Storage.FileSystem;

namespace Game.Tests.Fakes;

internal sealed class InMemoryFileSystem : IFileSystem
{
    private int _changes;

    public SortedDictionary<string, string> Files { get; } = new(StringComparer.Ordinal);

    public int? CrashAfterChanges { get; set; }

    public bool TruncateWrites { get; set; }

    public bool Exists(string path) => Files.ContainsKey(path);

    public string Read(string path) =>
        Files.TryGetValue(path, out var contents)
            ? contents
            : throw new FileNotFoundException(path);

    public void Write(string path, string contents) =>
        Change(() => Files[path] = TruncateWrites ? contents[..(contents.Length / 2)] : contents);

    public void Move(string from, string to) =>
        Change(() =>
        {
            if (Files.ContainsKey(to))
                throw new IOException($"Destination exists: {to}");

            if (!Files.Remove(from, out var contents))
                throw new FileNotFoundException(from);

            Files[to] = contents;
        });

    public void Delete(string path) =>
        Change(() =>
        {
            if (!Files.Remove(path))
                throw new FileNotFoundException(path);
        });

    private void Change(Action change)
    {
        if (_changes == CrashAfterChanges)
            throw new IOException("Simulated crash.");

        change();
        _changes++;
    }
}

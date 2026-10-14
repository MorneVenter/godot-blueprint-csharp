using Game.Core.Managers.SaveManager.Storage;

namespace Game.Tests.Fakes;

internal sealed class InMemorySaveStorage : ISaveStorage
{
    public Dictionary<int, string> Slots { get; } = [];

    public List<(int Slot, string Contents, DateTimeOffset BrokenAt)> SetAside { get; } = [];

    public Exception? WriteFailure { get; set; }

    public Exception? ReadFailure { get; set; }

    public bool Exists(int slot) => Slots.ContainsKey(slot);

    public string Read(int slot) => ReadFailure is null ? Slots[slot] : throw ReadFailure;

    public void Write(int slot, string contents)
    {
        if (WriteFailure is not null)
            throw WriteFailure;

        Slots[slot] = contents;
    }

    public void Delete(int slot) => Slots.Remove(slot);

    public void SetAsideBroken(int slot, DateTimeOffset brokenAt)
    {
        SetAside.Add((slot, Slots[slot], brokenAt));
        Slots.Remove(slot);
    }
}

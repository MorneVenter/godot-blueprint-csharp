namespace Game.Core.Managers.SaveManager.Storage;

public interface ISaveStorage
{
    bool Exists(int slot);

    string Read(int slot);

    void Write(int slot, string contents);

    void Delete(int slot);

    void SetAsideBroken(int slot, DateTimeOffset brokenAt);
}

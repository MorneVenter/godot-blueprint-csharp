using Game.Core.Managers.SaveManager.Store;
using Game.Tests.Fakes;

namespace Game.Tests.Core.Managers.SaveManager.Store;

[TestFixture]
public class SaveStoreTests
{
    private const int Slot = 1;
    private const int OtherSlot = 2;
    private const string Name = "Ada";
    private const string Scene = "res://scenes/Main.tscn";

    private static readonly DateTimeOffset Now = new(2026, 1, 2, 3, 4, 5, TimeSpan.Zero);

    private InMemorySaveStorage _storage = null!;
    private FixedTimeProvider _time = null!;
    private SaveStore _store = null!;

    [SetUp]
    public void SetUp()
    {
        _storage = new InMemorySaveStorage();
        _time = new FixedTimeProvider(Now);
        _store = CreateStore();
    }

    private SaveStore CreateStore() => new(_storage, _time);

    [Test]
    public void NothingIsLoadedInitially()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_store.Slot, Is.Null);
            Assert.That(() => _store.Data, Throws.InvalidOperationException);
            Assert.That(() => _store.LastWriteTime, Throws.InvalidOperationException);
            Assert.That(_store.Save, Throws.InvalidOperationException);
        });
    }

    [Test]
    public void LoadingAMissingSlotStartsEmptyWithoutWriting()
    {
        var result = _store.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(SaveLoadResult.New));
            Assert.That(_store.Slot, Is.EqualTo(Slot));
            Assert.That(_store.Data.Player.Name, Is.Empty);
            Assert.That(_store.LastWriteTime, Is.Null);
            Assert.That(_storage.Slots, Is.Empty);
        });
    }

    [Test]
    public void SaveWritesTheLoadedSlotAndStampsTheTime()
    {
        _store.LoadSave(Slot);
        _store.Save();

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Slots.Keys, Is.EquivalentTo(new[] { Slot }));
            Assert.That(_store.LastWriteTime, Is.EqualTo(Now));
        });
    }

    [Test]
    public void EverySaveUpdatesTheTime()
    {
        var later = Now.AddHours(1);
        _store.LoadSave(Slot);
        _store.Save();

        _time.Now = later;
        _store.Save();

        Assert.That(_store.LastWriteTime, Is.EqualTo(later));
    }

    [Test]
    public void SavedDataReloadsInANewStore()
    {
        _store.LoadSave(Slot);
        var player = _store.Data.Player;
        player.Name = Name;
        player.CurrentScene = Scene;
        player.TutorialCompleted = true;
        _store.Save();

        var reloaded = CreateStore();
        var result = reloaded.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(SaveLoadResult.Loaded));
            Assert.That(reloaded.Data.Player.Name, Is.EqualTo(Name));
            Assert.That(reloaded.Data.Player.CurrentScene, Is.EqualTo(Scene));
            Assert.That(reloaded.Data.Player.TutorialCompleted, Is.True);
            Assert.That(reloaded.LastWriteTime, Is.EqualTo(Now));
        });
    }

    [Test]
    public void DataStaysTheSameObjectAcrossSaves()
    {
        _store.LoadSave(Slot);
        var player = _store.Data.Player;

        _store.Save();
        player.Name = Name;
        _store.Save();

        var reloaded = CreateStore();
        reloaded.LoadSave(Slot);

        Assert.That(reloaded.Data.Player.Name, Is.EqualTo(Name));
    }

    [Test]
    public void UnsavedChangesAreLostOnReload()
    {
        _store.LoadSave(Slot);
        _store.Save();
        _store.Data.Player.Name = Name;

        _store.LoadSave(Slot);

        Assert.That(_store.Data.Player.Name, Is.Empty);
    }

    [Test]
    public void FailedSaveKeepsTheLastWriteTime()
    {
        _store.LoadSave(Slot);
        _store.Save();
        _time.Now = Now.AddHours(1);
        _storage.WriteFailure = new IOException("Disk full.");

        Assert.Multiple(() =>
        {
            Assert.That(_store.Save, Throws.TypeOf<IOException>());
            Assert.That(_store.LastWriteTime, Is.EqualTo(Now));
        });
    }

    [Test]
    [SetCulture("en-ZA")]
    public void SavingIgnoresSystemCulture()
    {
        _store.LoadSave(Slot);
        _store.Save();

        var reloaded = CreateStore();
        reloaded.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Slots[Slot], Does.Contain("2026-01-02T03:04:05"));
            Assert.That(reloaded.LastWriteTime, Is.EqualTo(Now));
        });
    }

    [Test]
    public void LoadingAnotherSlotSwitchesData()
    {
        _store.LoadSave(Slot);
        _store.Save();
        var firstData = _store.Data;

        _store.LoadSave(OtherSlot);

        Assert.Multiple(() =>
        {
            Assert.That(_store.Slot, Is.EqualTo(OtherSlot));
            Assert.That(_store.Data, Is.Not.SameAs(firstData));
            Assert.That(_store.Data.Player.Name, Is.Empty);
            Assert.That(_store.LastWriteTime, Is.Null);
        });
    }

    [Test]
    public void SaveExistsOnlyAfterSaving()
    {
        _store.LoadSave(Slot);
        var existedBeforeSave = _store.SaveExists(Slot);

        _store.Save();

        Assert.Multiple(() =>
        {
            Assert.That(existedBeforeSave, Is.False);
            Assert.That(_store.SaveExists(Slot), Is.True);
            Assert.That(_store.SaveExists(OtherSlot), Is.False);
        });
    }

    [Test]
    public void DeletingTheLoadedSlotUnloadsIt()
    {
        _store.LoadSave(Slot);
        _store.Save();

        _store.DeleteSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Slots, Is.Empty);
            Assert.That(_store.Slot, Is.Null);
            Assert.That(_store.Save, Throws.InvalidOperationException);
        });
    }

    [Test]
    public void DeletingAnotherSlotKeepsTheLoadedOne()
    {
        _store.LoadSave(OtherSlot);
        _store.Save();
        _store.LoadSave(Slot);

        _store.DeleteSave(OtherSlot);

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Slots, Does.Not.ContainKey(OtherSlot));
            Assert.That(_store.Slot, Is.EqualTo(Slot));
            Assert.That(_store.Save, Throws.Nothing);
        });
    }

    [Test]
    public void NegativeSlotsAreRejected()
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => _store.LoadSave(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => _store.SaveExists(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => _store.DeleteSave(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        });
    }

    [Test]
    public void SavesWithUnknownFieldsStillLoad()
    {
        _storage.Slots[Slot] =
            """{"Removed":1,"Data":{"OldSection":{},"Player":{"Name":"Ada","Old":true}}}""";

        _store.LoadSave(Slot);

        Assert.That(_store.Data.Player.Name, Is.EqualTo(Name));
    }

    [Test]
    [TestCase("{}")]
    [TestCase("""{"Data":{}}""")]
    public void MissingSectionsLoadWithDefaults(string contents)
    {
        _storage.Slots[Slot] = contents;

        _store.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(_store.Data.Player.Name, Is.Empty);
            Assert.That(_store.Data.Player.TutorialCompleted, Is.False);
            Assert.That(_store.LastWriteTime, Is.Null);
        });
    }

    [Test]
    [TestCase("")]
    [TestCase("not json")]
    [TestCase("null")]
    [TestCase("""{"Data":null}""")]
    [TestCase("""{"Data":{"Player":null}}""")]
    [TestCase("""{"Data":{"Player":{"Name":null}}}""")]
    [TestCase("""{"LastWriteTime":"yesterday"}""")]
    public void InvalidSavesAreSetAsideAndTheSlotStartsEmpty(string contents)
    {
        _storage.Slots[Slot] = contents;

        var result = _store.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(SaveLoadResult.BrokenSetAside));
            Assert.That(_storage.SetAside, Is.EqualTo(new[] { (Slot, contents, Now) }));
            Assert.That(_store.Slot, Is.EqualTo(Slot));
            Assert.That(_store.Data.Player.Name, Is.Empty);
            Assert.That(_store.LastWriteTime, Is.Null);
            Assert.That(_store.SaveExists(Slot), Is.False);
        });
    }

    [Test]
    public void CorruptFilesAreSetAside()
    {
        _storage.Slots[Slot] = "encrypted bytes";
        _storage.ReadFailure = new InvalidDataException("Checksum mismatch.");

        var result = _store.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(SaveLoadResult.BrokenSetAside));
            Assert.That(_storage.SetAside, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void SavingAfterSettingAsideKeepsTheBrokenSave()
    {
        _storage.Slots[Slot] = "not json";
        _store.LoadSave(Slot);

        _store.Data.Player.Name = Name;
        _store.Save();

        var reloaded = CreateStore();
        reloaded.LoadSave(Slot);

        Assert.Multiple(() =>
        {
            Assert.That(reloaded.Data.Player.Name, Is.EqualTo(Name));
            Assert.That(_storage.SetAside.Single().Contents, Is.EqualTo("not json"));
        });
    }

    [Test]
    public void ReadErrorsAreNotTreatedAsBroken()
    {
        _store.LoadSave(OtherSlot);
        _storage.Slots[Slot] = "{}";
        _storage.ReadFailure = new IOException("Disk unavailable.");

        Assert.Multiple(() =>
        {
            Assert.That(() => _store.LoadSave(Slot), Throws.TypeOf<IOException>());
            Assert.That(_storage.SetAside, Is.Empty);
            Assert.That(_storage.Slots, Does.ContainKey(Slot));
            Assert.That(_store.Slot, Is.EqualTo(OtherSlot));
        });
    }
}

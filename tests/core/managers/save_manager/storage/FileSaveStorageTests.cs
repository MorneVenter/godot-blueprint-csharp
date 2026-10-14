using Game.Core.Managers.SaveManager.Storage;
using Game.Tests.Fakes;

namespace Game.Tests.Core.Managers.SaveManager.Storage;

[TestFixture]
public class FileSaveStorageTests
{
    private const int Slot = 1;
    private const string SavePath = "user://savedata/save_001.save";
    private const string NewPath = SavePath + ".tmp";
    private const string OldPath = SavePath + ".old";
    private const string BrokenPath = "user://savedata/save_001_BROKEN_20260102T030405Z.save";
    private const string OldContents = "old";
    private const string NewContents = "new";

    private static readonly DateTimeOffset BrokenAt = new(2026, 1, 2, 3, 4, 5, TimeSpan.Zero);

    private InMemoryFileSystem _fileSystem = null!;
    private FileSaveStorage _storage = null!;

    [SetUp]
    public void SetUp()
    {
        _fileSystem = new InMemoryFileSystem();
        _storage = new FileSaveStorage(_fileSystem);
    }

    [Test]
    [TestCase(0, "user://savedata/save_000.save")]
    [TestCase(7, "user://savedata/save_007.save")]
    [TestCase(1234, "user://savedata/save_1234.save")]
    public void SlotsMapToNumberedFiles(int slot, string path)
    {
        _storage.Write(slot, NewContents);

        Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { path }));
    }

    [Test]
    [SetCulture("ar-SA")]
    public void FileNamesIgnoreSystemCulture()
    {
        _storage.Write(Slot, NewContents);
        _storage.SetAsideBroken(Slot, BrokenAt);

        Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { BrokenPath }));
    }

    [Test]
    public void WriteCreatesTheSave()
    {
        _storage.Write(Slot, NewContents);

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Exists(Slot), Is.True);
            Assert.That(_storage.Read(Slot), Is.EqualTo(NewContents));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    public void WriteReplacesTheSaveWithoutLeftovers()
    {
        _fileSystem.Files[SavePath] = OldContents;

        _storage.Write(Slot, NewContents);

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Read(Slot), Is.EqualTo(NewContents));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    // A replacing write makes four changes: write .tmp, move save to .old, move .tmp to save,
    // delete .old. Crashing before the third leaves the old save; from then on, the new one.
    [Test]
    [TestCase(0, OldContents)]
    [TestCase(1, OldContents)]
    [TestCase(2, NewContents)]
    [TestCase(3, NewContents)]
    public void CrashDuringWriteLeavesAWholeSave(int changesBeforeCrash, string expected)
    {
        _fileSystem.Files[SavePath] = OldContents;
        _fileSystem.CrashAfterChanges = changesBeforeCrash;

        Assert.That(() => _storage.Write(Slot, NewContents), Throws.TypeOf<IOException>());

        _fileSystem.CrashAfterChanges = null;
        var restarted = new FileSaveStorage(_fileSystem);

        Assert.Multiple(() =>
        {
            Assert.That(restarted.Read(Slot), Is.EqualTo(expected));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    public void CrashDuringFirstWriteLeavesNoSave(int changesBeforeCrash)
    {
        _fileSystem.CrashAfterChanges = changesBeforeCrash;

        Assert.That(() => _storage.Write(Slot, NewContents), Throws.TypeOf<IOException>());

        _fileSystem.CrashAfterChanges = null;
        var restarted = new FileSaveStorage(_fileSystem);

        Assert.Multiple(() =>
        {
            Assert.That(restarted.Exists(Slot), Is.False);
            Assert.That(_fileSystem.Files, Is.Empty);
        });
    }

    [Test]
    public void LoneTempFileIsDiscardedAsUncommitted()
    {
        _fileSystem.Files[NewPath] = "partial";

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Exists(Slot), Is.False);
            Assert.That(_fileSystem.Files, Is.Empty);
        });
    }

    [Test]
    public void SilentlyFailedWriteNeverReplacesTheSave()
    {
        _fileSystem.Files[SavePath] = OldContents;
        _fileSystem.TruncateWrites = true;

        Assert.That(() => _storage.Write(Slot, NewContents), Throws.TypeOf<IOException>());

        _fileSystem.TruncateWrites = false;

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Read(Slot), Is.EqualTo(OldContents));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    public void UncommittedTempFileIsDiscarded()
    {
        _fileSystem.Files[SavePath] = OldContents;
        _fileSystem.Files[NewPath] = "partial";

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Read(Slot), Is.EqualTo(OldContents));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    public void TempFileIsPromotedWhenTheSaveWasMovedAside()
    {
        _fileSystem.Files[OldPath] = OldContents;
        _fileSystem.Files[NewPath] = NewContents;

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Read(Slot), Is.EqualTo(NewContents));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    public void LeftoverOldFileIsRemovedOnceTheNewSaveIsInPlace()
    {
        _fileSystem.Files[SavePath] = NewContents;
        _fileSystem.Files[OldPath] = OldContents;

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Read(Slot), Is.EqualTo(NewContents));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    public void LoneOldFileIsRestoredRatherThanDeleted()
    {
        _fileSystem.Files[OldPath] = OldContents;

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Exists(Slot), Is.True);
            Assert.That(_storage.Read(Slot), Is.EqualTo(OldContents));
        });
    }

    [Test]
    public void RecoveryRunsBeforeWriting()
    {
        _fileSystem.Files[OldPath] = OldContents;
        _fileSystem.Files[NewPath] = NewContents;

        _storage.Write(Slot, "newer");

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Read(Slot), Is.EqualTo("newer"));
            Assert.That(_fileSystem.Files.Keys, Is.EqualTo(new[] { SavePath }));
        });
    }

    [Test]
    public void DeleteRemovesTheSaveAndLeftovers()
    {
        _fileSystem.Files[SavePath] = OldContents;
        _fileSystem.Files[NewPath] = "partial";

        _storage.Delete(Slot);

        Assert.That(_fileSystem.Files, Is.Empty);
    }

    [Test]
    public void DeletingAMissingSlotDoesNothing()
    {
        Assert.That(() => _storage.Delete(Slot), Throws.Nothing);
    }

    [Test]
    public void BrokenSaveIsKeptUnderATimestampedName()
    {
        _fileSystem.Files[SavePath] = OldContents;

        _storage.SetAsideBroken(Slot, BrokenAt);

        Assert.Multiple(() =>
        {
            Assert.That(_storage.Exists(Slot), Is.False);
            Assert.That(
                _fileSystem.Files,
                Is.EqualTo(
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        [BrokenPath] = OldContents,
                    }
                )
            );
        });
    }

    [Test]
    public void BrokenSavesSetAsideAtTheSameTimeNeverOverwriteEachOther()
    {
        for (var index = 1; index <= 3; index++)
        {
            _fileSystem.Files[SavePath] = $"broken {index}";
            _storage.SetAsideBroken(Slot, BrokenAt);
        }

        Assert.That(
            _fileSystem.Files.Keys,
            Is.EqualTo(
                new[]
                {
                    BrokenPath,
                    BrokenPath.Replace(".save", "_2.save", StringComparison.Ordinal),
                    BrokenPath.Replace(".save", "_3.save", StringComparison.Ordinal),
                }
            )
        );
    }
}

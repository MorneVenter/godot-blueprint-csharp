using System;
using System.Globalization;
using System.Text.Json;
using Godot;
using Project.Assets.Scripts.Saves;

namespace Project.Assets.Scripts.Managers
{
    public partial class SaveManager : Node
    {
        private int CurrentSaveId { get; set; } = 1;
        private Save SaveData { get; set; }

        private const string ENCRYPTION_KEY = "save_encryption_key";

        private const string USER_FOLDER = "user://";
        private const string SAVE_FOLDER = "savedata";
        private const string SAVE_NAME_TEMPLATE = "save_{0}.save";

        public override void _Ready()
        {
            LoadSaveDataIntoMemory();
        }

        private void LoadSaveDataIntoMemory()
        {
            var savePath =
                $"{USER_FOLDER}{SAVE_FOLDER}/{string.Format(CultureInfo.InvariantCulture, SAVE_NAME_TEMPLATE, CurrentSaveId)}";
            if (!FileAccess.FileExists(savePath))
            {
                CreateNewSaveFile(savePath);
            }
            var saveFile = FileAccess.OpenEncryptedWithPass(
                savePath,
                FileAccess.ModeFlags.Read,
                ENCRYPTION_KEY
            );
            var saveFileContent = saveFile.GetPascalString();
            if (string.IsNullOrEmpty(saveFileContent))
            {
                throw new NotSupportedException("Save file could not be read.");
            }
            try
            {
                SaveData = JsonSerializer.Deserialize<Save>(saveFileContent);
            }
            catch (Exception)
            {
                DeleteSave(CurrentSaveId);
            }
            saveFile.Close();
        }

        private static void CreateNewSaveFile(string savePath)
        {
            var directory = DirAccess.Open(USER_FOLDER);
            directory.MakeDirRecursive(SAVE_FOLDER);
            var saveFile = FileAccess.OpenEncryptedWithPass(
                savePath,
                FileAccess.ModeFlags.Write,
                ENCRYPTION_KEY
            );
            var newSave = new Save() { LastUpdated = DateTime.Now };
            var defaultSaveContent = JsonSerializer.Serialize(newSave);
            saveFile.StorePascalString(defaultSaveContent);
            saveFile.Close();
        }

        private void WriteSaveFileToDisk()
        {
            var savePath =
                $"{USER_FOLDER}{SAVE_FOLDER}/{string.Format(CultureInfo.InvariantCulture, SAVE_NAME_TEMPLATE, CurrentSaveId)}";
            if (!FileAccess.FileExists(savePath))
            {
                CreateNewSaveFile(savePath);
            }
            var saveFile = FileAccess.OpenEncryptedWithPass(
                savePath,
                FileAccess.ModeFlags.Write,
                ENCRYPTION_KEY
            );
            var updatedSaveContent = JsonSerializer.Serialize(SaveData);
            saveFile.StorePascalString(updatedSaveContent);
            saveFile.Close();
        }

        private void UnloadSave()
        {
            SaveData = null;
            CurrentSaveId = 1;
            LoadSaveDataIntoMemory();
        }

        public void LoadSave(int slot)
        {
            CurrentSaveId = slot;
            LoadSaveDataIntoMemory();
        }

        public void DeleteSave(int slot)
        {
            var savePath =
                $"{USER_FOLDER}{SAVE_FOLDER}/{string.Format(CultureInfo.InvariantCulture, SAVE_NAME_TEMPLATE, slot)}";
            var directory = DirAccess.Open(USER_FOLDER);
            directory.Remove(savePath);
            UnloadSave();
        }

        public SaveManager Mutate(Action<Save> mutation)
        {
            mutation(SaveData);
            SaveData.LastUpdated = DateTime.Now;
            WriteSaveFileToDisk();
            return this;
        }

        public Save Get()
        {
            return SaveData;
        }

        public string GetLastWriteDate(int slot)
        {
            CurrentSaveId = slot;
            LoadSaveDataIntoMemory();
            var dateAndTime = DateTime.Now.ToString(
                "dd/MM/yyyy hh:mm tt",
                CultureInfo.InvariantCulture
            );
            UnloadSave();
            return dateAndTime;
        }
    }
}

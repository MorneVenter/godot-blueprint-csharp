using System.Text.Json;

namespace Game.Core.Managers.SaveManager.Serialization;

internal static class SaveSerializer
{
    public static string Serialize(SaveFile file) =>
        JsonSerializer.Serialize(file, SaveJsonContext.Default.SaveFile);

    public static SaveFile Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize(json, SaveJsonContext.Default.SaveFile)
                ?? throw new InvalidDataException("Save file contains null.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("Save file contains invalid data.", exception);
        }
    }
}

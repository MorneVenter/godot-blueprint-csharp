using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Game.Core.Managers.SaveManager.Serialization;

internal static class SaveSerializer
{
    public static string Serialize<TData>(
        SaveFile<TData> file,
        JsonTypeInfo<SaveFile<TData>> typeInfo
    )
        where TData : class, new() => JsonSerializer.Serialize(file, typeInfo);

    public static SaveFile<TData> Deserialize<TData>(
        string json,
        JsonTypeInfo<SaveFile<TData>> typeInfo
    )
        where TData : class, new()
    {
        try
        {
            var file =
                JsonSerializer.Deserialize(json, typeInfo)
                ?? throw new InvalidDataException("Save file contains null.");

            // The source generator ignores nullable annotations on generic properties.
            return file.Data is null
                ? throw new InvalidDataException("Save file has null data.")
                : file;
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("Save file contains invalid data.", exception);
        }
    }
}

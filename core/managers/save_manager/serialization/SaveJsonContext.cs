using System.Text.Json.Serialization;
using Game.Core.Data.Save;

namespace Game.Core.Managers.SaveManager.Serialization;

[JsonSourceGenerationOptions(RespectNullableAnnotations = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(SaveFile<SaveData>))]
internal sealed partial class SaveJsonContext : JsonSerializerContext;

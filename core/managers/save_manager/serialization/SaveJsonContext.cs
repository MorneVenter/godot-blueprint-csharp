using System.Text.Json.Serialization;

namespace Game.Core.Managers.SaveManager.Serialization;

[JsonSourceGenerationOptions(RespectNullableAnnotations = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(SaveFile))]
internal sealed partial class SaveJsonContext : JsonSerializerContext;

using System.Text.Json.Serialization;

namespace FranksDinerBlazor.Shared.Dtos
{
    public class GoogleTokenValidationResult
    {
        [JsonPropertyName("data")] public GoogleTokenValidationData Data { get; set; } = null!;
    }
}

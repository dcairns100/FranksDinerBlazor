using System.Text.Json.Serialization;

namespace FranksDinerBlazor.Shared.Dtos
{
    public class FacebookTokenValidationResult
    {
        [JsonPropertyName("data")] public FacebookTokenValidationData Data { get; set; } = null!;
    }
}

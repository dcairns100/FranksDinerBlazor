using System.Text.Json.Serialization;

namespace FranksDinerBlazor.Shared.Dtos
{
    public class GoogeAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
    }
}

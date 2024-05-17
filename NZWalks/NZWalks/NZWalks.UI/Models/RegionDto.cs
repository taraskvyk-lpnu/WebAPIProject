using System.Text.Json.Serialization;

namespace NZWalks.UI.Models
{
    public class RegionDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("regionImageUrl")]
        public string? RegionImageUrl { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace SE.Services.Services.DiscordEmbedService.Models
{
    public class EmbedAuthor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
    }
}
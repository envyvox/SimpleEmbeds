using System.Text.Json.Serialization;

namespace SE.Services.Services.DiscordEmbedService.Models
{
    public class EmbedFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
    }
}
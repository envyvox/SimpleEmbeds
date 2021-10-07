using System.Globalization;
using Discord;
using SE.Data.Models;

namespace SE.Services.Extensions
{
    public static class EmbedExtensions
    {
        public static EmbedBuilder WithDefaultColor(this EmbedBuilder embedBuilder)
        {
            return embedBuilder.WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)));
        }

        public static Embed BuildEmbed(this EmbedModel model)
        {
            var embed = new EmbedBuilder();

            if (model.Author is not null) embed.WithAuthor(model.Author.Name, model.Author.IconUrl, model.Author.Url);
            if (model.Thumbnail is not null) embed.WithThumbnailUrl(model.Thumbnail);
            if (model.Title is not null) embed.WithTitle(model.Title);
            if (model.Description is not null) embed.WithDescription(model.Description);
            if (model.Image is not null) embed.WithImageUrl(model.Image);
            if (model.Footer is not null) embed.WithFooter(model.Footer.Text, model.Footer.IconUrl);

            if (model.Fields is null) return embed.Build();

            foreach (var field in model.Fields) embed.AddField(field.Name, field.Value, field.Inline);

            return embed.Build();
        }
    }
}

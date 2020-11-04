using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using SE.Framework.Autofac;
using SE.Services.Services.DiscordEmbedService.Models;
using SE.Services.Services.DiscordGuildService;

namespace SE.Services.Services.DiscordEmbedService.Impl
{
    [InjectableService]
    public class DiscordEmbedService : IDiscordEmbedService
    {
        private readonly IDiscordGuildService _discordGuildService;

        public DiscordEmbedService(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        public async Task SendEmbed(SocketUser socketUser, EmbedBuilder embedBuilder)
        {
            await socketUser.SendMessageAsync("", false, BuildEmbed(embedBuilder));
        }

        public async Task SendEmbed(ISocketMessageChannel socketMessageChannel, EmbedBuilder embedBuilder)
        {
            await socketMessageChannel.SendMessageAsync("", false, BuildEmbed(embedBuilder));
        }

        public async Task SendWebhookEmbedModel(string webhookUrl, EmbedModel model)
        {
            var webhookClient = new DiscordWebhookClient(webhookUrl);
            await webhookClient.SendMessageAsync(model.PlainText ?? "", false, new[] {BuildEmbedFromEmbedModel(model)});
        }

        public async Task SendWebhookMessage(string webhookUrl, string message)
        {
            var webhookClient = new DiscordWebhookClient(webhookUrl);
            await webhookClient.SendMessageAsync(message);
        }

        public async Task SendEmbedModel(ISocketMessageChannel channel, EmbedModel model)
        {
            await channel.SendMessageAsync(model.PlainText ?? "", false, BuildEmbedFromEmbedModel(model));
        }

        public async Task SendEmbedModel(ulong guildId, ulong channelId, EmbedModel model)
        {
            var channel = await _discordGuildService.GetSocketTextChannel(guildId, channelId);
            await channel.SendMessageAsync(model.PlainText ?? "", false, BuildEmbedFromEmbedModel(model));
        }

        public async Task ModifyEmbedModel(ulong guildId, ulong channelId, ulong messageId, EmbedModel model)
        {
            var message = await _discordGuildService.GetIUserMessage(guildId, channelId, messageId);
            
            if (!string.IsNullOrEmpty(model.PlainText)) 
                await message.ModifyAsync(x => x.Content = model.PlainText);
            
            await message.ModifyAsync(x => x.Embed = BuildEmbedFromEmbedModel(model));
        }
        
        private static Embed BuildEmbed(EmbedBuilder embedBuilder)
        {
            return embedBuilder
                .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                .Build();
        }
        
        private static Embed BuildEmbedFromEmbedModel(EmbedModel model)
        {
            var embed = new EmbedBuilder()
                .WithColor(new Color(Convert.ToUInt32(model.Color)));

            if (model.Title != null) embed.WithTitle(model.Title);
            if (model.Description != null) embed.WithDescription(model.Description);
            if (model.Author != null) embed.WithAuthor(model.Author.Name, model.Author.IconUrl, model.Author.Url);
            if (model.Footer != null) embed.WithFooter(model.Footer.Text, model.Footer.IconUrl);
            if (model.Thumbnail != null) embed.WithThumbnailUrl(model.Thumbnail);
            if (model.Image != null) embed.WithImageUrl(model.Image);

            if (model.Fields == null) return embed.Build();
            
            foreach (var field in model.Fields)
                embed.AddField(field.Name, field.Value, field.Inline);

            return embed.Build();
        }
    }
}
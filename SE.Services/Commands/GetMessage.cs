using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SE.Data.Enums;
using SE.Services.Services.DiscordEmbedService;
using SE.Services.Services.DiscordEmbedService.Models;
using SE.Services.Services.DiscordGuildService;
using EmbedAuthor = SE.Services.Services.DiscordEmbedService.Models.EmbedAuthor;
using EmbedField = SE.Services.Services.DiscordEmbedService.Models.EmbedField;
using EmbedFooter = SE.Services.Services.DiscordEmbedService.Models.EmbedFooter;

namespace SE.Services.Commands
{
    [Summary("How to get json code from messages")]
    public class GetMessage : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IDiscordEmbedService _discordEmbedService;

        public GetMessage(IDiscordGuildService discordGuildService, IDiscordEmbedService discordEmbedService)
        {
            _discordGuildService = discordGuildService;
            _discordEmbedService = discordEmbedService;
        }

        [Command("get")]
        [Summary("Returns the json code of the message.")]
        public async Task GetMessageTask(
            [Summary("Message ID")] ulong messageId)
        {
            var message = await _discordGuildService.GetIUserMessage(Context.Guild.Id, Context.Channel.Id, messageId);

            if (message == null)
            {
                await Task.FromException(new Exception(ReplyType.MessageWithIdNull.Parse()));
            }
            else
            {
                await GetMessageTask(message);
            }
        }

        [Command("get")]
        [Summary("Returns the json code of the message.")]
        public async Task GetMessageTask(
            [Summary("Channel ID")] ulong channelId,
            [Summary("Message ID")] ulong messageId)
        {
            var message = await _discordGuildService.GetIUserMessage(Context.Guild.Id, channelId, messageId);

            if (message == null)
            {
                await Task.FromException(new Exception(ReplyType.MessageWithIdNull.Parse()));
            }
            else
            {
                await GetMessageTask(message);
            }
        }

        private async Task GetMessageTask(IMessage message)
        {
            var embed = message.Embeds.FirstOrDefault(e => e != null);

            if (embed == null)
            {
                await Task.FromException(new Exception(ReplyType.MessageEmbedNull.Parse()));
            }
            else
            {
                var model = new EmbedModel
                {
                    PlainText = message.Content.Length > 0
                        ? message.Content
                        : null,

                    Title = embed.Title,

                    Description = embed.Description ?? "",

                    Author = embed.Author != null
                        ? new EmbedAuthor {IconUrl = embed.Author?.IconUrl, Name = embed.Author?.Name}
                        : null,

                    Color = embed.Color?.RawValue.ToString(),

                    Footer = embed.Footer != null
                        ? new EmbedFooter
                        {
                            IconUrl = embed.Footer?.IconUrl,
                            Text = embed.Footer?.Text
                        }
                        : null,

                    Thumbnail = embed.Thumbnail?.Url,

                    Image = embed.Image?.Url
                };

                var fields = new EmbedField[] { }.ToList();

                fields.AddRange(embed.Fields.Select(field => new EmbedField
                {
                    Inline = field.Inline,
                    Name = field.Name,
                    Value = field.Value
                }));

                model.Fields = fields.Count > 0
                    ? fields.ToArray()
                    : null;

                var json = JsonSerializer.Serialize(model, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true,
                    IgnoreNullValues = true,
                });

                var embedReply = new EmbedBuilder()
                    .WithDescription($"```json\n{json}```");

                await _discordEmbedService.SendEmbed(Context.Channel, embedReply);
                await Task.CompletedTask;
            }
        }
    }
}

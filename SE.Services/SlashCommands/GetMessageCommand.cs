using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SE.Data.Models;
using EmbedAuthor = SE.Data.Models.EmbedAuthor;
using EmbedField = SE.Data.Models.EmbedField;
using EmbedFooter = SE.Data.Models.EmbedFooter;
using InteractionType = SE.Data.Enums.InteractionType;

namespace SE.Services.SlashCommands
{
    public record GetMessageCommand(SocketCommandBase Command, InteractionType InteractionType) : IRequest;

    public class GetMessageCommandHandler : IRequestHandler<GetMessageCommand>
    {
        public async Task<Unit> Handle(GetMessageCommand request, CancellationToken ct)
        {
            IMessage message;

            switch (request.InteractionType)
            {
                case InteractionType.SlashCommand:
                {
                    var command = (SocketSlashCommand) request.Command;
                    var channel = (SocketTextChannel) command.Data.Options
                        .Single(x => x.Name == "channel").Value;
                    var messageId = ulong.Parse((string) command.Data.Options
                        .Single(x => x.Name == "message-id").Value);

                    message = await channel.GetMessageAsync(messageId);

                    break;
                }
                case InteractionType.MessageCommand:
                {
                    var command = (SocketMessageCommand) request.Command;

                    message = command.Data.Message;

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var json = GetJson(message);

            await request.Command.FollowupAsync($"```json\n{json}```");

            return Unit.Value;
        }

        private static string GetJson(IMessage message)
        {
            var messageEmbed = message.Embeds.FirstOrDefault();

            var model = new EmbedModel
            {
                PlainText = message.Content.Length > 0 ? message.Content : null,
                Title = messageEmbed?.Title,
                Description = messageEmbed?.Description,
                Author = messageEmbed?.Author is null
                    ? null
                    : new EmbedAuthor
                    {
                        Name = messageEmbed.Author?.Name,
                        IconUrl = messageEmbed.Author?.IconUrl,
                        Url = messageEmbed.Author?.Url
                    },
                Color = messageEmbed?.Color?.RawValue.ToString(),
                Footer = messageEmbed?.Footer is null
                    ? null
                    : new EmbedFooter
                    {
                        Text = messageEmbed.Footer?.Text,
                        IconUrl = messageEmbed.Footer?.IconUrl
                    },
                Thumbnail = messageEmbed?.Thumbnail?.Url,
                Image = messageEmbed?.Image?.Url
            };

            var fields = new List<EmbedField>();

            fields.AddRange(messageEmbed?.Fields.Select(x => new EmbedField
            {
                Name = x.Name,
                Value = x.Value.ToString(),
                Inline = x.Inline
            })!);

            model.Fields = fields.Count > 0 ? fields.ToArray() : null;

            var json = JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = false,
                IgnoreNullValues = true
            });

            return json;
        }
    }
}

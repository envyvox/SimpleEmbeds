using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using MediatR;
using Newtonsoft.Json;
using SE.Data.Models;
using static SE.Services.Extensions.EmbedExtensions;

namespace SE.Services.SlashCommands
{
    public record SendMessageCommand(SocketSlashCommand Command) : IRequest;

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
    {
        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken ct)
        {
            switch (request.Command.Data.Options.First().Name)
            {
                case "message":
                    await SendMessage(request.Command);
                    break;

                case "embed":
                    switch (request.Command.Data.Options.First().Options.First().Name)
                    {
                        case "from-json":
                            await SendEmbedFromJson(request.Command);
                            break;

                        case "from-builder":
                            await SendEmbedFromBuilder(request.Command);
                            break;
                    }

                    break;

                case "webhook":
                    switch (request.Command.Data.Options.First().Options.First().Name)
                    {
                        case "message":
                            await SendWebhookMessage(request.Command);
                            break;

                        case "embed":
                            await SendWebhookEmbed(request.Command);
                            break;
                    }

                    break;
            }

            return Unit.Value;
        }

        private static async Task SendMessage(SocketSlashCommand command)
        {
            var channel = (SocketTextChannel) command.Data.Options.First().Options
                .Single(x => x.Name == "channel").Value;
            var text = (string) command.Data.Options.First().Options
                .Single(x => x.Name == "text").Value;

            try
            {
                await channel.SendMessageAsync(text);
                await command.FollowupAsync("Message sended.");
            }
            catch (Exception e)
            {
                await command.FollowupAsync(e.Message);
            }
        }

        private async Task SendEmbedFromJson(SocketSlashCommand command)
        {
            var channel = (SocketTextChannel) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "channel").Value;
            var json = (string) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "json-code").Value;

            try
            {
                var model = JsonConvert.DeserializeObject<EmbedModel>(json);
                var embed = model.BuildEmbed();

                await channel.SendMessageAsync(model.PlainText ?? "", embed: embed);
                await command.FollowupAsync("Message sended.");
            }
            catch (Exception e)
            {
                await command.FollowupAsync(e.Message);
            }
        }

        private async Task SendEmbedFromBuilder(SocketSlashCommand command)
        {
            var channel = (SocketTextChannel) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "channel").Value;
            var messageBodyOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "message-body");
            var colorOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "color");
            var authorOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "author");
            var titleOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "title");
            var descriptionOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "description");
            var thumbnailOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "thumbnail");
            var imageOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "image");
            var footerOption = command.Data.Options.First().Options.First().Options
                .SingleOrDefault(x => x.Name == "footer");

            try
            {
                var embed = new EmbedBuilder();

                if (colorOption is not null)
                    embed.WithColor(new Color(uint.Parse((string) colorOption.Value, NumberStyles.HexNumber)));
                else embed.WithDefaultColor();

                if (authorOption is not null) embed.WithAuthor((string) authorOption.Value);
                if (titleOption is not null) embed.WithTitle((string) titleOption.Value);
                if (descriptionOption is not null) embed.WithDescription((string) descriptionOption.Value);
                if (thumbnailOption is not null) embed.WithThumbnailUrl((string) thumbnailOption.Value);
                if (imageOption is not null) embed.WithImageUrl((string) imageOption.Value);
                if (footerOption is not null) embed.WithFooter((string) footerOption.Value);

                await channel.SendMessageAsync(
                    text: messageBodyOption is null ? "" : (string) messageBodyOption.Value,
                    embed: embed.Build());
                await command.FollowupAsync("Message sended.");
            }
            catch (Exception e)
            {
                await command.FollowupAsync(e.Message);
            }
        }

        private async Task SendWebhookMessage(SocketSlashCommand command)
        {
            var url = (string) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "url").Value;
            var text = (string) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "text").Value;

            try
            {
                var webhook = new DiscordWebhookClient(url);

                await webhook.SendMessageAsync(text);
                await command.FollowupAsync("Message sended.");
            }
            catch (Exception e)
            {
                await command.FollowupAsync(e.Message);
            }
        }

        private async Task SendWebhookEmbed(SocketSlashCommand command)
        {
            var url = (string) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "url").Value;
            var json = (string) command.Data.Options.First().Options.First().Options
                .Single(x => x.Name == "json-code").Value;

            try
            {
                var model = JsonConvert.DeserializeObject<EmbedModel>(json);
                var embed = model.BuildEmbed();
                var webhook = new DiscordWebhookClient(url);

                await webhook.SendMessageAsync(model.PlainText, embeds: new[] { embed });
                await command.FollowupAsync("Message sended.");
            }
            catch (Exception e)
            {
                await command.FollowupAsync(e.Message);
            }
        }
    }
}

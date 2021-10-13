using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Newtonsoft.Json;
using SE.Data.Models;
using SE.Services.Extensions;

namespace SE.Services.SlashCommands
{
    public record ModifyMessageCommand(SocketSlashCommand Command) : IRequest;

    public class ModifyMessageCommandHandler : IRequestHandler<ModifyMessageCommand>
    {
        public async Task<Unit> Handle(ModifyMessageCommand request, CancellationToken cancellationToken)
        {
            var channel = (SocketTextChannel) request.Command.Data.Options
                .Single(x => x.Name == "channel").Value;
            var messageId = (ulong) request.Command.Data.Options
                .Single(x => x.Name == "message-id").Value;
            var json = (string) request.Command.Data.Options
                .Single(x => x.Name == "json-code").Value;

            try
            {
                var message = (IUserMessage) await channel.GetMessageAsync(messageId);
                var model = JsonConvert.DeserializeObject<EmbedModel>(json);
                var embed = model.BuildEmbed();

                if (!string.IsNullOrEmpty(model.PlainText))
                    await message.ModifyAsync(x => x.Content = model.PlainText);

                await message.ModifyAsync(x => x.Embed = embed);
                await request.Command.FollowupAsync("Message modified.");
            }
            catch (Exception e)
            {
                await request.Command.FollowupAsync(e.Message);
            }

            return Unit.Value;
        }
    }
}

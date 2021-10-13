using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SE.Services.Extensions;

namespace SE.Services.SlashCommands
{
    public record HelpCommand(SocketSlashCommand Command) : IRequest;

    public class HelpCommandHandler : IRequestHandler<HelpCommand>
    {
        public async Task<Unit> Handle(HelpCommand request, CancellationToken ct)
        {
            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(".");

            await request.Command.FollowupAsync(embed: embed.Build());

            return Unit.Value;
        }
    }
}

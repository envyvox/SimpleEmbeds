using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SE.Data.Enums;
using SE.Services.Client;
using SE.Services.Extensions;
using static SE.Data.PresetStrings;

namespace SE.Services.SlashCommands
{
    public record AboutCommand(SocketSlashCommand Command) : IRequest;

    public class AboutCommandHandler : IRequestHandler<AboutCommand>
    {
        private readonly IDiscordClientService _discordClientService;

        public AboutCommandHandler(IDiscordClientService discordClientService)
        {
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(AboutCommand request, CancellationToken ct)
        {
            var client = await _discordClientService.GetSocketClient();
            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithThumbnailUrl(client.CurrentUser.GetAvatarUrl())
                .WithDescription(
                    $"{client.CurrentUser.Mention} is a simple bot to help you send and edit embed messages using the " +
                    "[eb.nadeko.bot](https://eb.nadeko.bot/) website." +
                    "\n*All rights to [eb.nadeko.bot](https://eb.nadeko.bot/) belong to the developers of nadeko.*")
                .AddField("Join support server",
                    $"{EmoteType.PineappleLove.Display()} [Click here to join]({SupportServerInviteLink}).")
                .AddField("Invite a bot to your server",
                    $"{EmoteType.PineappleLove.Display()} [Click here to open the invite link]({BotInviteLink}).")
                .AddField("Contacts",
                    $"{EmoteType.DiscordLogo.Display()} Discord: <@550493599629049858>" +
                    $"\n{EmoteType.TwitterLogo.Display()} Twitter: [@envyvox](https://twitter.com/envyvox)")
                .AddField("Support",
                    "You can support this small but useful bot by sending any amount you like. " +
                    $"I will be grateful to you for every penny {EmoteType.PineappleLove.Display()}" +
                    $"\n{EmoteType.Mastercard.Display()} `5375 4141 0460 6651` EUGENE GARBUZOV");

            await request.Command.FollowupAsync(embed: embed.Build());
            return Unit.Value;
        }
    }
}

using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SE.Data.Enums;
using SE.Services.Services.DiscordEmbedService;

namespace SE.Services.Commands
{
    public class About : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;

        public About(IDiscordEmbedService discordEmbedService)
        {
            _discordEmbedService = discordEmbedService;
        }

        [Command("about")]
        public async Task AboutTask()
        {
            var embed = new EmbedBuilder()
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
                .WithDescription(ReplyType.AboutDesc.Parse())
                .AddField(ReplyType.AboutInviteFieldName.Parse(),
                    ReplyType.InviteFieldDesc.Parse(
                        EmoteType.PineappleLove.Display()))
                .AddField(ReplyType.AboutContactsFieldName.Parse(),
                    ReplyType.AboutContactsFieldDesc.Parse(
                        EmoteType.DiscordLogo.Display(), EmoteType.TwitterLogo.Display()))
                .AddField(ReplyType.AboutDonateFieldName.Parse(),
                    ReplyType.AboutDonateFieldDesc.Parse(
                        EmoteType.PineappleLove.Display(), EmoteType.Mastercard.Display()));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}

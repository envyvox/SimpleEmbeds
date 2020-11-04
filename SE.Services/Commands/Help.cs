using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SE.Data.Enums;
using SE.Services.Services.DiscordEmbedService;

namespace SE.Services.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;

        public Help(IDiscordEmbedService discordEmbedService)
        {
            _discordEmbedService = discordEmbedService;
        }

        [Command("help")]
        public async Task HelpTask()
        {
            var embed = new EmbedBuilder()
                .AddField(ReplyType.HelpWhatPermissionsNeedFieldName.Parse(
                    EmoteType.List.Display()),
                    ReplyType.HelpWhatPermissionsNeedFieldDesc.Parse())
                .AddField(ReplyType.HelpHowToSendMessageWithoutEmbedFieldName.Parse(
                        EmoteType.List.Display()),
                    ReplyType.HelpHowToSendMessageWithoutEmbedFieldDesc.Parse())
                .AddField(ReplyType.HelpHowToSendMessageWithEmbedFieldName.Parse(
                        EmoteType.List.Display()),
                    ReplyType.HelpHowToSendMessageWithEmbedFieldDesc.Parse())
                .AddField(ReplyType.HelpHowToModifyMessageWithoutEmbedFieldName.Parse(
                        EmoteType.List.Display()),
                    ReplyType.HelpHowToModifyMessageWithoutEmbedFieldDesc.Parse())
                .AddField(ReplyType.HelpHowToModifyMessageWithEmbedFieldName.Parse(
                        EmoteType.List.Display()),
                    ReplyType.HelpHowToModifyMessageWithEmbedFieldDesc.Parse())
                .AddField(ReplyType.HelpWhereCanIFindMyMessageIdFieldName.Parse(
                        EmoteType.List.Display()),
                    ReplyType.HelpWhereCanIFindMyMessageIdFieldDesc.Parse())
                .AddField(ReplyType.HelpHowToInviteToMyServerFieldName.Parse(
                        EmoteType.List.Display()),
                    ReplyType.HelpHowToInviteToMyServerFieldDesc.Parse());

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
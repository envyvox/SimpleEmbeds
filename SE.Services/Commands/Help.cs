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
                    EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpWhatPermissionsNeedFieldDesc.Parse())
                
                .AddField(ReplyType.HelpHowToSendMessageWithoutEmbedFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpHowToSendMessageWithoutEmbedFieldDesc.Parse(
                        EmoteType.PineappleThinking.Display()))
                
                .AddField(ReplyType.HelpHowToSendMessageWithEmbedFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpHowToSendMessageWithEmbedFieldDesc.Parse(
                        EmoteType.PineappleThinking.Display()))
                
                .AddField(ReplyType.HelpHowToModifyMessageWithoutEmbedFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpHowToModifyMessageWithoutEmbedFieldDesc.Parse(
                        EmoteType.PineappleThinking.Display()))
                
                .AddField(ReplyType.HelpHowToModifyMessageWithEmbedFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpHowToModifyMessageWithEmbedFieldDesc.Parse(
                        EmoteType.PineappleThinking.Display()))
                
                .AddField(ReplyType.HelpHowToSendWebhookMessageFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpHowToSendWebhookMessageFieldDesc.Parse(
                        EmoteType.PineappleThinking.Display()))
                
                .AddField(ReplyType.HowToGetCodeFromMessageFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HowToGetCodeFromMessageFieldDesc.Parse(
                        EmoteType.PineappleThinking.Display()))
                
                .AddField(ReplyType.HelpWhereCanIFindMyMessageIdFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpWhereCanIFindMyMessageIdFieldDesc.Parse(
                        EmoteType.PineappleReading.Display()))
                
                .AddField(ReplyType.HelpHowToInviteToMyServerFieldName.Parse(
                        EmoteType.List.Display(), EmoteType.PineappleConfused.Display()),
                    ReplyType.HelpHowToInviteToMyServerFieldDesc.Parse(
                        EmoteType.PineappleLove.Display()));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
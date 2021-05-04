using System.Linq;
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
        private readonly CommandService _commandService;

        public Help(IDiscordEmbedService discordEmbedService, CommandService commandService)
        {
            _discordEmbedService = discordEmbedService;
            _commandService = commandService;
        }

        [Command("help")]
        public async Task TestTask()
        {
            var modules = _commandService.Modules.ToList();
            var embed = new EmbedBuilder()
                .AddField(ReplyType.WhereGetJsonCodeFieldName.Parse(
                        EmoteType.PineappleThinking.Display()),
                    ReplyType.WhereGetJsonCodeFieldDesc.Parse(
                        EmoteType.PineappleReading.Display()) +
                    $"\n{EmoteType.Blank.Display()}");

            foreach (var module in modules)
            {
                if (module.Summary == null) continue;

                var moduleCommandsString = string.Empty;

                foreach (var command in module.Commands)
                {
                    var parameters = command.Parameters.Aggregate(string.Empty,
                        (current, parameter) => current + $"[{parameter.Summary}] ");

                    moduleCommandsString +=
                        $"{EmoteType.List.Display()} `-{command.Name} {parameters}`\n{command.Summary ?? EmoteType.Blank.Display()}\n\n";
                }

                embed.AddField($"{module.Summary} {EmoteType.PineappleThinking.Display()}",
                    moduleCommandsString + EmoteType.Blank.Display());
            }

            embed
                .AddField(ReplyType.WhereToFindMessageIdFieldName.Parse(
                        EmoteType.PineappleThinking.Display()),
                    ReplyType.WhereToFindMessageIdFieldDesc.Parse(
                        EmoteType.PineappleReading.Display()))
                .AddField(ReplyType.HowToInviteBotFieldName.Parse(
                        EmoteType.PineappleThinking.Display()),
                    ReplyType.InviteFieldDesc.Parse(
                        EmoteType.PineappleLove.Display()));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}

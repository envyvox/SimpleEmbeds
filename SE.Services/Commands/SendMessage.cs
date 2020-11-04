using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using SE.Services.Services.DiscordEmbedService;
using SE.Services.Services.DiscordEmbedService.Models;

namespace SE.Services.Commands
{
    [RequireUserPermission(GuildPermission.ManageMessages)]
    public class SendMessage : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;

        public SendMessage(IDiscordEmbedService discordEmbedService)
        {
            _discordEmbedService = discordEmbedService;
        }

        [Command("send")]
        public async Task SendMessageTask([Remainder] string msg)
        {
            if (msg.StartsWith("{"))
                await _discordEmbedService.SendEmbedModel(Context.Channel,
                    JsonConvert.DeserializeObject<EmbedModel>(msg));
            else await ReplyAsync(msg);
            await Task.CompletedTask;
        }
    }
}
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using SE.Services.Services.DiscordEmbedService;
using SE.Services.Services.DiscordEmbedService.Models;
using SE.Services.Services.DiscordGuildService;

namespace SE.Services.Commands
{
    [RequireContext(ContextType.Guild), RequireUserPermission(GuildPermission.ManageMessages)]
    public class ModifyMessage : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;

        public ModifyMessage(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
        }

        [Command("modify")]
        public async Task ModifyMessageTask(ulong messageId, [Remainder] string msg)
        {
            if (msg.StartsWith("{"))
                await _discordEmbedService.ModifyEmbedModel(Context.Guild.Id, Context.Channel.Id, messageId,
                    JsonConvert.DeserializeObject<EmbedModel>(msg));
            else await 
                (await _discordGuildService.GetIUserMessage(Context.Guild.Id, Context.Channel.Id, messageId))
                    .ModifyAsync(x => x.Content = msg);
            
            await Task.CompletedTask;
        }

        [Command("modify")]
        public async Task ModifyMessageTask(ulong channelId, ulong messageId, [Remainder] string msg)
        {
            if (msg.StartsWith("{"))
                await _discordEmbedService.ModifyEmbedModel(Context.Guild.Id, channelId, messageId,
                    JsonConvert.DeserializeObject<EmbedModel>(msg));
            else await 
                (await _discordGuildService.GetIUserMessage(Context.Guild.Id, channelId, messageId))
                .ModifyAsync(x => x.Content = msg);
            
            await Task.CompletedTask;
        }
    }
}
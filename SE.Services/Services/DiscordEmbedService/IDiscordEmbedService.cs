using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SE.Services.Services.DiscordEmbedService.Models;

namespace SE.Services.Services.DiscordEmbedService
{
    public interface IDiscordEmbedService
    {
        Task SendEmbed(SocketUser socketUser, EmbedBuilder embedBuilder);
        Task SendEmbed(ISocketMessageChannel socketMessageChannel, EmbedBuilder embedBuilder);
        
        Task SendEmbedModel(ISocketMessageChannel socketTextChannel, EmbedModel model);
        Task ModifyEmbedModel(ulong guildId, ulong channelId, ulong messageId, EmbedModel model);
    }
}
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SE.Services.Services.DiscordGuildService
{
    public interface IDiscordGuildService
    {
        Task<SocketTextChannel> GetSocketTextChannel(ulong guildId, ulong channelId);
        Task<IUserMessage> GetIUserMessage(ulong guildId, ulong channelId, ulong messageId);
    }
}

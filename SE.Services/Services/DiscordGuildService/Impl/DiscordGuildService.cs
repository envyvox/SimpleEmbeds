using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SE.Framework.Autofac;
using SE.Services.Services.DiscordClientService;

namespace SE.Services.Services.DiscordGuildService.Impl
{
    [InjectableService]
    public class DiscordGuildService : IDiscordGuildService
    {
        private readonly IDiscordClientService _discordClientService;

        public DiscordGuildService(IDiscordClientService discordClientService)
        {
            _discordClientService = discordClientService;
        }

        public async Task<SocketGuild> GetSocketGuild(ulong guildId)
        {
            var socketClient = await _discordClientService.GetSocketClient();
            return socketClient.GetGuild(guildId);
        }

        public async Task<SocketTextChannel> GetSocketTextChannel(ulong guildId, ulong channelId)
        {
            var guild = await GetSocketGuild(guildId);
            return guild.GetTextChannel(channelId);
        }

        public async Task<IUserMessage> GetIUserMessage(ulong guildId, ulong channelId, ulong messageId)
        {
            var channel = await GetSocketTextChannel(guildId, channelId);
            return (IUserMessage) await channel.GetMessageAsync(messageId);
        }
    }
}
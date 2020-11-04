using System.Threading.Tasks;
using Discord.WebSocket;

namespace SE.Services.Services.DiscordClientService
{
    public interface IDiscordClientService
    {
        Task Start();
        Task<DiscordSocketClient> GetSocketClient();
    }
}
using System.Threading.Tasks;
using Discord.WebSocket;

namespace SE.Services.Client
{
    public interface IDiscordClientService
    {
        Task Start();
        Task<DiscordSocketClient> GetSocketClient();
    }
}
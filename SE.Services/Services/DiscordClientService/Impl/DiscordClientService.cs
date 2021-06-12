using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using SE.Framework.Autofac;
using SE.Services.Services.DiscordClientService.Options;

namespace SE.Services.Services.DiscordClientService.Impl
{
    [InjectableService(IsSingletone = true)]
    public class DiscordClientService : IDiscordClientService
    {
        private readonly IOptions<DiscordClientOptions> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandService _commands;

        private DiscordSocketClient _socketClient;

        public DiscordClientService(IOptions<DiscordClientOptions> options, IServiceProvider serviceProvider,
            CommandService commands)
        {
            _options = options;
            _serviceProvider = serviceProvider;
            _commands = commands;
            _socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMembers |
                    GatewayIntents.GuildMessageReactions |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.GuildVoiceStates |
                    GatewayIntents.DirectMessages
            });
        }

        public async Task Start()
        {
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.Token);
            await _socketClient.StartAsync();

            _commands.CommandExecuted += CommandExecutedAsync;
            _socketClient.Log += Log;
            _socketClient.Ready += SocketClientOnReady;
            _socketClient.MessageReceived += SocketClientOnMessageReceived;
            _socketClient.JoinedGuild += SocketClientOnJoinedGuild;
            _socketClient.LeftGuild += SocketClientOnLeftGuild;
        }

        public async Task<DiscordSocketClient> GetSocketClient()
        {
            return await Task.FromResult(_socketClient);
        }

        private static async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context,
            IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason) && result.Error is not CommandError.UnknownCommand)
                await context.Channel.SendMessageAsync($"{context.User.Mention}, {result.ErrorReason}");
        }

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task SocketClientOnMessageReceived(SocketMessage messageParam)
        {
            if (messageParam is not SocketUserMessage message) return;

            var argPos = 0;

            if (!(message.HasCharPrefix('-', ref argPos) ||
                  message.HasMentionPrefix(_socketClient.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_socketClient, message);

            var result = await _commands.ExecuteAsync(
                context,
                argPos,
                _serviceProvider);

            if (result.IsSuccess && context.Channel is SocketGuildChannel)
            {
                await Task.Delay(1000);
                await message.DeleteAsync();
            }
        }

        private async Task SocketClientOnReady() => await UpdateClientStatus();

        private async Task SocketClientOnJoinedGuild(SocketGuild socketGuild) => await UpdateClientStatus();

        private async Task SocketClientOnLeftGuild(SocketGuild socketGuild) => await UpdateClientStatus();

        private async Task UpdateClientStatus()
        {
            await _socketClient.SetGameAsync(
                name: $"..help ..about | {_socketClient.Guilds.Count} servers",
                streamUrl: null,
                type: ActivityType.Watching);
        }
    }
}

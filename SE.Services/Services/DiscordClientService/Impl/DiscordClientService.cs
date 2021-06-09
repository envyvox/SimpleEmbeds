using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using SE.Data.Enums;
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
        }

        private static async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context,
            IResult result)
        {
            if (result?.Error != CommandError.UnknownCommand && !string.IsNullOrEmpty(result?.ErrorReason))
            {
                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                    .AddField(ReplyType.SomethingGoneWrong.Parse(),
                        result.Error switch
                        {
                            CommandError.ParseFailed => ReplyType.CommandErrorParseFailed.Parse(),
                            CommandError.BadArgCount => ReplyType.CommandErrorBadArgCount.Parse(),
                            CommandError.ObjectNotFound => ReplyType.CommandErrorObjectNotFound.Parse(),
                            CommandError.MultipleMatches => ReplyType.CommandErrorMultipleMatches.Parse(),
                            _ => result.ErrorReason
                        });

                await context.User.SendMessageAsync("", false, embed.Build());
            }
        }

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task SocketClientOnMessageReceived(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;

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

            if (result.IsSuccess && context.Channel.GetType() != typeof(SocketDMChannel))
            {
                await Task.Delay(1000);
                await message.DeleteAsync();
            }
        }

        private async Task SocketClientOnReady() =>
            await _socketClient.SetGameAsync($"-help -about | {_socketClient.Guilds.Count} servers", null, ActivityType.Watching);

        public async Task<DiscordSocketClient> GetSocketClient() =>
            await Task.FromResult(_socketClient);
    }
}

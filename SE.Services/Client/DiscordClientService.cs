using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Services.SlashCommands;
using InteractionType = SE.Data.Enums.InteractionType;

namespace SE.Services.Client
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly IOptions<DiscordClientOptions> _options;
        private readonly IMediator _mediator;
        private readonly DiscordSocketClient _socketClient;

        public DiscordClientService(
            IOptions<DiscordClientOptions> options,
            IMediator mediator)
        {
            _options = options;
            _mediator = mediator;
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
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.Token);
            await _socketClient.StartAsync();

            _socketClient.Ready += SocketClientOnReady;
            _socketClient.JoinedGuild += SocketClientOnJoinedGuild;
            _socketClient.LeftGuild += SocketClientOnLeftGuild;
            _socketClient.InteractionCreated += SocketClientOnInteractionCreated;
        }

        public async Task<DiscordSocketClient> GetSocketClient()
        {
            return await Task.FromResult(_socketClient);
        }

        private async Task<Unit> SocketClientOnInteractionCreated(SocketInteraction interaction)
        {
            await interaction.DeferAsync(true, new RequestOptions
            {
                RetryMode = RetryMode.Retry502,
                Timeout = 10000
            });

            try
            {
                switch (interaction)
                {
                    case SocketSlashCommand command:
                        return command.Data.Name switch
                        {
                            "about" => await _mediator.Send(new AboutCommand(command)),
                            "help" => await _mediator.Send(new HelpCommand(command)),
                            "get" => await _mediator.Send(new GetMessageCommand(command, InteractionType.SlashCommand)),
                            "send" => await _mediator.Send(new SendMessageCommand(command)),
                            _ => throw new ArgumentOutOfRangeException()
                        };

                    case SocketMessageCommand command:

                        return command.Data.Name switch
                        {
                            "Get json-code" => await _mediator.Send(new GetMessageCommand(command,
                                InteractionType.MessageCommand)),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                }
            }
            catch (Exception e)
            {
                await interaction.FollowupAsync(e.Message);
            }

            return Unit.Value;
        }

        private async Task SocketClientOnReady()
        {
            ApplicationCommandProperties[] commands =
            {
                new SlashCommandBuilder()
                    .WithName("help")
                    .WithDescription("Information on how to use commands")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("about")
                    .WithDescription("Bot information")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("send")
                    .WithDescription("Send a message")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("message")
                        .WithDescription("Send a simple text message")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Channel)
                            .WithRequired(true)
                            .WithName("channel")
                            .WithDescription("The channel to send the message to"))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("text")
                            .WithDescription("Message text")))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommandGroup)
                        .WithName("embed")
                        .WithDescription("Send a embeded message")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("from-json")
                            .WithDescription("Send a embeded message using generated json code")
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.Channel)
                                .WithRequired(true)
                                .WithName("channel")
                                .WithDescription("The channel to send the message to"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(true)
                                .WithName("json-code")
                                .WithDescription("Generated json code from eb.nadeko.bot")))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("from-builder")
                            .WithDescription(
                                "Send a embeded message using embed builder (you can't build embed fields here)")
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.Channel)
                                .WithRequired(true)
                                .WithName("channel")
                                .WithDescription("The channel to send the message to"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("message-body")
                                .WithDescription("Message body"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("color")
                                .WithDescription("HEX color"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("author")
                                .WithDescription("Embed author text"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("title")
                                .WithDescription("Embed title text"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("description")
                                .WithDescription("Embed description"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("thumbnail")
                                .WithDescription("Embed thumbnail URL"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("image")
                                .WithDescription("Embed image URL"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(false)
                                .WithName("footer")
                                .WithDescription("Embed footer text"))))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommandGroup)
                        .WithName("webhook")
                        .WithDescription("Send a message via webhook")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("message")
                            .WithDescription("Send a simple text message via webhook")
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(true)
                                .WithName("url")
                                .WithDescription("Webhook url"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(true)
                                .WithName("text")
                                .WithDescription("Message text")))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("embed")
                            .WithDescription("Send a embeded message via webhook")
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(true)
                                .WithName("url")
                                .WithDescription("Webhook url"))
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithType(ApplicationCommandOptionType.String)
                                .WithRequired(true)
                                .WithName("json-code")
                                .WithDescription("Generated json code from eb.nadeko.bot"))))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("get")
                    .WithDescription("Get json code from message")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Channel)
                        .WithRequired(true)
                        .WithName("channel")
                        .WithDescription("The channel in which the message is located"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("message-id")
                        .WithDescription("Message ID"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("modify")
                    .WithDescription("Modify a mesage")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Channel)
                        .WithRequired(true)
                        .WithName("channel")
                        .WithDescription("The channel in which the message is located"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("message-id")
                        .WithDescription("Message ID"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("json-code")
                        .WithDescription("Generated json code from eb.nadeko.bot"))
                    .Build(),

                new MessageCommandBuilder()
                    .WithName("Get json-code")
                    .Build()
            };

            try
            {
                await _socketClient.Rest.BulkOverwriteGuildCommands(commands, 798535744096043019);
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Error, Formatting.Indented);
                Console.WriteLine(json);
            }


            await UpdateClientStatus();
        }

        private async Task SocketClientOnJoinedGuild(SocketGuild socketGuild)
        {
            await UpdateClientStatus();
        }

        private async Task SocketClientOnLeftGuild(SocketGuild socketGuild)
        {
            await UpdateClientStatus();
        }

        private async Task UpdateClientStatus()
        {
            await _socketClient.SetGameAsync(
                name: $"/help /about | {_socketClient.Guilds.Count} servers",
                streamUrl: null,
                type: ActivityType.Watching);
        }
    }
}

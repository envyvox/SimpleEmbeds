// using System.Threading.Tasks;
// using Discord;
// using Discord.Commands;
// using Discord.WebSocket;
// using Newtonsoft.Json;
// using SE.Data.Models;
// using SE.Services.Services.Embed;
// using SE.Services.Services.Guild;
//
// namespace SE.Services.Commands
// {
//     [Summary("How to send messages")]
//     [RequireContext(ContextType.Guild), RequireUserPermission(GuildPermission.ManageMessages)]
//     public class SendMessage : ModuleBase<SocketCommandContext>
//     {
//         private readonly IDiscordEmbedService _discordEmbedService;
//         private readonly IDiscordGuildService _discordGuildService;
//
//         public SendMessage(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService)
//         {
//             _discordEmbedService = discordEmbedService;
//             _discordGuildService = discordGuildService;
//         }
//
//         [Command("send")]
//         [Summary("Sends a message to the current channel.")]
//         public async Task SendMessageTask(
//             [Summary("Message or json code")] [Remainder] string msg)
//         {
//             if (msg.StartsWith("{"))
//             {
//                 await _discordEmbedService.SendEmbedModel(Context.Channel,
//                     JsonConvert.DeserializeObject<EmbedModel>(msg));
//             }
//             else
//             {
//                 await ReplyAsync(msg);
//             }
//
//             await Task.CompletedTask;
//         }
//
//         [Command("send")]
//         [Summary("Sends a message to the specified channel.")]
//         public async Task SendMessageTask(
//             [Summary("Channel ID")] ulong channelId,
//             [Summary("Message or json code")] [Remainder] string msg)
//         {
//             if (msg.StartsWith("{"))
//             {
//                 await _discordEmbedService.SendEmbedModel(Context.Guild.Id, channelId,
//                     JsonConvert.DeserializeObject<EmbedModel>(msg));
//             }
//             else
//             {
//                 var channel = await _discordGuildService.GetSocketTextChannel(Context.Guild.Id, channelId);
//                 await channel.SendMessageAsync(msg);
//             }
//
//             await Task.CompletedTask;
//         }
//
//         [Command("send")]
//         [Summary("Sends a message to the specified channel.")]
//         public async Task SendMessageTask(
//             [Summary("#Channel")] ITextChannel channel,
//             [Summary("Message or json code")] [Remainder] string msg)
//         {
//             if (msg.StartsWith("{"))
//             {
//                 await _discordEmbedService.SendEmbedModel((ISocketMessageChannel) channel,
//                     JsonConvert.DeserializeObject<EmbedModel>(msg));
//             }
//             else
//             {
//                 await channel.SendMessageAsync(msg);
//             }
//
//             await Task.CompletedTask;
//         }
//
//         [Command("send-wh")]
//         [Summary("Sends a message using the specified webhook.")]
//         public async Task SendWebhookMessageTask(
//             [Summary("Webhook url")] string webhookUrl,
//             [Summary("Message or json code")] [Remainder] string msg)
//         {
//             if (msg.StartsWith("{"))
//             {
//                 await _discordEmbedService.SendWebhookEmbedModel(webhookUrl,
//                     JsonConvert.DeserializeObject<EmbedModel>(msg));
//             }
//             else
//             {
//                 await _discordEmbedService.SendWebhookMessage(webhookUrl, msg);
//             }
//
//             await Task.CompletedTask;
//         }
//     }
// }

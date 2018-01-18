using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

namespace Sorter.Modules.Admin
{
    [Group("admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Group("clean"), RequireUserPermission(GuildPermission.ManageMessages)]
        public class CleanModule : ModuleBase<SocketCommandContext>
        {
            // ~admin clean 15
            [Command]
            public async Task Default(uint amount = 10) => Messages(amount);

            // ~admin clean messages 15
            [Command("messages")]
            public async Task Messages(uint amount = 20)
            {
                for (int i = 0; i < amount + 1; i++)
                {
                    Task<RestUserMessage> res = Context.Channel.SendMessageAsync($"Message: {i}");
                    while (!res.IsCompleted);
                }

                DeleteMessages(amount, Context.Channel);
            }

            public static async void DeleteMessages(uint amount, ISocketMessageChannel channel )
            {
                //TODO: Fix crashes if the bot does not have permission to delete
                var messages = await channel.GetMessagesAsync((int)amount + 1).Flatten();   //+1 so the clean command is also deleted

                await channel.DeleteMessagesAsync(messages);
                const int delay = 3000;
                var m = await channel.SendMessageAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
                await Task.Delay(delay);
                await m.DeleteAsync();
            }

            public static async void DeleteCommand(ISocketMessageChannel contextChannel)
            {
                //TODO: Fix crashes if the bot does not have permission to delete
                var messages = await contextChannel.GetMessagesAsync(1).Flatten();
                await contextChannel.DeleteMessagesAsync(messages);

                const int delay = 3000;
                var m = await contextChannel.SendMessageAsync($"Command deleted. _This message will be deleted in {delay / 1000} seconds._");
                await Task.Delay(delay);
                await m.DeleteAsync();
            }
        }
    }
}

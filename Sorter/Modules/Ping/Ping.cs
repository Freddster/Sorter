using System.Threading.Tasks;
using Discord.Commands;

namespace Sorter.Modules.Ping
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong");
        }

        [Command("beep")]
        public async Task beepAsync()
        {
            await ReplyAsync("Boop");
        }
    }
}
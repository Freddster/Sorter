using System.Threading.Tasks;
using Discord.Commands;

namespace Sorter.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Hello world");
        }
    }
}
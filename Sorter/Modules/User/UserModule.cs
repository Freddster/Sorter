using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Sorter.Modules.User
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        public static async Task VoiceChannelUpdated(SocketUser user, SocketVoiceState userLeftVoiceState, SocketVoiceState userJoinedVoiceState)
        {
            if (userJoinedVoiceState.VoiceChannel != null)
            {
                UserJoinedModule.UserJoined(userJoinedVoiceState, user);
            }

            if (userLeftVoiceState.VoiceChannel != null)
            {
                UserLeftModule.UserLeft(userLeftVoiceState, user);
            }
        }
    }
}
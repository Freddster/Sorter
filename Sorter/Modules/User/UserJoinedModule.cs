using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Sorter.Modules;

namespace Sorter.Modules.User
{
    public class UserJoinedModule : ModuleBase<SocketCommandContext>
    {
        public static void UserJoined(SocketVoiceState userJoinedVoiceState, SocketUser user)
        {
            var voiceChannel = userJoinedVoiceState.VoiceChannel;
            if (voiceChannel.Users != null)
            {
                var channelName = voiceChannel.Name;

                var onlyCollection = voiceChannel.Guild.Roles;
                foreach (var socketRole in onlyCollection)
                {
                    if (socketRole.Name == channelName)
                    {
                        var guildUser = user as SocketGuildUser;
                        guildUser.AddRoleAsync(socketRole);
                    }
                }
            }

            Logging.LogUserJoinedChannel(voiceChannel.Guild, user, voiceChannel.Name);
        }
    }
}
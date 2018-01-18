using System;
using Discord.Commands;
using Discord.WebSocket;
using Sorter.Common;
using Sorter.Modules.Admin;

namespace Sorter.Modules.User
{
    public class UserLeftModule : ModuleBase<SocketCommandContext>
    {
        public static void UserLeft(SocketVoiceState userLeftVoiceState, SocketUser user)
        {
            var voiceChannel = userLeftVoiceState.VoiceChannel;
            if (voiceChannel.Users != null)
            {
                var channelName = voiceChannel.Name;

                var onlyCollection = voiceChannel.Guild.Roles;
                foreach (var socketRole in onlyCollection)
                {
                    if (socketRole.Name == channelName)
                    {
                        var guildUser = user as SocketGuildUser;
                        guildUser.RemoveRoleAsync(socketRole);
                    }
                }

                var usersCount = voiceChannel.Users.Count;
                if (usersCount == 0)
                {
                    String textChannelName = channelName.ToLower();
                    if (channelName.Contains(" "))
                    {
                        textChannelName = textChannelName.Replace(' ', '-').ToLower();

                        if (textChannelName.Contains("æ") || textChannelName.Contains("ø") || textChannelName.Contains("å"))
                            textChannelName = SpecialCharacterReplacer.ReplaceDanishChar(textChannelName);
                    }

                    //ulong textChannelId;
                    foreach (var socketGuildChannel in voiceChannel.Guild.Channels)
                    {
                        if (socketGuildChannel.Name != null && socketGuildChannel.Name.Equals(textChannelName))
                        {
                            ISocketMessageChannel test = voiceChannel.Guild.GetTextChannel(socketGuildChannel.Id);
                            //AdminModule.CleanModule.DeleteMessages(3, test);
                        }
                    }
                }
            }
        }
    }
}
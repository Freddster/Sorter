using System.Text.RegularExpressions;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Sorter.DataAccessLogic;
using Sorter.Models;
using Sorter.Modules.Admin;

namespace Sorter.Modules
{
    [Group("logging"), RequireUserPermission(ChannelPermission.ManageChannel)]
    public class Logging : ModuleBase<SocketCommandContext>
    {
        [Command("setChannel")]
        public Task SetLoggingChannel()
        {
            var channelId = Context.Message.Channel.Id;
            var guildId = Context.Guild.Id;
            SaveLoggingChannelId(channelId, guildId);
            return Task.CompletedTask;
        }

        [Command("remove")]
        public Task RemoveLoggingChannel()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var model = unitOfWork.LoggingChannelRepository.GetLoggingChannelId(Convert.ToString(Context.Guild.Id)).Result;
            if (model != null)
            {
                unitOfWork.LoggingChannelRepository.Remove(model);
                unitOfWork.Save();

                AdminModule.CleanModule.DeleteCommand(Context.Channel);
            }

            return Task.CompletedTask;
        }

        private static async void SaveLoggingChannelId(ulong id, ulong guildId)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var model = await unitOfWork.LoggingChannelRepository.GetLoggingChannelId(Convert.ToString(guildId));

            var loggingChannelModel = new LoggingChannelModel()
            {
                Guild = Convert.ToString(guildId),
                Value = Convert.ToString(id)
            };

            if (model == null)
            {
                unitOfWork.LoggingChannelRepository.Add(loggingChannelModel);
            }
            else
            {
                unitOfWork.LoggingChannelRepository.EditConfiguration(model, loggingChannelModel);
            }

            await unitOfWork.Save();
        }


        public static async Task LogUserJoinedChannel(SocketGuild voiceChannelGuild, SocketUser user, string channelName)
        {
            var model = GetModel(voiceChannelGuild);
            if (model != null)
            {
                var socketGuildUser = user as SocketGuildUser;
                var nickname = GetUsername(ref socketGuildUser);
                var socketTextChannel = GetSocketTextChannel(ref voiceChannelGuild, ref model);
                if (socketTextChannel != null)
                {
                    await socketTextChannel.SendMessageAsync($"**{nickname}** _joined_ channel **{channelName}**");

                }
            }
        }

        public static async Task LogUserLeftChannel(SocketGuild voiceChannelGuild, SocketUser user, string channelName)
        {
            var model = GetModel(voiceChannelGuild);
            if (model != null)
            {
                var socketGuildUser = user as SocketGuildUser;
                var nickname = GetUsername(ref socketGuildUser);
                var socketTextChannel = GetSocketTextChannel(ref voiceChannelGuild, ref model);
                if (socketTextChannel != null)
                {
                    await socketTextChannel.SendMessageAsync($"**{nickname}** _left_ channel **{channelName}**");

                }
            }
        }

        private static SocketTextChannel GetSocketTextChannel(ref SocketGuild voiceChannelGuild, ref LoggingChannelModel model)
        {
            return voiceChannelGuild.GetTextChannel(Convert.ToUInt64(model.Value));
        }

        private static string GetUsername(ref SocketGuildUser socketGuildUser)
        {
            return socketGuildUser.Username + "#" + socketGuildUser.Discriminator;
        }

        private static LoggingChannelModel GetModel(SocketGuild voiceChannelGuild)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            ulong id = voiceChannelGuild.Id;
            return unitOfWork.LoggingChannelRepository.GetLoggingChannelId(Convert.ToString(id)).Result;
        }
    }
}
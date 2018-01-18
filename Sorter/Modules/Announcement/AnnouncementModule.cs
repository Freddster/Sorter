using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Sorter.DataAccessLogic;
using Sorter.DataAccessLogic.Repositories;
using Sorter.Models;

namespace Sorter.Modules.Announcement
{
    [Group("announcement"), RequireUserPermission(ChannelPermission.ManageMessages)]
    public class AnnouncementModule : ModuleBase<SocketCommandContext>
    {
        public static async void SaveAnnouncementChannelId(ulong id, ulong guildId)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var model = await unitOfWork.AnnouncementChannelRepository.GetAnnouncementChannelId(Convert.ToString(guildId));

            var announcementChannelModel = new AnnouncementChannelModel()
            {
                Guild = Convert.ToString(guildId),
                Value = Convert.ToString(id)
            };

            if (model == null)
            {
                unitOfWork.AnnouncementChannelRepository.Add(announcementChannelModel);
            }
            else
            {
                unitOfWork.AnnouncementChannelRepository.EditConfiguration(model, announcementChannelModel);
            }

            await unitOfWork.Save();
        }

        [Command("setChannel")]
        public Task SetAnnouncementId()
        {
            var channelId = Context.Message.Channel.Id;
            var guildId = Context.Guild.Id;
            SaveAnnouncementChannelId(channelId, guildId);
            return Task.CompletedTask;
        }

        public static async Task UserJoinedAnnoucement(SocketGuildUser user)
        {
            var announcementModule = new AnnouncementModule();
            var announcementChannelId = announcementModule.GetAnnouncementChannelId(user);

            var guild = user.Guild;
            var announcementChannel = guild.GetTextChannel(announcementChannelId);
            await announcementChannel.SendMessageAsync($"{user.Mention}, velkommen til den bedste danske Guild Wars 2 guild **{guild.Name}**");
        }

        public static async Task UserLeftAnnoucement(SocketGuildUser user)
        {
            var announcementModule = new AnnouncementModule();
            var announcementChannelId = announcementModule.GetAnnouncementChannelId(user);

            var guild = user.Guild;
            var announcementChannel = guild.GetTextChannel(announcementChannelId);
            await announcementChannel.SendMessageAsync($"{user.Username} har desværre forladt os");
        }

        private ulong GetAnnouncementChannelId(SocketGuildUser user)
        {
            var guild = user.Guild;
            var guildId = Convert.ToString(guild.Id);
            using (var db = new SorterContext())
            {
                var singleOrDefault = db
                    .Set<AnnouncementChannelModel>()
                    .SingleOrDefault(g => g.Guild == guildId);

                if (singleOrDefault == null)
                {
                    user.Guild.Owner.SendMessageAsync("Announcement Channel not set");

                    user.SendMessageAsync("AnnouncementChannel is probably not set. Try the set channel command, and try again");
                    throw new InvalidDataException("Announcement channel was not found. AnnouncementChannel is probably not set");
                }

                var keyValue = singleOrDefault;
                return Convert.ToUInt64(keyValue.Value);
            }
        }
    }
}
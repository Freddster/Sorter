using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sorter.Models;

namespace Sorter.DataAccessLogic
{
    public interface IAnnouncementChannelRepository : IRepository<AnnouncementChannelModel>
    {
        Task<AnnouncementChannelModel> GetAnnouncementChannelId(string guildId);
        void EditConfiguration(AnnouncementChannelModel oldModel, AnnouncementChannelModel newModel);
    }
}
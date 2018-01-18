using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Sorter.Models;

namespace Sorter.DataAccessLogic.Repositories
{
    public class AnnouncementChannelRepository : GenericRepository<AnnouncementChannelModel>, IAnnouncementChannelRepository
    {
        private DbSet<AnnouncementChannelModel> _dbSet;

        public AnnouncementChannelRepository(SorterContext context) : base(context)
        {
            _dbSet = context.AnnouncementChannelModels;
        }

        public async Task<AnnouncementChannelModel> GetAnnouncementChannelId(string guildId)
        {
            return _dbSet.SingleOrDefault(g => g.Guild == guildId);
        }

        public void EditConfiguration(AnnouncementChannelModel modelToChange ,AnnouncementChannelModel newModel)
        {
            modelToChange.Value = newModel.Value;
            modelToChange.Guild = newModel.Guild;
            MarkAsDirty(modelToChange);
        }
    }
}
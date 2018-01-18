using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Sorter.Models;

namespace Sorter.DataAccessLogic.Repositories
{
    public class LoggingChannelRepository : GenericRepository<LoggingChannelModel>, ILoggingChannelRepository
    {
        private DbSet<LoggingChannelModel> _dbSet;

        public LoggingChannelRepository(SorterContext context) : base(context)
        {
            _dbSet = context.LoggingChannelModels;
        }

        public async Task<LoggingChannelModel> GetLoggingChannelId(string guildId)
        {
            return _dbSet.SingleOrDefault(g => g.Guild == guildId);
        }

        public void EditConfiguration(LoggingChannelModel modelToChange, LoggingChannelModel newModel)
        {
            modelToChange.Value = newModel.Value;
            modelToChange.Guild = newModel.Guild;
            MarkAsDirty(modelToChange);
        }
    }
}
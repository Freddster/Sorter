using System.Threading.Tasks;
using Sorter.Models;

namespace Sorter.DataAccessLogic
{
    public interface ILoggingChannelRepository : IRepository<LoggingChannelModel>
    {
        Task<LoggingChannelModel> GetLoggingChannelId(string guildId);
        void EditConfiguration(LoggingChannelModel oldModel, LoggingChannelModel newModel);
    }
}
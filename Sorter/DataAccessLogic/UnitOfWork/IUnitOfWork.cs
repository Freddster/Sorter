using System.Threading.Tasks;

namespace Sorter.DataAccessLogic
{
    public interface IUnitOfWork
    {
        IAnnouncementChannelRepository AnnouncementChannelRepository { get; }
        ILoggingChannelRepository LoggingChannelRepository { get; }
        IKeyValueRepository KeyValueRepository { get; }

        Task Save();
    }
}
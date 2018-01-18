using System.Threading.Tasks;
using Sorter.DataAccessLogic.Repositories;
using Sorter.Models;

namespace Sorter.DataAccessLogic
{
    public class UnitOfWork : IUnitOfWork
    {
        private SorterContext _context;

        public UnitOfWork()
        {
            _context = new SorterContext();
            InitializeRepositories();
        }

        public UnitOfWork(SorterContext context)
        {
            _context = context;
            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            AnnouncementChannelRepository = new AnnouncementChannelRepository(_context);    
            LoggingChannelRepository = new LoggingChannelRepository(_context);
            KeyValueRepository = new KeyValueRepository(_context);
        }

        public IAnnouncementChannelRepository AnnouncementChannelRepository { get; set; }
        public ILoggingChannelRepository LoggingChannelRepository { get; set; }
        public IKeyValueRepository KeyValueRepository { get; set; }

        public async Task Save()
        {
            _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
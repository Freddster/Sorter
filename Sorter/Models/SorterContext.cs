using System.Data.Entity;

namespace Sorter.Models
{
    public class SorterContext : DbContext
    {
        public SorterContext() : base("name=botConfiguration")
        {
        }

        public DbSet<AnnouncementChannelModel> AnnouncementChannelModels { get; set; }
        public DbSet<LoggingChannelModel> LoggingChannelModels { get; set; }
        public DbSet<KeyValueModel> KeyValueModels { get; set; }
    }
}
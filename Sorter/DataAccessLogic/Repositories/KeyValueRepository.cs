using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Sorter.Models;

namespace Sorter.DataAccessLogic.Repositories
{
    public class KeyValueRepository : GenericRepository<KeyValueModel>, IKeyValueRepository
    {
        private DbSet<KeyValueModel> _dbSet;

        public KeyValueRepository(SorterContext context) : base(context)
        {
            _dbSet = context.KeyValueModels;
        }

        public KeyValueModel GetToken(string key)
        {
            return _dbSet.SingleOrDefault(k => k.Key == key);
        }

        public List<KeyValueModel> GetTokenList(string key)
        {
            return _dbSet.ToList();
        }
    }
}
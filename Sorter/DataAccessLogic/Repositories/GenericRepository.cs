using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Sorter.DataAccessLogic.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        protected DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public async void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public async void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        protected void MarkAsDirty(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
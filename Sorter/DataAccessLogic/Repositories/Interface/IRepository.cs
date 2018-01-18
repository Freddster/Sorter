using System.Collections.Generic;

namespace Sorter.DataAccessLogic
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
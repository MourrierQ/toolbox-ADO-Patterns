using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Repository
{
    public interface IDataService<TKey, TEntity>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TKey ID);
        TEntity Add(TEntity model);
        void Remove(TKey ID);
        TEntity Update(TEntity model);

    }
}

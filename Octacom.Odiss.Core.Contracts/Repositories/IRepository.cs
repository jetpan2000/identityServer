using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
    {
        (Guid key, byte[] timestamp) Insert(TEntity item);
        (Guid key, byte[] timestamp) Update(TEntity item, Guid ID);
        void Delete(Guid ID);
    }

    public interface IReadOnlyRepository<TEntity>
    {
        TEntity Get(Guid ID);
        IEnumerable<TEntity> GetAll();
    }
}

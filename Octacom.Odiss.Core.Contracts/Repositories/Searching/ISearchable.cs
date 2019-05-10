using System;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public interface ISearchable<TEntity, TSearchParameters>
        where TSearchParameters : SearchParameters
    {
        SearchResult<TEntity> Search(TSearchParameters parameters);
    }
}

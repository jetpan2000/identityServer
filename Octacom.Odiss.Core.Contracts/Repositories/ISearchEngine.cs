using Octacom.Odiss.Core.Contracts.Repositories.Searching;
using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public interface ISearchEngine<TEntity>
    {
        SearchResult<TEntity> Search(IDictionary<string, object> searchParameters = null, int page = 1, int? pageSize = null, IDictionary<string, SortOrder> sortings = null);
        SearchResult<TEntity> Search(SearchOptions options);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Octacom.Odiss.Core.DataLayer
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public static class SearchHelper
    {
        public static IDictionary<string, Search.EF.SortOrder> MapSortings(IDictionary<string, Contracts.Repositories.Searching.SortOrder> sortings = null)
        {
            IDictionary<string, Octacom.Search.EF.SortOrder> sortingsMapped = null;

            if (sortings != null)
            {
                sortingsMapped = sortings.Select(x => new KeyValuePair<string, Octacom.Search.EF.SortOrder>(x.Key, MapSortOrder(x.Value))).ToDictionary(x => x.Key, x => x.Value);
            }

            return sortingsMapped;
        }

        public static Contracts.Repositories.Searching.SearchResult<TEntity> MapSearchResults<TEntity>(Search.EF.SearchResult<TEntity> result)
        {
            return new Contracts.Repositories.Searching.SearchResult<TEntity>
            {
                FilteredCount = result.FilteredCount,
                Records = result.Records,
                TotalCount = result.TotalCount,
                NumberOfPages = result.NumberOfPages
            };
        }

        private static Octacom.Search.EF.SortOrder MapSortOrder(Contracts.Repositories.Searching.SortOrder sortOrder)
        {
            if (sortOrder == Contracts.Repositories.Searching.SortOrder.Ascending)
            {
                return Octacom.Search.EF.SortOrder.Ascending;
            }
            else if (sortOrder == Contracts.Repositories.Searching.SortOrder.Descending)
            {
                return Octacom.Search.EF.SortOrder.Descending;
            }
            else
            {
                return Octacom.Search.EF.SortOrder.None;
            }
        }
    }
}

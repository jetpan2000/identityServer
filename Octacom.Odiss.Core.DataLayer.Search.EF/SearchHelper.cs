using System.Collections.Generic;
using System.Linq;

namespace Octacom.Odiss.Core.DataLayer.Search.EF
{
    internal static class SearchHelper
    {
        internal static IDictionary<string, Octacom.Search.EF.SortOrder> MapSortings(IDictionary<string, Octacom.Odiss.Core.Contracts.DataLayer.Search.SortOrder> sortings = null)
        {
            IDictionary<string, Octacom.Search.EF.SortOrder> sortingsMapped = null;

            if (sortings != null)
            {
                sortingsMapped = sortings.Select(x => new KeyValuePair<string, Octacom.Search.EF.SortOrder>(x.Key, MapSortOrder(x.Value))).ToDictionary(x => x.Key, x => x.Value);
            }

            return sortingsMapped;
        }

        internal static Octacom.Odiss.Core.Contracts.DataLayer.Search.SearchResult<TEntity> MapSearchResults<TEntity>(Octacom.Search.EF.SearchResult<TEntity> result)
        {
            return new Octacom.Odiss.Core.Contracts.DataLayer.Search.SearchResult<TEntity>
            {
                FilteredCount = result.FilteredCount,
                Records = result.Records,
                TotalCount = result.TotalCount,
                NumberOfPages = result.NumberOfPages
            };
        }

        private static Octacom.Search.EF.SortOrder MapSortOrder(Octacom.Odiss.Core.Contracts.DataLayer.Search.SortOrder sortOrder)
        {
            if (sortOrder == Octacom.Odiss.Core.Contracts.DataLayer.Search.SortOrder.Ascending)
            {
                return Octacom.Search.EF.SortOrder.Ascending;
            }
            else if (sortOrder == Octacom.Odiss.Core.Contracts.DataLayer.Search.SortOrder.Descending)
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
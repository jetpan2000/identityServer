using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Search
{
    public class SearchResult<TEntity>
    {
        /// <summary>
        /// Records retrieved by the search
        /// </summary>
        public IEnumerable<TEntity> Records { get; set; }

        /// <summary>
        /// Total records that were searched on before applying search filtering
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total records that were resulted from the search (before filtering results on paging)
        /// </summary>
        public int FilteredCount { get; set; }

        /// <summary>
        /// Number of pages for the filtered results
        /// </summary>
        public int NumberOfPages { get; set; }
    }

    public class SearchResult : SearchResult<dynamic>
    {

    }
}

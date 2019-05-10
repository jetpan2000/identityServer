using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public class SearchResult<TEntity>
    {
        public IEnumerable<TEntity> Records { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public int NumberOfPages { get; set; }
    }
}

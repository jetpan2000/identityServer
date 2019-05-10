using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public class SearchOptions
    {
        public IDictionary<string, object> SearchParameters { get; set; }
        public int Page { get; set; } = 1;
        public int? PageSize { get; set; }
        public IDictionary<string, SortOrder> Sortings { get; set; }
    }
}

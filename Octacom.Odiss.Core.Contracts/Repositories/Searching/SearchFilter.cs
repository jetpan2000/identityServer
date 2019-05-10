using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public class SearchFilter<TValue>
    {
        public TValue Value { get; set; }
        public FilterType FilterType { get; set; } = FilterType.Equals;
        public SortOrder SortOrder { get; set; } = SortOrder.None;


        public SearchFilter()
        {

        }

        public SearchFilter(FilterType filterType)
        {
            FilterType = filterType;
        }

        public SearchFilter(FilterType filterType, SortOrder sortOrder)
        {
            FilterType = filterType;
            SortOrder = sortOrder;
        }

        public SearchFilter(TValue defaultValue, FilterType filterType, SortOrder sortOrder)
        {
            Value = defaultValue;
            FilterType = filterType;
            SortOrder = sortOrder;
        }

        public SearchFilter(TValue defaultValue, SortOrder sortOrder)
        {
            Value = defaultValue;
            SortOrder = sortOrder;
        }

        public SearchFilter(TValue defaultValue, FilterType filterType)
        {
            Value = defaultValue;
            FilterType = filterType;
        }

        public SearchFilter(TValue defaultValue)
        {
            Value = defaultValue;
        }
    }
}

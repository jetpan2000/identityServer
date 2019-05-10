using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Settings.Entities
{
    public interface ISearchableField : IField
    {
        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        [Obsolete("Avoid using for new implementations")]
        string FilterCommand { get; set; }

        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        [Obsolete("Avoid using for new implementations")]
        string FilterData { get; set; }

        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        [Obsolete("Avoid using for new implementations")]
        int FilterType { get; set; }

        SearchFieldConfiguration SearchConfiguration { get; set; }
    }

    public class SearchField : Field, ISearchableField
    {
        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        [Obsolete("Avoid using for new implementations")]
        public string FilterCommand { get; set; }

        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        [Obsolete("Avoid using for new implementations")]
        public string FilterData { get; set; }

        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        [Obsolete("Avoid using for new implementations")]
        public int FilterType { get; set; }

        public SearchFieldConfiguration SearchConfiguration { get; set; }
    }

    public class SearchFieldConfiguration
    {
        public string EntityName { get; set; }
        public string DisplayFormat { get; set; }
        public bool AllowSearchByStringInput { get; set; }
        public int? MaxRecords { get; set; }
        public string SearchFields { get; set; }
        public IDictionary<string, string> SortOrder { get; set; }
        public SearchFieldAutocompleteConfiguration AutocompleteConfiguration { get; set; }
    }

    public class SearchFieldAutocompleteConfiguration
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}

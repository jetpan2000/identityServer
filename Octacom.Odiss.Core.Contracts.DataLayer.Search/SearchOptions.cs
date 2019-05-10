using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Search
{
    public class SearchOptions
    {
        /// <summary>
        /// Search Parameters specific in a dictionary where the key is the property name and object is the value to search for.
        /// It is also allowed to add to the property name a dot and the comparison operator (e.g. Name.StartsWith) and to use
        /// multiple properties separated by AND or OR (e.g. FirstName OR LastName). It is also allowed to search on ICollection
        /// properties for either Any or All and in parenthesis the name of the nested field with dots for comparison operator
        /// (e.g. Items.Any(Code.Equals)). See tests inside Octacom.Search.EF for how this can be used.
        /// 
        /// UPDATE: There is support for the key to be the Identifier of the Field in the corresponding Application. In that case
        /// the search parameter is resolved to be based on how this field is configured to provide searches on.
        /// </summary>
        public IDictionary<string, object> SearchParameters { get; set; }

        /// <summary>
        /// Page to retrieve records for (defaults to 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Size of page (defaults to the value from site configuration)
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Sortings that are applied to the search query. Dictionary where the key is the property name and SortOrder specified what
        /// sorting rules to apply. The dictionary order is used for the order of how the sortings are applied in the query.
        /// If not set then the implemented DefaultSortings of the Search Engine is used.
        /// </summary>
        public IDictionary<string, SortOrder> Sortings { get; set; }

        /// <summary>
        /// Additional arguments supplied in the search request. Will only ever be impacted in custom SearchEngine implementations where
        /// GetQueriable is overriden and specific situations arise from these arguments.
        /// </summary>
        public IDictionary<string, object> AdditionalArguments { get; set; }
    }

    /// <summary>
    /// Search Options against a specific EntityName (used for searching in GlobalSearchEngine)
    /// </summary>
    public class GlobalSearchOptions : SearchOptions
    {
        /// <summary>
        /// Name of Entity to perform non-generic search on. It's used to determine which concrete Search Engine to use.
        /// ISearchEngine has mappings registered for the entity names and types.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Identifier for the Application which performs the Search. It's only required if SearchParameters contains any keys
        /// which are FieldIdentifiers and the entity for the search is different from the application calling. In order to
        /// resolve a query from a field we need to perform lookup for the calling application to get the field.
        /// </summary>
        public string CallingApplicationIdentifier { get; set; }
    }
}
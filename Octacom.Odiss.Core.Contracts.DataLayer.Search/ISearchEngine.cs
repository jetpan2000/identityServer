namespace Octacom.Odiss.Core.Contracts.DataLayer.Search
{
    public interface ISearchEngine<TEntity>
    {
        SearchResult<TEntity> Search(SearchOptions options);
    }

    /// <summary>
    /// Non-generic ISearchEngine
    /// </summary>
    public interface ISearchEngine
    {
        /// <summary>
        /// Non-generic search
        /// </summary>
        /// <param name="options">Must include EntityName which has been mapped to a Type</param>
        /// <returns>Search Result with dynamic result (as they can only be determined at run-time)</returns>
        SearchResult<dynamic> Search(SearchOptions options);
    }
}
using System;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Search
{
    public class GlobalSearchEngine
    {
        private readonly IServiceProvider serviceProvider;
        private readonly SearchEngineRegistry registry;

        public GlobalSearchEngine(IServiceProvider serviceProvider, SearchEngineRegistry registry)
        {
            this.serviceProvider = serviceProvider;
            this.registry = registry;
        }

        /// <summary>
        /// Non-generic search
        /// </summary>
        /// <param name="options">Must include EntityName which has been mapped to a Type</param>
        /// <returns>Search Result with dynamic result (as they can only be determined at run-time)</returns>
        public SearchResult Search(GlobalSearchOptions options)
        {
            var searchEngine = GetSearchEngineForEntity(options.EntityName);

            var result = searchEngine.Search(options);

            return new SearchResult
            {
                FilteredCount = result.FilteredCount,
                NumberOfPages = result.NumberOfPages,
                TotalCount = result.TotalCount,
                Records = result.Records
            };
        }

        public ISearchEngine GetSearchEngineForEntity(string entityName)
        {
            return new EntitySearchEngine(entityName, registry, serviceProvider);
        }
    }
}
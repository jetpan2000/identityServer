using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Search
{
    /// <summary>
    /// Performs searches on entities in a dynamic way. Requires instantiation by entityName where it's been registered in the GlobalSearchEngine.
    /// </summary>
    internal class EntitySearchEngine : ISearchEngine
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Type entityType;

        public EntitySearchEngine(string entityName, SearchEngineRegistry registry, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.entityType = registry.GetEntityType(entityName);
        }

        public SearchResult<dynamic> Search(SearchOptions options)
        {
            var searchEngine = GetSearchEngine();
            var searchEngineType = searchEngine.GetType();
            var searchMethod = searchEngineType.GetMethod("Search");
            var resultType = searchMethod.ReturnType;
            var result = searchMethod.Invoke(searchEngine, new object[] { options });

            return new SearchResult
            {
                FilteredCount = (int)resultType.GetProperty("FilteredCount").GetValue(result),
                NumberOfPages = (int)resultType.GetProperty("NumberOfPages").GetValue(result),
                TotalCount = (int)resultType.GetProperty("TotalCount").GetValue(result),
                Records = (IEnumerable<dynamic>)resultType.GetProperty("Records").GetValue(result)
            };
        }

        private object GetSearchEngine()
        {
            var adapterType = typeof(ISearchEngine<>).MakeGenericType(this.entityType);

            return serviceProvider.GetService(adapterType);
        }
    }
}

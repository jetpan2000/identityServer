using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using Octacom.Odiss.Core.Contracts.DataLayer.Search;
using Octacom.Odiss.Core.Contracts.Settings;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Octacom.Odiss.Core.DataLayer.Search.EF
{
    public class OdissSearchEngine<TEntity> : ISearchEngine<TEntity>
        where TEntity : class
    {
        private readonly InternalSearchEngine<TEntity> searchEF;
        private readonly SearchOptionMiddleware searchOptionMiddleware;
        private readonly IDbContextFactory<DbContext> dbContextFactory;

        public OdissSearchEngine(IServiceProvider serviceProvider)
        {
            var defaultSortings = DefaultSortings != null ? SearchHelper.MapSortings(DefaultSortings) : null;

            var settingsService = (ISettingsService)serviceProvider.GetService(typeof(ISettingsService));
            var applicationService = (IApplicationService)serviceProvider.GetService(typeof(IApplicationService));
            var searchEngineRegistry = (SearchEngineRegistry)serviceProvider.GetService(typeof(SearchEngineRegistry));

            this.dbContextFactory = (IDbContextFactory<DbContext>)serviceProvider.GetService(typeof(IDbContextFactory<DbContext>));
            this.searchEF = new InternalSearchEngine<TEntity>(settingsService, dbContextFactory, GetQueriable, defaultSortings);
            this.searchOptionMiddleware = new SearchOptionMiddleware(applicationService, searchEngineRegistry, typeof(TEntity));
        }

        public virtual Contracts.DataLayer.Search.SearchResult<TEntity> Search(SearchOptions options)
        {
            return SearchInternal<TEntity>(options, null);
        }

        internal Contracts.DataLayer.Search.SearchResult<TEntity> SearchInternal<TKey>(SearchOptions options, Expression<Func<TEntity, TKey>> keySelector)
        {
            if (options == null)
            {
                options = new SearchOptions();
            }

            this.searchEF.StoreSearchOptions(options);
            searchOptionMiddleware.Execute(options);

            // Make SearchOptions available in GetQueriable when it's overriden for a custom SearchEngine

            using (var ctx = dbContextFactory.Create())
            {
                var sortings = SearchHelper.MapSortings(options.Sortings);

                var result = this.searchEF.Search(ctx, options.SearchParameters, options.Page, options.PageSize, sortings, keySelector);

                return SearchHelper.MapSearchResults(result);
            }
        }

        /// <summary>
        /// Middleware for building base IQueryable to search on (practical when by default certain related data on searchable entity is restricted per user)
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="options">In terms of the actual Search Engine this does nothing but it's a argument here in order to allow customizing the returned IQueryable if there are any custom SearchOptions passed</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetQueriable(DbContext dbContext, SearchOptions options)
        {
            return dbContext.Set<TEntity>();
        }

        protected virtual IDictionary<string, SortOrder> DefaultSortings => null;
    }

    public class OdissSearchEngine<TEntity, TKey> : OdissSearchEngine<TEntity>, ISearchEngine<TEntity>
        where TEntity : class
    {
        public OdissSearchEngine(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Contracts.DataLayer.Search.SearchResult<TEntity> Search(SearchOptions options)
        {
            return SearchInternal(options, GetKeySelector());
        }

        protected virtual Expression<Func<TEntity, TKey>> GetKeySelector()
        {
            // Consider using EF to provide a default value based on the key configured on the Entity

            return null;
        }
    }
}
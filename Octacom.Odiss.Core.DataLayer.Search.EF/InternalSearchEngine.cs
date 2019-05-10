using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Octacom.Odiss.Core.Contracts.Settings;
using Octacom.Search.EF;

namespace Octacom.Odiss.Core.DataLayer.Search.EF
{
    /// <summary>
    /// The purpose of this class is to provide a implementation of SearchEngine from Octacom.Search.EF which has the basic
    /// set up for Odiss entities while hiding the implementation details for Octacom.Search.EF from Odiss sites.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class InternalSearchEngine<TEntity> : SearchEngine<TEntity>
        where TEntity : class
    {
        private readonly ISettingsService settingsService;
        private readonly IDbContextFactory<DbContext> dbContextFactory;
        private readonly Func<DbContext, Octacom.Odiss.Core.Contracts.DataLayer.Search.SearchOptions, IQueryable<TEntity>> getQueriable;
        private readonly IDictionary<string, SortOrder> defaultSortings;

        private Octacom.Odiss.Core.Contracts.DataLayer.Search.SearchOptions searchOptions;

        public InternalSearchEngine(ISettingsService settingsService, IDbContextFactory<DbContext> dbContextFactory, Func<DbContext, Octacom.Odiss.Core.Contracts.DataLayer.Search.SearchOptions, IQueryable<TEntity>> getQueriable, IDictionary<string, SortOrder> defaultSortings)
        {
            this.settingsService = settingsService;
            this.dbContextFactory = dbContextFactory;
            this.getQueriable = getQueriable;
            this.defaultSortings = defaultSortings;
        }

        internal SearchResult<TEntity> SearchByStoredOptions<TKey>(DbContext dbContext, Expression<Func<TEntity, TKey>> keySelector)
        {
            if (this.searchOptions == null)
            {
                throw new Exception("No internal SearchOption stored. Need to call StoreSearchOptions prior to using this overload.");
            }

            var sortings = SearchHelper.MapSortings(this.searchOptions.Sortings);

            return this.Search(dbContext, this.searchOptions.SearchParameters, this.searchOptions.Page, this.searchOptions.PageSize, sortings, keySelector);
        }

        /// <summary>
        /// Stores a copy of the original searchOptions so they can be passed again in GetQueriable
        /// The purpose is to be able to give access to them when overriding the method OdissSearchEngine.GetQueriable for custom queries
        /// 
        /// This design may be a little confusing but as this is an internal class which will never be exposed outside of this library
        /// then we are able to limit the usage of it. Only OdissSearchEngine is intended to use this feature.
        /// </summary>
        internal void StoreSearchOptions(Octacom.Odiss.Core.Contracts.DataLayer.Search.SearchOptions searchOptions)
        {
            this.searchOptions = new Contracts.DataLayer.Search.SearchOptions
            {
                AdditionalArguments = searchOptions.AdditionalArguments?.Clone(),
                Page = searchOptions.Page,
                PageSize = searchOptions.PageSize,
                SearchParameters = searchOptions.SearchParameters?.Clone(),
                Sortings = searchOptions.Sortings?.Clone()
            };
        }

        protected override IDictionary<string, SortOrder> DefaultSortings
        {
            get
            {
                if (this.defaultSortings != null)
                {
                    return this.defaultSortings;
                }

                using (var ctx = dbContextFactory.Create())
                {
                    var entityTypes = ctx.GetEntityTypes();
                    var entityType = entityTypes.Single(x => x.FullName == typeof(TEntity).FullName);
                    var keyNames = entityType.KeyProperties.Select(x => x.Name);

                    var kvp = keyNames.Select(key => new KeyValuePair<string, SortOrder>(key, Octacom.Search.EF.SortOrder.Ascending));

                    return kvp.ToDictionary(x => x.Key, x => x.Value);
                }
            }
        }

        protected override IQueryable<TEntity> GetQueriable(DbContext dbContext)
        {
            return this.getQueriable(dbContext, this.searchOptions);
        }

        protected override int? MaxRecords => settingsService.Get().MaxRecords;
        protected override int MaxPageSize => settingsService.Get().MaxPerPage;
    }
}
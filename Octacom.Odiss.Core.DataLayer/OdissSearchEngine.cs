using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Search.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Octacom.Odiss.Core.DataLayer
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public class OdissSearchEngine<TEntity> : SearchEngine<TEntity>, ISearchEngine<TEntity>
        where TEntity : class
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IConfigService configService;

        public OdissSearchEngine(IDbContextFactory dbContextFactory, IConfigService configService) : base()
        {
            this.dbContextFactory = dbContextFactory;
            this.configService = configService;
        }

        // We're doing it this way to avoid creating a dependency on IServiceEngine for Octacom.Search.EF
        // A contract should not be dependent on a specific implementation, otherwise its purpose is defeated
        public virtual Contracts.Repositories.Searching.SearchResult<TEntity> Search(IDictionary<string, object> searchParameters = null, int page = 1, int? pageSize = null, IDictionary<string, Contracts.Repositories.Searching.SortOrder> sortings = null)
        {
            using (var ctx = dbContextFactory.Get())
            {
                var results = Search<TEntity>(ctx, searchParameters, page, pageSize, SearchHelper.MapSortings(sortings), null);

                return SearchHelper.MapSearchResults(results);
            }
        }

        public Contracts.Repositories.Searching.SearchResult<TEntity> Search(Contracts.Repositories.Searching.SearchOptions options)
        {
            if (options != null)
            {
                return Search(options.SearchParameters, options.Page, options.PageSize, options.Sortings);
            }
            else
            {
                return Search();
            }
        }

        protected override IQueryable<TEntity> GetQueriable(DbContext dbContext)
        {
            return dbContext.Set<TEntity>();
        }

        protected override IDictionary<string, Search.EF.SortOrder> DefaultSortings
        {
            get
            {
                using (var ctx = dbContextFactory.Get())
                {
                    var entityTypes = ctx.GetEntityTypes();
                    var entityType = entityTypes.Single(x => x.FullName == typeof(TEntity).FullName);
                    var keyNames = entityType.KeyProperties.Select(x => x.Name);

                    var kvp = keyNames.Select(key => new KeyValuePair<string, Search.EF.SortOrder>(key, Octacom.Search.EF.SortOrder.Ascending));

                    return kvp.ToDictionary(x => x.Key, x => x.Value);
                }
            }
        }

        protected override int? MaxRecords => configService.GetApplicationSettings().MaxRecords;
        protected override int MaxPageSize => configService.GetApplicationSettings().MaxPerPage;
    }
}

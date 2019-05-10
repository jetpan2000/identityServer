using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.Entities.Documents;
using Octacom.Search.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Octacom.Odiss.Core.DataLayer.Documents
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public abstract class DocumentSearchEngine<TDocument, TDocumentResult> : OdissSearchEngine<TDocumentResult>, ISearchEngine<TDocumentResult>
        where TDocument : Document, IDocumentRecord
        where TDocumentResult : class, IDocumentRecord
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IConfigService configService;

        public DocumentSearchEngine(IDbContextFactory dbContextFactory, IConfigService configService) : base(dbContextFactory, configService)
        {
            this.dbContextFactory = dbContextFactory;
            this.configService = configService;
        }

        // We're doing it this way to avoid creating a dependency on IServiceEngine for Octacom.Search.EF
        // A contract should not be dependent on a specific implementation, otherwise its purpose is defeated
        public override Contracts.Repositories.Searching.SearchResult<TDocumentResult> Search(IDictionary<string, object> searchParameters = null, int page = 1, int? pageSize = null, IDictionary<string, Contracts.Repositories.Searching.SortOrder> sortings = null)
        {
            using (var ctx = dbContextFactory.Get())
            {
                var results = Search(ctx, searchParameters, page, pageSize, SearchHelper.MapSortings(sortings), x => x.GUID);

                return SearchHelper.MapSearchResults(results);
            }
        }

        protected IQueryable<TDocument> GetDocumentQueriable(DbContext dbContext)
        {
            // TODO - Add UsersDocument WHERE clause here
            return dbContext.Set<TDocument>();
        }

        protected override IDictionary<string, SortOrder> DefaultSortings => new Dictionary<string, SortOrder>
        {
            { "CaptureDate", SortOrder.Descending }
        };
    }
}

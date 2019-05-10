using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document;
using Octacom.Odiss.Core.Contracts.DataLayer.Search;
using Octacom.Odiss.Core.DataLayer.Search.EF;

namespace Octacom.Odiss.Core.DataLayer.Repository.Document.EF
{
    public class DocumentSearchEngine<TDocument, TDocumentResult> : OdissSearchEngine<TDocumentResult, Guid>, ISearchEngine<TDocumentResult>
        where TDocument : Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document.Document, IDocumentRecord
        where TDocumentResult : class, IDocumentRecord
    {
        public DocumentSearchEngine(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Entities.Documents;

namespace Octacom.Odiss.Core.DataLayer.Documents
{
    public class DocumentRepository<TDocument> : IDocumentRepository<TDocument>
        where TDocument : Document
    {
        private readonly IDbContextFactory dbContextFactory;

        public DocumentRepository(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public virtual TDocument Get(Guid documentId)
        {
            using (var ctx = dbContextFactory.Get())
            {
                return ctx.Set<TDocument>().FirstOrDefault(x => x.GUID == documentId);
            }
        }

        public virtual void Create(TDocument document)
        {
            using (var ctx = dbContextFactory.Get())
            {
                ctx.Set<TDocument>().Add(document);
                ctx.SaveChanges();
            }
        }

        public virtual void Update(TDocument document, Guid documentId)
        {
            using (var ctx = dbContextFactory.Get())
            {
                var existing = ctx.Set<TDocument>().FirstOrDefault(x => x.GUID == documentId);
                ctx.Entry(existing).State = EntityState.Detached;
                ctx.Entry(document).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public virtual void Delete(Guid documentId)
        {
            using (var ctx = dbContextFactory.Get())
            {
                var document = ctx.Set<TDocument>().FirstOrDefault(x => x.GUID == documentId);
                ctx.Set<TDocument>().Remove(document);
                ctx.SaveChanges();
            }
        }

        //public IEnumerable<AuditLog> GetAuditLogs(Guid documentId)
        //{
        //    using (var ctx = dbContextFactory.GetIdentityTracking())
        //    {
        //        return ctx.GetLogs<TDocument>(documentId).ToList();
        //    }
        //}
    }
}

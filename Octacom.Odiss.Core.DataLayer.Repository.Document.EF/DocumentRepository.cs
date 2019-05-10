using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document;

namespace Octacom.Odiss.Core.DataLayer.Repository.Document.EF
{
    public class DocumentRepository<TDocument> : IDocumentRepository<TDocument>
        where TDocument : Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document.Document
    {
        private readonly IDbContextFactory<DbContext> dbContextFactory;

        public DocumentRepository(IDbContextFactory<DbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public virtual TDocument Get(Guid documentId)
        {
            using (var ctx = dbContextFactory.Create())
            {
                return ctx.Set<TDocument>().FirstOrDefault(x => x.GUID == documentId);
            }
        }

        public virtual void Create(TDocument document)
        {
            using (var ctx = dbContextFactory.Create())
            {
                ctx.Set<TDocument>().Add(document);
                ctx.SaveChanges();
            }
        }

        public virtual void Update(TDocument document, Guid documentId)
        {
            using (var ctx = dbContextFactory.Create())
            {
                var existing = ctx.Set<TDocument>().FirstOrDefault(x => x.GUID == documentId);
                ctx.Entry(existing).State = EntityState.Detached;
                ctx.Entry(document).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public virtual void Delete(Guid documentId)
        {
            using (var ctx = dbContextFactory.Create())
            {
                var document = ctx.Set<TDocument>().FirstOrDefault(x => x.GUID == documentId);
                ctx.Set<TDocument>().Remove(document);
                ctx.SaveChanges();
            }
        }
    }
}

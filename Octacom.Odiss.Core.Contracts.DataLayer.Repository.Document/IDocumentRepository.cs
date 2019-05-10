using System;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document
{
    public interface IDocumentRepository<TDocument>
        where TDocument : Document
    {
        TDocument Get(Guid documentId);
        void Create(TDocument document);
        void Update(TDocument document, Guid documentId);
        void Delete(Guid documentId);
    }
}

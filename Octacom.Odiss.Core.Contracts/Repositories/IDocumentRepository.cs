using System;
using Octacom.Odiss.Core.Entities.Documents;

namespace Octacom.Odiss.Core.Contracts.Repositories
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
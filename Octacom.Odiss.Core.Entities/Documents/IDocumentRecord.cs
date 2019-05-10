using System;

namespace Octacom.Odiss.Core.Entities.Documents
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document.IDocumentRecord instead")]
    public interface IDocumentRecord
    {
        Guid GUID { get; set; }
    }
}

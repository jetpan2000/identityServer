using Octacom.Odiss.Core.Entities.Storage;
using System;

namespace Octacom.Odiss.Core.Entities.Documents
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document.Document instead")]
    public class Document : IDocumentRecord
    {
        public Guid GUID { get; set; }
        public int GroupCode { get; set; }
        public string DirectoryId { get; set; }
        public Directory Directory { get; set; }
        public string FileName { get; set; }
        public DateTime CaptureDate { get; set; }
        public bool? UserSubmitted { get; set; }
        public string SubmittedBy { get; set; }
    }
}

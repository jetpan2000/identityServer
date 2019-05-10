using System;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document
{
    public class Document : IDocumentRecord
    {
        public Guid GUID { get; set; }
        public int GroupCode { get; set; }
        public string DirectoryId { get; set; }
        public string FileName { get; set; }
        public DateTime CaptureDate { get; set; }
        public bool? UserSubmitted { get; set; }
        public string SubmittedBy { get; set; }
    }
}

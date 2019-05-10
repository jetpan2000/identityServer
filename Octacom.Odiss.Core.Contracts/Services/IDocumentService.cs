using Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document;

namespace Octacom.Odiss.Core.Contracts.Services
{
    public interface IDocumentService<TDocument>
        where TDocument : Document
    {
        /// <summary>
        /// Saves the document file on the disk and creates a entry in tblGroup for it.
        /// </summary>
        void SubmitDocument(TDocument document, byte[] bytes, string originalFilename);

        /// <summary>
        /// Only saves the document on the disk. Useful when we want to store the document in a different database table than the default (e.g. supporting documents).
        /// </summary>
        void SaveDocumentToDisk(TDocument document, byte[] bytes, string originalFilename);
    }
}

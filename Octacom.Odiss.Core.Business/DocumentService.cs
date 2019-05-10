using System;
using System.IO;
using Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document;
using Octacom.Odiss.Core.Contracts.Services;

namespace Octacom.Odiss.Core.Business
{
    public class DocumentService<TDocument> : IDocumentService<TDocument>
        where TDocument : Document
    {
        private readonly IDocumentRepository<TDocument> repository;
        private readonly IPrincipalService principalService;
        private readonly IStorageService storageService;

        public DocumentService(IDocumentRepository<TDocument> repository, IPrincipalService principalService, IStorageService storageService)
        {
            this.repository = repository;
            this.principalService = principalService;
            this.storageService = storageService;
        }

        public virtual void SaveDocumentToDisk(TDocument document, byte[] bytes, string originalFilename)
        {
            var diskFile = storageService.GenerateUniqueUploadFilePath(Path.GetExtension(originalFilename));

            var saveResult = storageService.UploadFile(bytes, diskFile.FileName);
            var principal = principalService.GetCurrentPrincipal();

            document.CaptureDate = DateTime.Now;
            document.DirectoryId = saveResult.OdissDirectory.Id;
            document.FileName = diskFile.FileName;
            document.SubmittedBy = principal.Identity.Name;
            document.UserSubmitted = true;
        }

        public virtual void SubmitDocument(TDocument document, byte[] bytes, string originalFilename)
        {
            SaveDocumentToDisk(document, bytes, originalFilename);

            repository.Create(document);
        }
    }
}

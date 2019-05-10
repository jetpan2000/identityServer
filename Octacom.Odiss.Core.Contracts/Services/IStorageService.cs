using System;
using Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document;
using Octacom.Odiss.Core.Entities.Storage;

namespace Octacom.Odiss.Core.Contracts.Services
{
    public interface IStorageService
    {
        Directory RetrieveUploadDirectory();
        FileResult UploadFile(byte[] bytes, string filename);
        FileResult SaveFile(byte[] bytes, string filename, Directory directory);
        FileResult GenerateUniqueFilePath(Directory directory, Location location, string extension);
        FileResult GenerateUniqueUploadFilePath(string extension);
        FileResult GetDocumentFile(Document document);
    }
}

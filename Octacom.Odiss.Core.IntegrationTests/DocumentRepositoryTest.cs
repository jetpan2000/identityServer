using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.DataLayer;
using Octacom.Odiss.Core.DataLayer.Documents;
using Octacom.Odiss.Core.Entities.Documents;
using Octacom.Odiss.Core.IntegrationTests.TestImplementations;

namespace Octacom.Odiss.Core.IntegrationTests
{
    [TestClass]
    public class DocumentRepositoryTest
    {
        private IDocumentRepository<Document> baseDocumentRepository;
        private IDocumentRepository<ABCGroupDocument> cardinalHealthDocumentRepository;

        [TestInitialize]
        public void Initialize()
        {
            baseDocumentRepository = new DocumentRepository<Document>(new BaseDocumentDbContextFactory());
            cardinalHealthDocumentRepository = new DocumentRepository<ABCGroupDocument>(new ABCGroupDocumentDbContextFactory());
        }

        [TestMethod]
        public void DocumentRepositoryBaseType_Get_ReturnsDocument()
        {
            var documentId = new Guid("280D8530-E3F3-E811-822E-D89EF34A256D");
            var document = baseDocumentRepository.Get(documentId);

            Assert.AreEqual("00000001.tif", document.FileName);
        }

        [TestMethod]
        public void DocumentRepositoryConcreteType_Get_ReturnsDocumentWithCustomProperties()
        {
            var documentId = new Guid("280D8530-E3F3-E811-822E-D89EF34A256D");
            var document = cardinalHealthDocumentRepository.Get(documentId);

            Assert.AreEqual("1", document.PONumber);
        }

        [TestMethod]
        public void DocumentRepository_Create_CreatesDocument()
        {
            const string fileName = "CREATED_DOCUMENT.pdf";

            var document = new Document
            {
                FileName = fileName,
                DirectoryId = "A0001",
                CaptureDate = DateTime.Now
            };

            baseDocumentRepository.Create(document);

            var retrievedDocument = baseDocumentRepository.Get(document.GUID);

            Assert.AreEqual(document.FileName, retrievedDocument.FileName);
        }

        [TestMethod]
        public void DocumentRepository_Update_UpdatesDocument()
        {
            const string fileName = "CREATED_DOCUMENT.pdf";
            const string updateBy = "UPDATE TEST";

            var document = new Document
            {
                FileName = fileName,
                DirectoryId = "A0001",
                CaptureDate = DateTime.Now
            };

            baseDocumentRepository.Create(document);

            document.UserSubmitted = true;
            document.SubmittedBy = updateBy;
            baseDocumentRepository.Update(document, document.GUID);

            var retrievedDocument = baseDocumentRepository.Get(document.GUID);

            Assert.AreEqual(updateBy, retrievedDocument.SubmittedBy);
        }

        [TestMethod]
        public void DocumentRepository_Delete_DeletesDocument()
        {
            const string fileName = "CREATED_DOCUMENT.pdf";

            var document = new Document
            {
                FileName = fileName,
                DirectoryId = "A0001",
                CaptureDate = DateTime.Now
            };

            baseDocumentRepository.Create(document);

            baseDocumentRepository.Delete(document.GUID);

            var retrievedDocument = baseDocumentRepository.Get(document.GUID);

            Assert.IsNull(retrievedDocument);
        }

        //[TestMethod]
        //public void DocumentRepository_GetAuditLogs_HasSavedChanges()
        //{
        //    var document = new Document
        //    {
        //        FileName = "BOGUS",
        //        DirectoryID = "A0001",
        //        CaptureDate = DateTime.Now
        //    };

        //    baseDocumentRepository.Create(document);

        //    document.FileName = "FUGAZI";
        //    baseDocumentRepository.Update(document, document.GUID);

        //    var auditLogs = baseDocumentRepository.GetAuditLogs(document.GUID);

            
        //}
    }
}

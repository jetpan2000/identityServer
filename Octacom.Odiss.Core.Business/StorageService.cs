using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using Octacom.Odiss.Core.Contracts;
using Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.Contracts.Settings;

namespace Octacom.Odiss.Core.Business
{
    public class StorageService : IStorageService
    {
        private readonly ICachingService cachingService;
        private readonly IDbContextFactory<DbContext> dbContextFactory;
        private readonly ISettingsService settingsService;
        private const string UPLOAD_STRING = "UPLOAD";

        public StorageService(ICachingService cachingService, IDbContextFactory<DbContext> dbContextFactory, ISettingsService settingsService)
        {
            this.cachingService = cachingService;
            this.dbContextFactory = dbContextFactory;
            this.settingsService = settingsService;
        }

        public Entities.Storage.Directory RetrieveUploadDirectory()
        {
            return this.cachingService.GetOrSet("UploadDirectory", () =>
            {
                using (var ctx = dbContextFactory.Create())
                {
                    var directoryDbSet = ctx.Set<Entities.Storage.Directory>();
                    var locationDbSet = ctx.Set<Entities.Storage.Location>();

                    var directory = directoryDbSet.Include(x => x.Location).FirstOrDefault(x => x.Id == UPLOAD_STRING);

                    if (directory == null)
                    {
                        var location = locationDbSet.FirstOrDefault(x => x.Id == UPLOAD_STRING);

                        if (location == null)
                        {
                            var settings = settingsService.Get();

                            location = new Entities.Storage.Location
                            {
                                Id = UPLOAD_STRING,
                                Volume = UPLOAD_STRING,
                                RPath = settings.DocumentsPath
                            };

                            locationDbSet.Add(location);
                        }

                        directory = new Entities.Storage.Directory
                        {
                            Id = UPLOAD_STRING,
                            LocationId = UPLOAD_STRING,
                            Name = "ALL",
                            Location = location
                        };

                        directoryDbSet.Add(directory);

                        ctx.SaveChanges();
                    }

                    return directory;
                }
            });
        }

        public FileResult SaveFile(byte[] bytes, string filename, Entities.Storage.Directory directory)
        {
            var filePath = Path.Combine(GetDirectoryPath(directory, directory.Location), filename);

            var saveFileResult = new FileResult
            {
                AbsolutePath = filePath,
                OdissDirectory = directory
            };

            if (!Directory.Exists(saveFileResult.DirectoryName))
            {
                Directory.CreateDirectory(saveFileResult.DirectoryName);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(bytes, 0, bytes.Length);

                return saveFileResult;
            }
        }

        public FileResult UploadFile(byte[] bytes, string filename)
        {
            var uploadDirectory = this.RetrieveUploadDirectory();

            return SaveFile(bytes, filename, uploadDirectory);
        }

        public FileResult GetDocumentFile(Document document)
        {
            using (var ctx = dbContextFactory.Create())
            {
                var directory = ctx.Set<Entities.Storage.Directory>().Include(x => x.Location).FirstOrDefault(x => x.Id == document.DirectoryId);
                var location = directory.Location ?? ctx.Set<Entities.Storage.Location>().Find(directory.LocationId);

                return new FileResult
                {
                    AbsolutePath = Path.Combine(GetDirectoryPath(directory, location), document.FileName),
                    OdissDirectory = directory
                };
            }
        }

        public FileResult GenerateUniqueFilePath(Entities.Storage.Directory directory, Entities.Storage.Location location, string fileExtension)
        {
            var fullDirPath = this.GetDirectoryPath(directory, location);

            string filePath = string.Empty;

            do
            {
                filePath = Path.Combine(fullDirPath, Guid.NewGuid().ToString() + fileExtension);
            }
            while (File.Exists(filePath));

            return new FileResult
            {
                AbsolutePath = filePath,
                OdissDirectory = directory
            };
        }

        public FileResult GenerateUniqueUploadFilePath(string extension)
        {
            var uploadDirectory = this.RetrieveUploadDirectory();

            return GenerateUniqueFilePath(uploadDirectory, uploadDirectory.Location, extension);
        }

        private string GetDirectoryPath(Entities.Storage.Directory directory, Entities.Storage.Location location)
        {
            var settings = settingsService.Get();

            return Path.Combine(location.RPath ?? settings.DocumentsPath, location.Volume, directory.Name);
        }
    }
}
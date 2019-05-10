using System.IO;

namespace Octacom.Odiss.Core.Contracts
{
    public class FileResult
    {
        public string AbsolutePath { get; set; }
        public string DirectoryName => Path.GetDirectoryName(AbsolutePath);
        public string FileName => Path.GetFileName(AbsolutePath);
        public Entities.Storage.Directory OdissDirectory { get; set; }
    }
}

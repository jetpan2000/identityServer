using System;

namespace Octacom.Odiss.Core.Entities.User
{
    [Flags]
    public enum UserPermission
    {
        None = 0,       // 0
        ViewDocuments = 1,       // 1
        ExportResults = 1 << 1,  // 2
        ViewNotes = 1 << 2,  // 4
        AddNotes = 1 << 3,  // 8
        DeleteNotes = 1 << 4,  // 16
        EditProperties = 1 << 5,  // 32
        PrintDocuments = 1 << 6,  // 64
        SaveDocuments = 1 << 7,  // 128
        ViewAudits = 1 << 8,  // 256
        AnyApplication = 1 << 9,  // 512
        SubmitDocuments = 1 << 10, // 1024
        EmailDocument = 1 << 11, // 2048
        ViewReports = 1 << 12, // 4096
        ViewHiddenFields = 1 << 13, // 8192
        DeleteDocuments = 1 << 14, // 16384
        All = int.MaxValue         // 2147483647
    }
}

using Octacom.Odiss.Core.Entities.Application.Custom;
using System;

namespace Octacom.Odiss.Core.Entities.Application
{
    public class Application
    {
        public Guid ID { get; set; }
        public Guid IDDatabase { get; set; }
        public Guid? IDFieldGroupBy { get; set; }
        public string Name { get; set; }
        public string DB { get; set; }
        public string DBSchema { get; set; }
        public string DocumentsPath { get; set; }
        public string SortBy { get; set; }
        public string SearchTitle { get; set; }
        public bool RestrictDocumentAccess { get; set; }
        public Field[] Fields { get; set; }
        public Field[] FieldsItems { get; set; }
        public Field[] FieldsSummary { get; set; }
        public Field[] FieldsComment { get; set; }
        public int TabOrder { get; set; }
        public bool EnablePages { get; set; }
        public bool EnableProperties { get; set; }
        public bool EnableEditProperties { get; set; }
        public bool EnableNotes { get; set; }
        public bool EnableEmail { get; set; }
        public bool EnableSubmitDocuments { get; set; }
        public bool EnableReports { get; set; }
        public ApplicationType Type { get; set; }
        //public ApplicationTypeEnum Type { get; set; }
        public string CustomData { get; set; }
        public AppCustomData Custom { get; set; }
        public string TableName { get; set; }
        //public CustomAction[] CustomActions { get; set; }
        //public string ItemsTable { get; set; }
    }
}

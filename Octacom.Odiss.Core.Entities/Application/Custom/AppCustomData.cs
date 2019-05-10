using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Entities.Application.Custom
{
    public class AppCustomData
    {
        public string BaseNamespace { get; set; }
        public string Namespace { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public Guid InvoiceApplicationID { get; set; }
        public string ItemsTable { get; set; }
        public bool EnableAttachments { get; set; }
        public bool SemiCustom { get; set; }
        public EmailModule EmailModule { get; set; }
        public ViewerModule ViewerModule { get; set; }
        public UploadModule UploadModule { get; set; }
        public WorkflowModule WorkflowModule { get; set; }
        public bool NoWrap { get; set; } = true;
        public List<Guid> HiddenFields { get; set; }
        public Guid? RestrictFieldForTempUser { get; set; }
        public bool OpenViewer { get; set; } = true;
        public List<Guid> FieldsGroupBy { get; set; }
        public bool EnableExport { get; set; } = true;
        public bool EnableSelection { get; set; } = true;
        public bool IsSearchResultResponsive { get; set; } = true;
        public bool GroupMultipleDocuments { get; set; }
        public string RunAfterSubmit { get; set; }
        public string RunAfterEdit { get; set; }
        public bool? IsNewUsersDefault { get; set; }
        public string SaveChangesButtonTitle { get; set; }
        public string SubmitTitle { get; set; }
        public bool ShowTotalRowsBadge { get; set; }
        public bool IgnoreDefaultViewer { get; set; }
        public bool HideTab { get; set; }
        public Guid? SubApplicationId { get; set; }
        public bool AutoSearch { get; set; }
        public string ViewerTitle { get; set; }
        public bool SearchRefreshOnModalClose { get; set; }
        public bool RequiresOneFieldForSearch { get; set; }

        // Application
        public string GroupName { get; set; }
        public string DocumentsView { get; set; }
        public string DocumentsViewAll { get; set; }
        public CustomQuery CustomQuery { get; set; }
    }
}

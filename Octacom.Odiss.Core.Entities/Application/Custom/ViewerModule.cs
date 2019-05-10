using Octacom.Odiss.Core.Entities.Settings;

namespace Octacom.Odiss.Core.Entities.Application.Custom
{
    public class ViewerModule
    {
        public bool DisablePDFLib { get; set; }
        /// <summary>
        /// This property is used if you want to show only certain pages to the user.
        /// Set the DBFieldName where the pages are set, separated by comma (,)
        /// Example: If the file has hundreds of pages and you only want to show 2
        /// </summary>
        public string DBFieldNameVisiblePages { get; set; }
        public string DBFieldNameDocumentName { get; set; }

        /// <summary>
        /// Field with highlight coordinates to show annotations using ONet
        /// </summary>
        public string DBFieldNameHighlightAreas { get; set; }

        /// <summary>
        /// Sort selected fields by specific DB Field
        /// </summary>
        public string DBFieldNameSortFilesBy { get; set; }

        /// <summary>
        /// Overrides the Settings Viewer Type
        /// </summary>
        public ViewerType ViewerType { get; set; }

        public SidebarModule Sidebar { get; set; }

        public class SidebarModule
        {
            public Size Width { get; set; }

            public TabVisibility[] Visibility { get; set; }

            public enum Size
            {
                Regular,
                Medium,
                Large,
                ExtraLarge
            }

            public class TabVisibility
            {
                public string TabName { get; set; }
                public bool IsExpanded { get; set; }
            }
        }
    }
}

using System;

namespace Octacom.Odiss.Core.Settings
{
    /// <summary>
    /// This entity is only ever required to retrieve a row from [dbo].[Fields] in Odiss 5
    /// 
    /// It is not intended to be used for Odiss 6 as we're going to make a better structure
    /// </summary>
    internal class FieldResult
    {
        public Guid ID { get; set; }
        public string UniqueName { get; set; }
        public string Name { get; set; }
        public string MapTo { get; set; }
        public string Format { get; set; }
        public bool IsKey { get; set; }
        public int Type { get; set; }
        public int VisibilityType { get; set; }
        public bool Editable { get; set; }
        public int? FilterOrder { get; set; }
        public int? ResultOrder { get; set; }
        public int? ViewerOrder { get; set; }
        public int? FilterType { get; set; }
        public bool? NotVisibleList { get; set; }
        public bool? NotVisibleViewer { get; set; }
        public bool? NotVisibleSubmit { get; set; }
        public bool? NotVisibleFilter { get; set; }
        public bool? NotVisibleExport { get; set; }
        public string FilterCommand { get; set; }
        public string FilterData { get; set; }
    }
}
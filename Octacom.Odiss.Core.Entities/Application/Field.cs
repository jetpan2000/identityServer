using System;

namespace Octacom.Odiss.Core.Entities.Application
{
    public class Field
    {
        public Guid ID { get; set; }
        public Guid IDApplication { get; set; }
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public string MapTo { get; set; }
        public bool Editable { get; set; }
        //public FieldFilterTypeEnum FilterType { get; set; }
        public string FilterCommand { get; set; }
        public int? FilterOrder { get; set; }
        public int? ResultOrder { get; set; }
        public bool EnableRestriction { get; set; }
        public bool EnableAutoComplete { get; set; }
        public FieldFilterType FilterType { get; set; }
        public string AutoComplete_FilterCommand { get; set; }
        public string AutoComplete_MapFields { get; set; }
        public Application Parent { get; set; }
        public string FilterData { get; set; }
        //public IEnumerable<TreeData> FilterDataNodes { get; set; }
        public string HeaderGroupName { get; set; }
        public string Format { get; set; }
        public bool IsRequired { get; set; }
        //public FieldVisibilityTypeEnum VisibilityType { get; set; }
        public string ShowInView { get; set; }
        public string NameForView { get; set; }
        public bool NotVisibleList { get; set; }
        public bool NotVisibleViewer { get; set; }
        public bool NotVisibleSubmit { get; set; }
        public bool NotVisibleFilter { get; set; }
        public int MaxLen { get; set; }

        //private string _Options { get; set; }
        //private string Options
        //{
        //    get => _Options;
        //    set
        //    {
        //        _Options = value;
        //        if (!string.IsNullOrEmpty(_Options))
        //        {
        //            Settings = System.Web.Helpers.Json.Decode<FieldSettings>(_Options);
        //        }
        //    }
        //}

        //public FieldSettings Settings { get; set; } = new FieldSettings();

        public string DBColumnName => MapTo;
    }

    public enum FieldType
    {
        Number = 1,
        Text = 2,
        DateRange = 3,
        Tree = 4,
        Dropdown = 5,
        Radio = 6,
        NumberRange = 7,
        Button = 8,
        TextArea = 9,
        AutoComplete = 10,
        Array = 11,
        RadioQuery = 12,
        MultipleLink = 13,
        FormattedText = 14,
        BooleanYesNo = 15,
        BooleanCheckbox = 16
    }

    public enum FieldFilterType
    {
        None = 0,
        View = 1,
        Json = 2,
        StoredProcedure = 3,
        Rest = 4
    }
}

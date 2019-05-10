using Octacom.Odiss.Core.Contracts.Settings.Entities;
using Newtonsoft.Json.Linq;

namespace Octacom.Odiss.Core.Settings
{
    internal static class FieldResultExtensions
    {
        public static TField ToField<TField>(this FieldResult fieldResult)
            where TField : Field, new()
        {
            return new TField
            {
                Identifier = fieldResult.UniqueName ?? fieldResult.ID.ToString(),
                DisplayName = fieldResult.Name,
                Type = fieldResult.Type,
                IsKey = fieldResult.IsKey,
                MapTo = fieldResult.MapTo,
                Format = fieldResult.Format
            };
        }

        public static GridField ToGridField(this FieldResult fieldResult)
        {
            var field = fieldResult.ToField<GridField>();
            field.Order = fieldResult.ResultOrder ?? int.MaxValue;

            return field;
        }

        public static SearchField ToSearchField(this FieldResult fieldResult)
        {
            var field = fieldResult.ToField<SearchField>();

            SetupSearchableField(field, fieldResult);

            return field;
        }

        public static void SetupSearchableField(ISearchableField field, FieldResult fieldResult)
        {
            field.SearchConfiguration = ParseSearchFieldConfiguration(fieldResult.FilterData);
            field.FilterCommand = fieldResult.FilterCommand;
            field.FilterData = fieldResult.FilterData;
            field.FilterType = fieldResult.FilterType ?? 0;
        }

        public static PropertyField ToPropertyField(this FieldResult fieldResult)
        {
            return fieldResult.ToGenericPropertyField<PropertyField>();
        }

        public static TPropertyField ToGenericPropertyField<TPropertyField>(this FieldResult fieldResult)
            where TPropertyField : PropertyField, new()
        {
            var field = fieldResult.ToField<TPropertyField>();

            field.IsEditable = field.IsEditable;
            field.Order = fieldResult.ViewerOrder ?? fieldResult.ResultOrder ?? int.MaxValue;

            return field;
        }

        public static LookupPropertyField ToLookupPropertyField(this FieldResult fieldResult)
        {
            var field = fieldResult.ToGenericPropertyField<LookupPropertyField>();

            SetupSearchableField(field, fieldResult);

            return field;
        }

        private static SearchFieldConfiguration ParseSearchFieldConfiguration(string filterData)
        {
            if (string.IsNullOrEmpty(filterData))
            {
                return null;
            }

            var obj = JObject.Parse(filterData);

            var search = obj.ContainsKey("search") ? obj["search"].ToObject<SearchFieldConfiguration>() : new SearchFieldConfiguration();
            search.DisplayFormat = obj.ContainsKey("displayFormat") ? obj["displayFormat"].ToString() : null;

            if (obj.ContainsKey("value") && obj.ContainsKey("text"))
            {
                search.AutocompleteConfiguration = new SearchFieldAutocompleteConfiguration
                {
                    Text = obj["text"].ToString(),
                    Value = obj["value"].ToString()
                };
            }

            return search;
        }
    }
}

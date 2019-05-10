using Octacom.Odiss.Core.Contracts.Validation.Entities;
using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Settings.Entities
{
    public interface IField
    {
        /// <summary>
        /// In Odiss5 this is the ID column (Guid) but moving forward this is going to be a string which tells in human language what it is
        /// </summary>
        string Identifier { get; set; }

        [Obsolete("Warning - This property possibly will be deprecated in the future")]
        string MapTo { get; set; }
    }

    public class Field : IField
    {
        /// <summary>
        /// In Odiss5 this is the ID column (Guid) but moving forward this is going to be a string which tells in human language what it is
        /// </summary>
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        public string Format { get; set; }
        public bool IsKey { get; set; }
        [Obsolete("Warning - This property possibly will be deprecated in the future")]
        public string MapTo { get; set; }
        public int Order { get; set; }
        public int Type { get; set; } // TODO - Create a enum type
    }

    public class GridField : Field
    {
    }

    public class PropertyField : Field
    {
        public bool IsEditable { get; set; }
        public IEnumerable<ValidationRule> ValidationRules { get; set; }
    }

    public class LookupPropertyField : PropertyField, ISearchableField
    {
        public string FilterCommand { get; set; }
        public string FilterData { get; set; }
        public int FilterType { get; set; }
        public SearchFieldConfiguration SearchConfiguration { get; set; }
    }
}
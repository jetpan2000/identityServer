using Octacom.Odiss.Core.Contracts.Settings.Entities;
using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Settings
{
    public class Application
    {
        /// <summary>
        /// In Odiss5 this is the ID column (Guid) but moving forward this is going to be a string which tells in human language what it is
        /// </summary>
        public string Identifier { get; set; }
        public string DisplayName { get; set; }

        [Obsolete("Warning - This property possibly will be deprecated in the future")]
        public bool EnableCreate { get; set; }
        [Obsolete("Warning - This property possibly will be deprecated in the future")]
        public bool EnableUpdate { get; set; }
        [Obsolete("Warning - This property possibly will be deprecated in the future")]
        public bool EnableDelete { get; set; }

        public IEnumerable<Field> HiddenFields { get; set; }
        public IEnumerable<SearchField> SearchFields { get; set; }
        public IEnumerable<GridField> GridFields { get; set; }
        public IEnumerable<PropertyField> PropertyFields { get; set; }
        public IEnumerable<LookupPropertyField> LookupPropertyFields { get; set; }

        /// <summary>
        /// This will be phased out in the future as Odiss 6 gets built
        /// </summary>
        public string CustomData { get; set; }
    }
}
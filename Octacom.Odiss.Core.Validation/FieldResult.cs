using Newtonsoft.Json;
using System;

namespace Octacom.Odiss.Core.Validation
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
        public string ValidationRulesJson { get; set; }
        public FieldValidation ValidationRules
        {
            get
            {
                if (!string.IsNullOrEmpty(ValidationRulesJson))
                {
                    return JsonConvert.DeserializeObject<FieldValidation>(ValidationRulesJson);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ValidationRulesJson = JsonConvert.SerializeObject(value);
            }
        }
    }

    internal class FieldValidation
    {
        public bool IsRequired { get; set; } = false;
        public bool IsAlpha { get; set; }
        public bool IsAlphanumeric { get; set; } = false;
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }
}
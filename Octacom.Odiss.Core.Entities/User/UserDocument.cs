using System;

namespace Octacom.Odiss.Core.Entities.User
{
    public class UserDocument
    {
        public Guid Id { get; set; }
        public Guid IDUser { get; set; }
        public Guid IDApplication { get; set; }
        public Guid IDField { get; set; }
        public object FieldValue { get; set; }
        public string FieldText { get; set; }
        public string TreeID { get; set; }
    }
}

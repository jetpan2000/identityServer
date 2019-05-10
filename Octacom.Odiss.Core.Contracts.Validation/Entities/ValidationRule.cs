using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Validation.Entities
{
    // IDEAS
    // *   Use this class as a inherited class for each validation rule implementation. An validation rule implementation consists
    //     of how the rule is applied in pseudo-code with a parser which can convert it to C# code. The reason for the pseudo-code is
    //     to be able to work with various different programming languages (e.g. C#, JavaScript, even VB.NET). There would need to be
    //     some sort of Rule Engine implementation for each. The major benefit is to accomplish no replication of object validations
    //     across various different systems (client side JavaScript, Odiss back-end system and perhaps QC validator or something else)
    //
    //     For now Method will be used to describe what needs to be performed. So we're really only getting a description now as to what
    //     needs to be validated and under what rules. Nothing about how that it's done.
    public class ValidationRule
    {
        public string FieldIdentifier { get; set; }
        public string Method { get; set; }
        public string InvalidMessage { get; set; }
        public IEnumerable<object> Arguments { get; set; }
    }
}

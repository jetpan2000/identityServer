using System.Collections.Generic;
using Octacom.Odiss.Core.Contracts.Validation.Entities;

namespace Octacom.Odiss.Core.Contracts.Validation
{
    /// <summary>
    /// Interface to retrieve rules for validating fields within Odiss (does not perform the actual validations, only describes how to do them)
    /// </summary>
    public interface IFieldValidationProvider
    {
        IEnumerable<ValidationRule> GetRules(string applicationIdentifier);
    }
}
using System;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public enum FilterType
    {
        Like,
        StartsWith,
        EndsWith,
        Equals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        IsNull,
        IsNotNull
    }
}

﻿using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public abstract class SearchParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.Contracts.Repositories.Searching
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.DataLayer.Search instead")]
    public class UserSearchParameters : SearchParameters
    {
        public SearchFilter<bool> Active { get; set; } = new SearchFilter<bool>(true, FilterType.Equals);
        public SearchFilter<string> UserName { get; set; } = new SearchFilter<string>(FilterType.Like);
        public SearchFilter<string> FirstName { get; set; } = new SearchFilter<string>(FilterType.Like);
        public SearchFilter<string> LastName { get; set; } = new SearchFilter<string>(FilterType.Like);
        public SearchFilter<byte?> Type { get; set; } = new SearchFilter<byte?>(FilterType.Equals);
    }
}

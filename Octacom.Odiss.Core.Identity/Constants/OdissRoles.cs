using System.Collections.Generic;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Constants
{
    public class OdissRoles
    {
        public static IEnumerable<OdissRole> AllRoles => new List<OdissRole> { Regular, Temporary, Administrator, Super, Octacom };
        public static OdissRole Regular => new OdissRole { Id = 0, Name = "Regular" };
        public static OdissRole Temporary => new OdissRole { Id = 1, Name = "Temporary" };
        public static OdissRole Administrator => new OdissRole { Id = 2, Name = "Administrator" };
        public static OdissRole Super => new OdissRole { Id = 3, Name = "Super" };
        public static OdissRole Octacom => new OdissRole { Id = 4, Name = "Octacom" };
    }
}

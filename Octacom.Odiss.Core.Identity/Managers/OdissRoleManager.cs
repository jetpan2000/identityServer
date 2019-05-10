using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Managers
{
    public class OdissRoleManager : RoleManager<OdissRole, byte>
    {
        public OdissRoleManager(IRoleStore<OdissRole, byte> store) : base(store)
        {
        }
    }
}

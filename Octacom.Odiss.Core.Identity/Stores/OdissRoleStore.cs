using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.Identity.Constants;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Stores
{
    public class OdissRoleStore : IRoleStore<OdissRole, byte>
    {
        public Task<OdissRole> FindByIdAsync(byte roleId)
        {
            return Task.FromResult(OdissRoles.AllRoles.SingleOrDefault(x => x.Id == roleId));
        }

        public Task<OdissRole> FindByNameAsync(string roleName)
        {
            return Task.FromResult(OdissRoles.AllRoles.SingleOrDefault(x => x.Name == roleName));
        }

        public Task CreateAsync(OdissRole role)
        {
            throw new NotSupportedException("User Roles are currently hard coded");
        }

        public Task DeleteAsync(OdissRole role)
        {
            throw new NotSupportedException("User Roles are currently hard coded");
        }

        public Task UpdateAsync(OdissRole role)
        {
            throw new NotSupportedException("User Roles are currently hard coded");
        }

        public void Dispose()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Identity.Contracts;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Implementations
{
    public class OdissUserRepository : IOdissUserRepository
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly IUserGroupRepository groupRepository;

        public OdissUserRepository(IApplicationRepository applicationRepository, IUserGroupRepository groupRepository)
        {
            this.applicationRepository = applicationRepository;
            this.groupRepository = groupRepository;
        }

        public IEnumerable<Guid> GetUserApplications(Guid userId)
        {
            var applications = applicationRepository.GetByUserId(userId);

            return applications.Select(x => x.ID);
        }

        public IEnumerable<Guid> GetUserGroups(Guid userId)
        {
            var groups = groupRepository.GetByUserId(userId);

            return groups.Select(x => x.ID);
        }
    }
}

using Octacom.Odiss.Core.Common;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Services;
using System;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IPrincipalService principalService;

        public UserService(IUserRepository userRepository, IPrincipalService principalService)
        {
            this.userRepository = userRepository;
            this.principalService = principalService;
        }

        public ServiceResult UnlockUser(Guid userId)
        {
            var user = userRepository.Get(userId);

            user.WrongAccessAttempts = null;
            user.LockAccessUntil = null;

            var updateResult = userRepository.Update(user, user.Id);

            return new ServiceResult
            {
                IsSuccess = updateResult.key != null
            };
        }

        public bool HasPermission<TUserPermission>(TUserPermission permission)
            where TUserPermission : Enum, IConvertible
        {
            var principal = principalService.GetCurrentPrincipal();

            var user = userRepository.GetByUsername(principal.Identity.Name);

            if (user == null)
            {
                return false;
            }

            var permissions = EnumHelper.ConvertTo<TUserPermission>(user.Permissions);
            var userType = (UserType)user.Type;

            switch (userType)
            {
                case UserType.Super:
                case UserType.Octacom:
                    return true;

                case UserType.Administrator:
                    // Administrators should have ViewAudit permission if they want to access the Audit page.
                    // It's not activated by default. Only super users have access with no restriction.
                    return Convert.ToInt32(permission) != ((int)UserPermission.ViewAudits);

                default:
                    return permissions.HasFlag(permission);
            }
        }

        public User GetCurrentLoggedInUser()
        {
            var principal = principalService.GetCurrentPrincipal();

            if (principal?.Identity == null)
            {
                return null;
            }

            return userRepository.GetByUsername(principal.Identity.Name);
        }
    }
}

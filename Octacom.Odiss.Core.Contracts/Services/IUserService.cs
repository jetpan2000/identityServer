using System;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.Contracts.Services
{
    public interface IUserService
    {
        ServiceResult UnlockUser(Guid userId);
        bool HasPermission<TUserPermission>(TUserPermission permission)
            where TUserPermission : Enum, IConvertible;
        User GetCurrentLoggedInUser();
    }
}

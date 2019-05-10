using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Dapper;
using Octacom.Odiss.Core.Identity.Entities;
using Octacom.Odiss.Core.Identity.Constants;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Utilities;

namespace Octacom.Odiss.Core.Identity.Dapper
{
    public class OdissUserStore : IOdissUserStore
    {
        private readonly IUserSessionStore<Guid, Guid> sessionStore;
        private readonly Serialization serialization;

        public OdissUserStore(IUserSessionStore<Guid, Guid> sessionStore, Serialization serialization)
        {
            this.sessionStore = sessionStore;
            this.serialization = serialization;
        }

        public async Task CreateAsync(OdissUser user)
        {
            using (var db = new MainDatabase().Get)
            {
                var sql = "INSERT INTO [dbo].[Users] (UserName, Password, Type, FirstName, LastName, PhoneOffice, PhoneMobile, Email, Company, Expire, Permissions, Active, LockAccessUntil, WrongAccessAttempts) VALUES (@UserName, @Password, @Type, @FirstName, @LastName, @PhoneOffice, @PhoneMobile, @Email, @Company, @Expire, @Permissions, @Active, @LockAccessUntil, @WrongAccessAttempts)";
                await db.ExecuteAsync(sql, user);
            }
        }

        public async Task UpdateAsync(OdissUser user)
        {
            using (var db = new MainDatabase().Get)
            {
                var sql = "UPDATE [dbo].[Users] SET UserName = @UserName, Password = @Password, Type = @Type, FirstName = @FirstName, LastName = @LastName, PhoneOffice = @PhoneOffice, PhoneMobile = @PhoneMobile, Email = @Email, Company = @Company, Expire = @Expire, Permissions = @Permissions, Active = @Active, LockAccessUntil = @LockAccessUntil, WrongAccessAttempts = @WrongAccessAttempts WHERE ID = @Id";
                await db.ExecuteAsync(sql, user);
            }
        }

        public async Task DeleteAsync(OdissUser user)
        {
            using (var db = new MainDatabase().Get)
            {
                var sql = "DELETE FROM [dbo].[Users] WHERE Id = @Id";
                await db.ExecuteAsync(sql, user);
            }
        }

        public void Dispose()
        {
        }

        public async Task<OdissUser> FindByIdAsync(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryFirstOrDefaultAsync<OdissUser>("SELECT * FROM [dbo].[Users] WHERE ID = @userId", new { userId });
            }
        }

        public async Task<OdissUser> FindByNameAsync(string userName)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryFirstOrDefaultAsync<OdissUser>("SELECT * FROM [dbo].[Users] WHERE UserName = @userName", new { userName });
            }
        }

        public Task<int> GetAccessFailedCountAsync(OdissUser user)
        {
            return Task.FromResult(user.WrongAccessAttempts ?? 0);
        }

        public Task<bool> GetLockoutEnabledAsync(OdissUser user)
        {
            return Task.FromResult(true);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(OdissUser user)
        {
            var lockOutDate = user.LockAccessUntil.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(user.LockAccessUntil.Value, DateTimeKind.Local))
                : new DateTimeOffset();

            return Task.FromResult(lockOutDate);
        }

        public Task<int> IncrementAccessFailedCountAsync(OdissUser user)
        {
            int increasedValue = (user.WrongAccessAttempts ?? 0) + 1;
            user.WrongAccessAttempts = increasedValue;

            return Task.FromResult(increasedValue);
        }

        public Task ResetAccessFailedCountAsync(OdissUser user)
        {
            user.WrongAccessAttempts = null;

            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(OdissUser user, bool enabled)
        {
            throw new NotSupportedException("All Users have Lockout Enabled and there is no mechanism to change that per user");
        }

        public Task SetLockoutEndDateAsync(OdissUser user, DateTimeOffset lockoutEnd)
        {
            user.LockAccessUntil = lockoutEnd.LocalDateTime;

            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(OdissUser user, string passwordHash)
        {
            user.Password = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(OdissUser user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(OdissUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public Task SetTwoFactorEnabledAsync(OdissUser user, bool enabled)
        {
            return Task.FromResult(0); // Not using Two Factor authentication
        }

        public Task<bool> GetTwoFactorEnabledAsync(OdissUser user)
        {
            return Task.FromResult(false); // Not using Two Factor authentication
        }

        public Task AddToRoleAsync(OdissUser user, string roleName)
        {
            var role = OdissRoles.AllRoles.SingleOrDefault(x => x.Name == roleName);

            if (role == null)
            {
                throw new ArgumentException($"User role ${roleName} does not exists");
            }

            user.Type = role.Id;

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(OdissUser user, string roleName)
        {
            var role = OdissRoles.AllRoles.SingleOrDefault(x => x.Name == roleName);

            if (role == null)
            {
                throw new ArgumentException($"User role ${roleName} does not exists");
            }

            if (user.Type != role.Id)
            {
                throw new Exception($"User is not in role ${roleName}");
            }

            user.Type = null;

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(OdissUser user)
        {
            IList<string> roles = OdissRoles.AllRoles
                .Where(x => x.Id == user.Type)
                .Select(x => x.Name)
                .ToList();

            return Task.FromResult(roles);
        }

        public async Task<bool> IsInRoleAsync(OdissUser user, string roleName)
        {
            var roles = await GetRolesAsync(user);

            return roles.Contains(roleName);
        }

        public Task SetEmailAsync(OdissUser user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(OdissUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(OdissUser user)
        {
            return Task.FromResult(true); // Not using this
        }

        public Task SetEmailConfirmedAsync(OdissUser user, bool confirmed)
        {
            return Task.FromResult(0); // Not using this
        }

        public async Task<OdissUser> FindByEmailAsync(string email)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryFirstOrDefaultAsync<OdissUser>("SELECT * FROM [dbo].[Users] WHERE Email = @email", new { email });
            }
        }

        public async Task<IList<Claim>> GetClaimsAsync(OdissUser user)
        {
            var session = await sessionStore.GetByUserIdAsync(user.Id);

            if (session == null)
            {
                throw new NullReferenceException("No session for user found");
            }

            return serialization.Deserialize<IList<Claim>>(session.Data);
        }

        public async Task SetClaimsAsync(OdissUser user, IList<Claim> claims)
        {
            var session = await sessionStore.GetByUserIdAsync(user.Id);

            if (session == null)
            {
                throw new NullReferenceException("No session for user found");
            }

            session.Data = serialization.Serialize(claims);

            await sessionStore.UpdateAsync(session);
        }

        public async Task AddClaimAsync(OdissUser user, Claim claim)
        {
            var session = await sessionStore.GetByUserIdAsync(user.Id);

            if (session == null)
            {
                throw new NullReferenceException("No session for user found");
            }

            var claims = serialization.Deserialize<IList<Claim>>(session.Data);
            claims.Add(claim);
            session.Data = serialization.Serialize(claims);

            await sessionStore.UpdateAsync(session);
        }

        public async Task RemoveClaimAsync(OdissUser user, Claim claim)
        {
            var session = await sessionStore.GetByUserIdAsync(user.Id);

            if (session == null)
            {
                throw new NullReferenceException("No session for user found");
            }

            var claims = serialization.Deserialize<IList<Claim>>(session.Data);
            var claimToRemove = claims.FirstOrDefault(x => x.Type == claim.Type);
            
            if (claimToRemove == null)
            {
                return;
            }

            claims.Remove(claimToRemove);
            session.Data = serialization.Serialize(claims);

            await sessionStore.UpdateAsync(session);
        }
    }
}

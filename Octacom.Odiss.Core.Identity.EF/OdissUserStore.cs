using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.DataLayer;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.EF
{
    public class OdissUserStore : IUserStore<OdissUser, Guid>, IUserLockoutStore<OdissUser, Guid>, IUserPasswordStore<OdissUser, Guid>, IUserTwoFactorStore<OdissUser, Guid>
    {
        private readonly IDbContextFactory dbContextFactory;

        public OdissUserStore(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task CreateAsync(OdissUser user)
        {
            using (var ctx = dbContextFactory.Get())
            {
                ctx.Set<OdissUser>().Add(user);

                await ctx.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(OdissUser user)
        {
            using (var ctx = dbContextFactory.Get())
            {
                ctx.Set<OdissUser>().Remove(user);

                await ctx.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
        }

        public async Task<OdissUser> FindByIdAsync(Guid userId)
        {
            using (var ctx = dbContextFactory.Get())
            {
                return await ctx.Set<OdissUser>().FindAsync(userId);
            }
        }

        public async Task<OdissUser> FindByNameAsync(string userName)
        {
            using (var ctx = dbContextFactory.Get())
            {
                return await ctx.Set<OdissUser>().FirstOrDefaultAsync(x => x.UserName == userName);
            }
        }

        public async Task UpdateAsync(OdissUser user)
        {
            using (var ctx = dbContextFactory.Get())
            {
                var dbSet = ctx.Set<OdissUser>();
                var existing = await dbSet.FindAsync(user.Id);

                ctx.Entry(existing).State = EntityState.Detached;
                ctx.Entry(user).State = EntityState.Modified;

                await ctx.SaveChangesAsync();
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
            var dateTime = user.LockAccessUntil ?? DateTime.MinValue;
            return Task.FromResult((DateTimeOffset)dateTime);
        }

        public async Task<int> IncrementAccessFailedCountAsync(OdissUser user)
        {
            int increasedValue = (user.WrongAccessAttempts ?? 0) + 1;
            user.WrongAccessAttempts = increasedValue;

            await UpdateAsync(user);

            return increasedValue;
        }

        public async Task ResetAccessFailedCountAsync(OdissUser user)
        {
            user.WrongAccessAttempts = 0;
            user.LockAccessUntil = null;

            await UpdateAsync(user);
        }

        public async Task SetLockoutEnabledAsync(OdissUser user, bool enabled)
        {
            if (enabled)
            {
                user.LockAccessUntil = DateTime.Now.AddMinutes(30);
            }
            else
            {
                user.LockAccessUntil = null;
            }

            await UpdateAsync(user);
        }

        public async Task SetLockoutEndDateAsync(OdissUser user, DateTimeOffset lockoutEnd)
        {
            user.LockAccessUntil = lockoutEnd.Date;

            await UpdateAsync(user);
        }

        public async Task SetPasswordHashAsync(OdissUser user, string passwordHash)
        {
            user.Password = passwordHash;

            await UpdateAsync(user);
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
    }
}

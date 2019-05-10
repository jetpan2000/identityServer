using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Contracts
{
    public interface IUserSessionStore<TUserKey, TSessionKey>
    {
        /// <summary>
        /// Creates a new login session for userId. Returns the id of the session
        /// </summary>
        Task<TSessionKey> CreateAsync(TUserKey userId, DateTime expiryDate);

        /// <summary>
        /// Get session
        /// </summary>
        Task<UserSession<TUserKey, TSessionKey>> GetAsync(TSessionKey sessionId);

        /// <summary>
        /// Get session by userId
        /// </summary>
        Task<UserSession<TUserKey, TSessionKey>> GetByUserIdAsync(TUserKey userId);

        /// <summary>
        /// Updates the session
        /// </summary>
        Task<bool> UpdateAsync(UserSession<TUserKey, TSessionKey> session);

        /// <summary>
        /// Removes the session
        /// </summary>
        Task<bool> RemoveAsync(TSessionKey sessionId);

        /// <summary>
        /// Removes the session by userId
        /// </summary>
        Task<bool> RemoveByUserIdAsync(TUserKey userId);

        /// <summary>
        /// Deletes all expired user sessions from the database
        /// </summary>
        /// <returns>All user session records which were deleted as a result of the call</returns>
        Task<IEnumerable<UserSession<TUserKey, TSessionKey>>> ClearSessionsAsync();
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Utilities;

namespace Octacom.Odiss.Core.Identity.Managers
{
    public class UserSessionManager<TUserKey, TSessionKey> : IDisposable
    {
        private readonly IUserSessionStore<TUserKey, TSessionKey> store;
        private readonly IAuthenticationManager authenticationManager;
        private readonly ILogger logger;

        /// <summary>
        /// Amount of time that sessions are active from creation/extension
        /// </summary>
        public TimeSpan MaximumSessionTimeout { get; set; }

        public UserSessionManager(IUserSessionStore<TUserKey, TSessionKey> store, IAuthenticationManager authenticationManager, ILogger logger)
        {
            this.store = store;
            this.authenticationManager = authenticationManager;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new user sessions and returns the sessionId
        /// </summary>
        public async Task<TSessionKey> CreateAsync(TUserKey userId)
        {
            await store.RemoveByUserIdAsync(userId);
            var sessionId = await store.CreateAsync(userId, DateTime.Now.Add(MaximumSessionTimeout));
            logger.LogSystemActivity("User Session Created", new { userId, sessionId });

            var deletedSessions = await store.ClearSessionsAsync();
            if (deletedSessions.Any())
            {
                logger.LogSystemActivity("Expired User Sessions Deleted", deletedSessions.Select(x => x.ToLogMessage()));
            }

            return sessionId;
        }

        /// <summary>
        /// Extends the session by the configured maximum session timeout
        /// </summary>
        public async Task ExtendAsync(TSessionKey sessionId)
        {
            var session = await store.GetAsync(sessionId);
            session.ExpiryDate = DateTime.Now.Add(MaximumSessionTimeout);
            session.LastActionAt = DateTime.Now;

            await store.UpdateAsync(session);
            logger.LogSystemActivity("User Session Extended", session.ToLogMessage());

            var deletedSessions = await store.ClearSessionsAsync();
            if (deletedSessions.Any())
            {
                logger.LogSystemActivity("Expired User Sessions Deleted", deletedSessions.Select(x => x.ToLogMessage()));
            }
        }

        /// <summary>
        /// Checks to see if the session is still active
        /// </summary>
        public async Task<bool> IsActiveAsync(TSessionKey sessionId)
        {
            var session = await store.GetAsync(sessionId);

            if (session == null)
            {
                return false;
            }

            return session.ExpiryDate >= DateTime.Now;
        }

        /// <summary>
        /// Removes the session
        /// </summary>
        public async Task RemoveAsync(TSessionKey sessionId)
        {
            await store.RemoveAsync(sessionId);
            logger.LogSystemActivity("User Session removed", new { sessionId });
        }

        public void Dispose()
        {
        }
    }
}

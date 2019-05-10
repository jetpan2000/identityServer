using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Utilities
{
    internal static class LoggingExtensions
    {
        internal static UserSessionLogMessage<TUserKey, TSessionKey> ToLogMessage<TUserKey, TSessionKey>(this UserSession<TUserKey, TSessionKey> userSession)
        {
            return new UserSessionLogMessage<TUserKey, TSessionKey>
            {
                CreatedAt = userSession.CreatedAt,
                ExpiryDate = userSession.ExpiryDate,
                LastActionAt = userSession.LastActionAt,
                SessionId = userSession.SessionId,
                UserId = userSession.UserId
            };
        }
    }
}

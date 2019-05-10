using System;

namespace Octacom.Odiss.Core.Identity.Entities
{
    public class UserSession<TUserKey, TSessionKey>
    {
        public TSessionKey SessionId { get; set; }
        public TUserKey UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActionAt { get; set; }
    }

    internal class UserSessionLogMessage<TUserKey, TSessionKey>
    {
        public TSessionKey SessionId { get; set; }
        public TUserKey UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActionAt { get; set; }
    }
}

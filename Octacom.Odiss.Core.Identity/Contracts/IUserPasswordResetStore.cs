using System;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.Identity.Contracts
{
    /// <summary>
    /// Store that gains access to reset password tokens and to register them
    /// </summary>
    public interface IUserPasswordResetStore
    {
        void Initialize(TimeSpan expiryLength);

        Task<UserPasswordReset> GetUserPasswordResetAsync(string passwordResetKey);
        Task<string> RegisterPasswordResetTokenAsync(Guid userId, string passwordResetToken);
        Task ClearPasswordResetTokensAsync(Guid userId);
    }

    public class UserPasswordReset
    {
        public string PasswordResetKey { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PasswordResetToken { get; set; }
    }

    public enum UserPasswordResetStatus
    {
        NotExists,
        Expired,
        Valid
    }
}

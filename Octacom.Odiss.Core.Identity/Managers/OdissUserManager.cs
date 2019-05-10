using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Owin;
using Octacom.Odiss.Core.Identity.Constants;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Entities;
using Octacom.Odiss.Core.Contracts.Infrastructure;

namespace Octacom.Odiss.Core.Identity.Managers
{
    public class OdissUserManager : UserManager<OdissUser, Guid>
    {
        private readonly IOdissUserStore store;
        private readonly IOdissUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly IUserPasswordResetStore userPasswordResetStore;
        private readonly UserSessionManager<Guid, Guid> userSessionManager;
        private readonly ILogger logger;

        public OdissUserManager(IOdissUserStore store, IAppBuilder app, IOdissUserRepository userRepository, IEmailService emailService, IUserPasswordResetStore userPasswordResetStore, UserSessionManager<Guid, Guid> userSessionManager, ILogger logger) : base(store)
        {
            this.store = store;
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.userPasswordResetStore = userPasswordResetStore;
            this.userSessionManager = userSessionManager;
            this.logger = logger;
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(OdissUser user, string authenticationType)
        {
            var sessionId = await userSessionManager.CreateAsync(user.Id);

            var claims = new List<Claim>()
            {
                new Claim(OdissClaims.Id, user.Id.ToString()),
                new Claim(OdissClaims.SessionId, sessionId.ToString()),
                new Claim(OdissClaims.UserName, user.UserName),
                new Claim(OdissClaims.UserType, user.Type.ToString()),
                new Claim(OdissClaims.Permissions, user.Permissions.ToString()),
                new Claim(OdissClaims.Applications, string.Join(",", userRepository.GetUserApplications(user.Id))),
                new Claim(OdissClaims.Groups, string.Join(",", userRepository.GetUserGroups(user.Id))),
                new Claim(OdissClaims.Email, user.Email),
                new Claim(OdissClaims.FirstName, user.FirstName),
                new Claim(OdissClaims.LastName, user.LastName)
            };

            await store.SetClaimsAsync(user, claims);

            return await base.CreateIdentityAsync(user, authenticationType);
        }

        public async Task SendForgotPasswordEmailAsync(string email, string callbackUrl)
        {
            var user = await FindByEmailAsync(email);
            var token = await GeneratePasswordResetTokenAsync(user.Id);
            var passwordResetKey = await userPasswordResetStore.RegisterPasswordResetTokenAsync(user.Id, token);

            var emailData = emailService.GenerateForgotPasswordEmail(user, passwordResetKey, callbackUrl);
            await emailService.SendAsync(emailData);
            logger.LogActivity("SendForgotPasswordEmail", new { email, callbackUrl, passwordResetKey, emailData });
        }

        public async Task SendForgotUsernameEmailAsync(string email)
        {
            var user = await FindByEmailAsync(email);

            var emailData = emailService.GenerateForgotUsernameEmail(user);
            await emailService.SendAsync(emailData);
            logger.LogActivity("SendForgotUsernameEmail", new { email, emailData });
        }

        public async Task<IdentityResult> ResetPasswordAsync(string passwordResetKey, string newPassword)
        {
            // Retrieve password reset token from IUserPasswordResetStore
            var userPasswordReset = await userPasswordResetStore.GetUserPasswordResetAsync(passwordResetKey);

            if (userPasswordReset == null)
            {
                string error = "No active password reset token stored";
                logger.LogActivity("ResetPassword", new { userPasswordReset, error });
                return new IdentityResult(new string[] { error });
            }

            if (userPasswordReset.ExpiryDate < DateTime.Now)
            {
                string error = "Password reset key is expired";
                logger.LogActivity("ResetPassword", new { userPasswordReset, error });
                return new IdentityResult(new string[] { error });
            }

            // Call ResetPassword method of the base class
            var passwordResetResult = await base.ResetPasswordAsync(userPasswordReset.UserId, userPasswordReset.PasswordResetToken, newPassword);

            if (!passwordResetResult.Succeeded)
            {
                logger.LogActivity("ResetPassword", new { userPasswordReset, errors = passwordResetResult.Errors });
                return passwordResetResult;
            }

            // Send e-mail to the user to notify of the change
            var user = await FindByIdAsync(userPasswordReset.UserId);
            var emailData = emailService.GeneratePasswordChangedEmail(user);
            await emailService.SendAsync(emailData);

            // Clear password reset token for user
            await userPasswordResetStore.ClearPasswordResetTokensAsync(userPasswordReset.UserId);
            logger.LogActivity("ResetPassword", new { userPasswordReset, emailData });

            return passwordResetResult;
        }

        public async Task<UserPasswordResetStatus> CheckUserPasswordResetStatusAsync(string passwordResetKey)
        {
            var userPasswordReset = await userPasswordResetStore.GetUserPasswordResetAsync(passwordResetKey);

            if (userPasswordReset == null)
            {
                return UserPasswordResetStatus.NotExists;
            }

            if (userPasswordReset.ExpiryDate > DateTime.Now)
            {
                return UserPasswordResetStatus.Expired;
            }
            else
            {
                return UserPasswordResetStatus.Valid;
            }
        }
    }
}
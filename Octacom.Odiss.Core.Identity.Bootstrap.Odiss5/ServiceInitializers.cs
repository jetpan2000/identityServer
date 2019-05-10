using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;
using Octacom.Odiss.Core.Identity.Entities;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.Identity.Validators;
using Octacom.Odiss.Core.Identity.Managers;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5
{
    public static class ServiceInitializers
    {
        public static void InitializeUserManager(UserManager<OdissUser, Guid> manager, Container container)
        {
            var app = container.GetInstance<IAppBuilder>();
            var cfg = container.GetInstance<IConfigService>();
            var appSettings = cfg.GetApplicationSettings();

            manager.UserValidator = new UserValidator<OdissUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            //Configure validation logic for passwords
            manager.PasswordValidator = new OdissPasswordValidator()
            {
                RequiredLength = appSettings.MinimumPasswordLength,
                RequireNonLetterOrDigit = false,
                RequireDigit = appSettings.ForcePasswordStrength,
                RequireLowercase = appSettings.ForcePasswordStrength,
                RequireUppercase = appSettings.ForcePasswordStrength,
                MaximumPasswordLength = appSettings.MaximumPasswordLength,
                MinimumPasswordStrength = 0 // Not yet implemented
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(appSettings.LockUserForMinutes);
            manager.MaxFailedAccessAttemptsBeforeLockout = appSettings.LockUserAfterAttempts;
            manager.EmailService = container.GetInstance<Identity.Contracts.IEmailService>();
            manager.SmsService = container.GetInstance<Identity.Contracts.ISmsService>();

            var dataProtectionProvider =
                 app.GetDataProtectionProvider();

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                 new DataProtectorTokenProvider<OdissUser, Guid>(
                  dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        public static void InitializeSignInManager(SignInManager<OdissUser, Guid> manager)
        {
            // TODO
        }

        public static void InitializeUserSessionManager(UserSessionManager<Guid, Guid> manager, Container container)
        {
            var cfg = container.GetInstance<IConfigService>();
            var appSettings = cfg.GetApplicationSettings();

            manager.MaximumSessionTimeout = TimeSpan.FromHours(appSettings.MaximumSessionTimeout);
        }
    }
}

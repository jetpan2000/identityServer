using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SimpleInjector;
using Octacom.Odiss.Core.Identity.Constants;
using Octacom.Odiss.Core.Identity.Dapper;
using Octacom.Odiss.Core.Identity.Entities;
using Octacom.Odiss.Core.Identity.Managers;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Implementations;
using Octacom.Odiss.Core.Identity.Stores;
using Octacom.Odiss.Core.Identity.Middleware;
using IOdissLogger = Octacom.Odiss.Core.Contracts.Infrastructure.ILogger;
using Octacom.Odiss.Core.Identity.Utilities;
using Microsoft.Owin.Logging;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5
{
    public static class Startup
    {
        public static void BootstrapIdentity(this IAppBuilder app, Container container)
        {
            RegisterServices(app, container);
            SetupOwin(app, container);
        }

        public static void RegisterServices(this IAppBuilder app, Container container)
        {
            RegisterServices<OdissUserStore, OdissUserManager, OdissSignInManager>(
                app,
                container,
                userManagerInitializer: manager =>
                {
                    ServiceInitializers.InitializeUserManager(manager, container);
                },
                signInManagerInitializer: manager => ServiceInitializers.InitializeSignInManager(manager),
                userSessionManagerInitializer: manager => ServiceInitializers.InitializeUserSessionManager(manager, container)
            );
        }

        public static void RegisterServices<TUserStore, TUserManager, TSignInManager>(this IAppBuilder app, Container container, Action<UserManager<OdissUser, Guid>> userManagerInitializer = null, Action<SignInManager<OdissUser, Guid>> signInManagerInitializer = null, Action<UserSessionManager<Guid, Guid>> userSessionManagerInitializer = null)
            where TUserStore : class, IUserStore<OdissUser, Guid>
            where TUserManager : UserManager<OdissUser, Guid>
            where TSignInManager : SignInManager<OdissUser, Guid>
        {
            container.Register<IAppBuilder>(() => app, Lifestyle.Scoped);
            container.Register<IUserStore<OdissUser, Guid>, TUserStore>(Lifestyle.Scoped);
            container.Register<IOdissUserStore, OdissUserStore>(Lifestyle.Scoped);
            container.Register<IRoleStore<OdissRole, byte>, OdissRoleStore>(Lifestyle.Scoped);
            container.Register<IUserPasswordResetStore, OdissUserPasswordResetStore>(Lifestyle.Scoped);
            container.Register<IUserSessionStore<Guid, Guid>, OdissUserSessionStore>(Lifestyle.Scoped);
            container.Register<UserManager<OdissUser, Guid>, TUserManager>(Lifestyle.Scoped);
            container.Register<SignInManager<OdissUser, Guid>, TSignInManager>(Lifestyle.Scoped);
            container.Register<RoleManager<OdissRole, byte>, OdissRoleManager>(Lifestyle.Scoped);
            container.Register<UserSessionManager<Guid, Guid>>(Lifestyle.Scoped);
            container.Register<IAuthenticationAdapter, AuthenticationAdapter>(Lifestyle.Scoped);
            container.Register<IOdissUserRepository, OdissUserRepository>(Lifestyle.Scoped);
            container.Register<IEmailService, EmailService>(Lifestyle.Scoped);
            container.Register<ISmsService, SmsService>(Lifestyle.Scoped);
            container.Register<ILogger, OdissOwinLogger>(Lifestyle.Scoped);
            container.Register<IAuthenticationManager>(() => container.IsVerifying
                ? new OwinContext(new Dictionary<string, object>()).Authentication
                : HttpContext.Current.GetOwinContext().Authentication, Lifestyle.Scoped);

            if (userManagerInitializer != null)
            {
                container.RegisterInitializer<UserManager<OdissUser, Guid>>(userManagerInitializer);
            }

            if (signInManagerInitializer != null)
            {
                container.RegisterInitializer<SignInManager<OdissUser, Guid>>(signInManagerInitializer);
            }

            if (userSessionManagerInitializer != null)
            {
                container.RegisterInitializer<UserSessionManager<Guid, Guid>>(userSessionManagerInitializer);
            }

            container.Register<Action<UserSessionManager<Guid, Guid>>>(() => userSessionManagerInitializer ?? new Action<UserSessionManager<Guid, Guid>>((manager) => { }), Lifestyle.Scoped);

            container.RegisterInitializer<IUserPasswordResetStore>(store =>
            {
                // TODO - Retrieve this value from appSettings
                store.Initialize(TimeSpan.FromMinutes(30));
            });

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
        }

        public static void SetupOwin(this IAppBuilder app, Container container)
        {
            app.CreatePerOwinContext(() => container.GetInstance<OdissUserManager>());
            app.CreatePerOwinContext(() => container.GetInstance<OdissRoleManager>());
            app.CreatePerOwinContext(() => new OdissOwinLogger(container.GetInstance<IOdissLogger>()));
            app.CreatePerOwinContext(() =>
            {
                var manager = new UserSessionManager<Guid, Guid>(
                    container.GetInstance<IUserSessionStore<Guid, Guid>>(), 
                    container.GetInstance<IAuthenticationManager>(),
                    container.GetInstance<IOdissLogger>()
                    );

                var initializer = container.GetInstance<Action<UserSessionManager<Guid, Guid>>>();
                initializer(manager);

                return manager;
            });

        // Enable the application to use a cookie to store information for the signed in user
        // and to use a cookie to temporarily store information about a user logging in with a third party login provider
        // Configure the sign in cookie
        app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<OdissUserManager, OdissUser, Guid>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                        getUserIdCallback: (identity) =>
                        {
                            var value = identity.FindFirstValue(OdissClaims.Id);

                            if (value == null)
                            {
                                return Guid.Empty;
                            }

                            return Guid.Parse(value);
                        })
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var requestContext = new RequestContext(httpContext, new RouteData());
            var urlHelper = new System.Web.Mvc.UrlHelper(requestContext);

            var redirectUrl = urlHelper.Action("Index", "Home");

            app.Use(typeof(SessionMiddleware), redirectUrl, container.GetInstance<IOdissLogger>());
        }
    }
}

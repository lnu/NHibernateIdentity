using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.AspNet.Identity;
using System;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // session used by asp.net identity
            container.RegisterType<ISession>(new PerRequestLifetimeManager(), new InjectionFactory(c =>
            {
                return NHibernateSessionFactory.SessionFactory.OpenSession();
            }));
            container.RegisterType<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>(new PerRequestLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ISession>()));
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new PerRequestLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ISession>()));
            container.RegisterType<IAuthenticationManager>(new PerRequestLifetimeManager(), new InjectionFactory(c =>
            {
                return System.Web.HttpContext.Current.GetOwinContext().Authentication;
            }));
            container.RegisterType<UserManager<ApplicationUser>>(new PerRequestLifetimeManager());
            //container.RegisterType<EcdtRoleManager>(new PerRequestLifetimeManager());
            //container.RegisterType<EcdtSignInManager>(new PerRequestLifetimeManager());
        }
    }
}

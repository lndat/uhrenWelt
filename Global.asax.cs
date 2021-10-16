using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace uhrenWelt
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(Object o, EventArgs eventArgs)
        {
            // cookie auslenen
            var cookie = Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value)) return;

            var encryptedTicket = cookie.Value; // cookie value (mein ticket) rausholen
            var ticket = FormsAuthentication.Decrypt(encryptedTicket); // ticket entschlüsseln
            IIdentity userIdentity = new GenericIdentity(ticket.Name); // identity erstellen
            HttpContext.Current.User = new GenericPrincipal(userIdentity, null); // current user identity zuweisen
        }
    }
}
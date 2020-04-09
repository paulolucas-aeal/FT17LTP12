using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WebAppPrepFT17DatabaseFirst
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }

        protected void Session_Start()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                HttpContext.Current.Response.End();
            }
        }

    }
}

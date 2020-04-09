using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebAppPrepFT17DatabaseFirst
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            ScriptBundle scriptBundle = new ScriptBundle("~/bundles/js");

            scriptBundle.Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery-3.4.1.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/modernizr-2.8.3.js",
                "~/Scripts/alertify.js"
                );

            bundles.Add(scriptBundle);

            StyleBundle styleBundle = new StyleBundle("~/bundles/css");

            styleBundle.Include(
                "~/Content/bootstrap.css",
                "~/Content/Site.css",
                "~/Content/alertifyjs/alertify.css",
                "~/Content/alertifyjs/themes/default.css"
                );

            bundles.Add(styleBundle);

            BundleTable.EnableOptimizations = true;
        }
    }
}


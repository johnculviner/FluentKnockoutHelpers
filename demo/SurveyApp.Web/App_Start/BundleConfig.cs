using System;
using System.Web;
using System.Web.Optimization;

namespace SurveyApp.Web
{
    public class BundleConfig
    {
        public static class Scripts
        {
            public const string All = "~/Scripts";
        }

        public static class Css
        {
            public const string All = "~/Content/";
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Ignore("_references.js");
            
            //Scripts
            bundles.Add(new ScriptBundle(Scripts.All).Include("~/Scripts/*.js"));
            
            //CSS
            bundles.Add(new StyleBundle(Css.All)
                .Include("~/Content/base/*.css")
                .Include("~/Content/app/*.css"));
        }
    }
}
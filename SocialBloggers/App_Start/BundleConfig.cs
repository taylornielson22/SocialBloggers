using System.Web;
using System.Web.Optimization;

namespace SocialBloggers
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                        "~/Scripts/Popup.js",
                        "~/Scripts/Comment.js",
                        "~/Scripts/BlogPosts.js",
                        "~/Scripts/Accounts.js",
                      "~/Scripts/Popup.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Buttons.css",
                      "~/Content/Popup.css",
                      "~/Content/site.css"));
        }
    }
}

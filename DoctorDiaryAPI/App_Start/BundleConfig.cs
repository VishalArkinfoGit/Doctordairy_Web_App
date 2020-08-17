using System.Web;
using System.Web.Optimization;

namespace DoctorDiaryAPI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                            "~/Content/assets/js/jquery.min.js",
                            "~/Content/assets/js/popper.min.js",
                            "~/Content/assets/js/bootstrap.min.js",
                            "~/Content/assets/js/slick.js",
                            "~/Content/assets/plugins/select2/js/select2.min.js",
                            "~/Content/assets/js/moment.min.js",
                            "~/Content/assets/js/bootstrap-datetimepicker.min.js",
                            "~/Content/assets/plugins/daterangepicker/daterangepicker.js",
                            "~/Content/assets/js/script.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/assets/css/bootstrap.min.css",
                      "~/Content/assets/plugins/fontawesome/css/fontawesome.min.css",
                      "~/Content/assets/plugins/fontawesome/css/all.min.css",
                      "~/Content/assets/css/bootstrap-datetimepicker.min.css",
                      "~/Content/assets/plugins/select2/css/select2.min.css",
                      "~/Content/assets/css/style.css",
                      "~/Content/assets/plugins/daterangepicker/daterangepicker.css",
                      "~/Content/Site.css"));
        }
    }
}

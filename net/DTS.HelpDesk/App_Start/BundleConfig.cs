using System.Web;
using System.Web.Optimization;

namespace DTS.HelpDesk
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Public/Scripts/vendor/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Public/Scripts/vendor/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Public/Scripts/vendor/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Public/Scripts/vendor/bootstrap.js",
                "~/Public/Scripts/vendor/respond.js"
                ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Public/Styles/bootstrap.css",
                      "~/Public/Styles/site.css"));

            #region Utilities
            bundles.Add(new ScriptBundle("~/scripts/utilities").Include(
                "~/Public/Scripts/dts/dts.namespace.js",
                "~/Public/Scripts/dts/dts.utils.js",
                "~/Public/Scripts/vendor/icanhaz/icanhaz.js"
            ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/Public/Scripts/Vendor/dataTables-1.10.4/media/js/jquery.dataTables.js",
                "~/Public/Scripts/Vendor/dataTablesBootstrap/dataTables.bootstrap.js"
            ));

            bundles.Add(new StyleBundle("~/styles/datatables").Include(
                "~/Public/Scripts/Vendor/dataTables-1.10.4/media/css/jquery.dataTables.css",
                "~/Public/Scripts/Vendor/dataTablesBootstrap/dataTables.bootstrap.css"
            ));


            #endregion


            #region Admin Area

            bundles.Add(new StyleBundle("~/admin/styles").Include(
                "~/Public/Styles/bootstrap.css",
                "~/Public/Admin/css/plugins/metisMenu/metisMenu.css",
                "~/Public/Admin/css/sb-admin-2.css",
                "~/Public/Admin/font-awesome-4.2.0/css/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/admin/scripts").Include(
                "~/Public/Admin/js/bootstrap.js",
                "~/Public/Admin/js/respond.js",
                "~/Public/Admin/js/plugins/metisMenu/metisMenu.js",
                "~/Public/Admin/js/sb-admin-2.js",
                "~/Public/Scripts/Vendor/bootbox/bootbox.js"
            ));

            bundles.Add(new ScriptBundle("~/admin/modernizr").Include(
            "~/Public/Admin/js/modernizr-*"));



            #endregion


            #region editor


            bundles.Add(new StyleBundle("~/editor/styles").Include(
                "~/Public/Scripts/vendor/summernote/summernote.css"
            ));

            bundles.Add(new ScriptBundle("~/editor/scripts").Include(
                "~/Public/Scripts/vendor/summernote/summernote.js"
            ));



            #endregion


            #region Account

            bundles.Add(new ScriptBundle("~/scripts/accounts").Include(
                "~/Public/Scripts/apps/accounts/accounts.js"
            ));

            bundles.Add(new ScriptBundle("~/scripts/account").Include(
                "~/Public/Scripts/apps/accounts/account.js"
            ));

            #endregion

            #region FAQ

            bundles.Add(new ScriptBundle("~/scripts/faqs").Include(
                "~/Public/Scripts/vendor/moment/moment-2.8.4.js",
                "~/Public/Scripts/apps/faqs/faqs.js"
            ));

            bundles.Add(new ScriptBundle("~/scripts/faq").Include(
                "~/Public/Scripts/vendor/summernote/summernote.js",
                "~/Public/Scripts/apps/faqs/faq.js"
            ));
            #endregion

            #region Article

            bundles.Add(new ScriptBundle("~/scripts/articles").Include(
                "~/Public/Scripts/vendor/moment/moment-2.8.4.js",
                "~/Public/Scripts/vendor/moment/moment-2.8.4.js",
                "~/Public/Scripts/apps/articles/articles.js"
            ));

            bundles.Add(new ScriptBundle("~/scripts/article").Include(
                "~/Public/Scripts/vendor/summernote/summernote.js",
                "~/Public/Scripts/vendor/moment/moment-2.8.4.js",
                "~/Public/Scripts/vendor/boostrap-datetimepicker/js/bootstrap-datetimepicker.js",
                "~/Public/Scripts/apps/articles/article.js"
            ));


            bundles.Add(new StyleBundle("~/styles/article").Include(
                "~/Public/Scripts/vendor/boostrap-datetimepicker/css/bootstrap-datetimepicker.css"
            ));
            #endregion




            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}

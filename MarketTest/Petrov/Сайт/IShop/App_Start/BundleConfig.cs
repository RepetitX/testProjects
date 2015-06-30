using System.Web.Optimization;

namespace IShop
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/globalize.js",
                "~/Scripts/globalize.culture.ru-RU.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/noty").Include(
                "~/Scripts/noty/jquery.noty.js",
                "~/Scripts/noty/themes/default.js",
                "~/Scripts/noty/layouts/top.js",
                "~/Scripts/noty/layouts/center.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/jquery.maskedinput.js",
                "~/Scripts/admin/jquery.form.js",
                "~/Scripts/admin/jquery.tablednd.js",
                "~/Scripts/admin/Common.common.js",
                "~/Scripts/admin/Common.grid.js"));

            bundles.Add(new ScriptBundle("~/bundles/cufon").Include(
                "~/Scripts/cufon-yui.js",
                "~/Scripts/front/CorporateACon_400.font.js",
                "~/Scripts/front/fonts.js"));

            #endregion

            #region Styles

            bundles.Add(new StyleBundle("~/Styles/site").Include(
                "~/Styles/bootstrap.css",
                "~/Styles/bootstrap-additional.css",
                "~/Styles/fonts.css",
                "~/Styles/site.css"));

            bundles.Add(new StyleBundle("~/Styles/admin").Include(
                "~/Styles/bootstrap.css",
                "~/Styles/bootstrap-additional.css",
                "~/Styles/bootstrap-multiselect.css",
                "~/Styles/bootstrap-datetimepicker.css",
                "~/Styles/admin.css"));

            bundles.Add(new StyleBundle("~/Styles/signin").Include(
                "~/Styles/bootstrap.css",
                "~/Styles/bootstrap-additional.css",
                "~/Styles/signin.css"));

            #endregion

            BundleTable.EnableOptimizations = false;
        }
    }
}

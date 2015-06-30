using System.Web.Optimization;

namespace AdminExample
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-material").Include(
                    "~/Scripts/angular/angular.min.js",
                    "~/Scripts/angular/i18n/angular-locale_ru-ru.js",
                    "~/Scripts/angular/angular-animate.min.js",
                    "~/Scripts/angular/angular-route.min.js",
                    "~/Scripts/angular/angular-aria.min.js",
                    "~/Scripts/angularMaterial/angular-material.min.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Scripts/angularMaterial/angular-material.min.css")
                      );


            bundles.Add(new StyleBundle("~/bundles/admin").Include(

                //контроллеры
                "~/Scripts/controllers/AppCtrl.js",
                "~/Scripts/controllers/OrdersCtrl.js",
                "~/Scripts/controllers/OrderCtrl.js",
                "~/Scripts/controllers/AddProductDialogCtrl.js",

                //сервисы
                "~/Scripts/services/OrderRepository.js",
                "~/Scripts/services/ManagerRepository.js",
                "~/Scripts/services/ProductRepository.js",

                //модели
                "~/Scripts/models/Address.js",
                "~/Scripts/models/Manager.js",
                "~/Scripts/models/Name.js",
                "~/Scripts/models/Order.js",
                "~/Scripts/models/OrderItem.js",
                "~/Scripts/models/Product.js",

                //приложение
                "~/Scripts/Application.js")
                );
        }
    }
}

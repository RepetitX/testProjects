var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    angular
        .module("StarterApp", ["ngMaterial", "ngRoute"])
        .controller("AppCtrl", ShopAdmin.AppCtrl)
        .controller("OrdersCtrl", ShopAdmin.OrdersCtrl)
        .controller("OrderCtrl", ShopAdmin.OrdersCtrl)
        .controller("AddProductDialogCtrl", ShopAdmin.AddProductDialogCtrl)
        .service("orderRepository", ShopAdmin.OrderRepository)
        .service("managerRepository", ShopAdmin.ManagerRepository)
        .service("productRepository", ShopAdmin.ProductRepository)
        .config(function ($mdThemingProvider) {
        $mdThemingProvider.theme("default")
            .primaryPalette("blue")
            .accentPalette("orange");
    })
        .config([
        "$routeProvider",
        function ($routeProvider) {
            $routeProvider
                .when("/Orders", {
                templateUrl: "Scripts/controllers/views/OrdersCtrl.html",
                controller: ShopAdmin.OrdersCtrl,
                resolve: {
                    orders: [
                        "orderRepository",
                        function (orderRepository) {
                            return orderRepository.getMany(200, 0);
                        }
                    ]
                }
            })
                .when("/Orders/:id", {
                templateUrl: "Scripts/controllers/views/OrderCtrl.html",
                controller: ShopAdmin.OrderCtrl,
                resolve: {
                    order: [
                        "$route", "orderRepository",
                        function ($route, orderRepository) {
                            var id = parseInt($route.current.params.id);
                            return orderRepository.getOne(id);
                        }
                    ]
                }
            })
                .otherwise({
                redirectTo: "/Orders"
            });
        }
    ])
        .run(["$rootScope", "$window", "$http", function ($rootScope, $window, $http) {
            $rootScope.$on("$routeChangeStart", function (e, curr, prev) {
                //перемотаем на начало страницы
                if (curr.$$route && curr.$$route.resolve) {
                    // Show a loading message until promises are not resolved
                    $rootScope.loadingRoute = true;
                }
            });
            $rootScope.$on("$routeChangeSuccess", function (e, curr, prev) {
                // Hide loading message
                $rootScope.loadingRoute = false;
            });
        }]);
    ;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=Application.js.map
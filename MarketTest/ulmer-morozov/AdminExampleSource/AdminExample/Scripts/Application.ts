module ShopAdmin {
    "use strict";
    angular
        .module("StarterApp", ["ngMaterial", "ngRoute"])

    //контроллеры
        .controller("AppCtrl", AppCtrl)
        .controller("OrdersCtrl", OrdersCtrl)
        .controller("OrderCtrl", OrdersCtrl)
        .controller("AddProductDialogCtrl", AddProductDialogCtrl)

    //сервисы
        .service("orderRepository", OrderRepository)
        .service("managerRepository", ManagerRepository)
        .service("productRepository", ProductRepository)

    //настроим цветовую тему
        .config(
        $mdThemingProvider => {
            $mdThemingProvider.theme("default")
                .primaryPalette("blue")
                .accentPalette("orange");
        })

    //настроим роутинг
        .config([
        "$routeProvider",
        ($routeProvider: ng.route.IRouteProvider) => {
            $routeProvider
                .when("/Orders", {
                templateUrl: "Scripts/controllers/views/OrdersCtrl.html",
                controller: OrdersCtrl,
                resolve: {
                    orders: [
                        "orderRepository",
                        (orderRepository: IOrderRepository) => {
                            return orderRepository.getMany(200, 0);
                        }
                    ]
                }
            })
                .when("/Orders/:id", {
                templateUrl: "Scripts/controllers/views/OrderCtrl.html",
                controller: OrderCtrl,
                resolve: {
                    order: [
                        "$route", "orderRepository",
                        ($route, orderRepository: IOrderRepository) => {
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
    //индикатор загрузки в время переключения
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
    }]);;
}
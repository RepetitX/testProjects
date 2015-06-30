var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var AppCtrl = (function () {
        function AppCtrl($scope, $mdSidenav) {
            this.$scope = $scope;
            this.$mdSidenav = $mdSidenav;
            //оживим навигацию
            $scope.toggleSidenav = function (menuId) {
                $mdSidenav(menuId).toggle();
            };
            console.log("AppCtrl инициализирован");
        }
        //DI
        AppCtrl.$inject = [
            "$scope",
            "$mdSidenav"
        ];
        return AppCtrl;
    })();
    ShopAdmin.AppCtrl = AppCtrl;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=AppCtrl.js.map
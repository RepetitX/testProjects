module ShopAdmin {
    "use strict";

    export class AppCtrl {

        //DI
        static $inject = [
            "$scope",
            "$mdSidenav"
        ];

        constructor(private $scope: IAppScope, private $mdSidenav) {
            //оживим навигацию
            $scope.toggleSidenav = menuId => {
                $mdSidenav(menuId).toggle();
            }
            console.log("AppCtrl инициализирован");
        }
    }
}
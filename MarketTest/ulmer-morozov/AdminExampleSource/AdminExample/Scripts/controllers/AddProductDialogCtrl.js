var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var AddProductDialogCtrl = (function () {
        function AddProductDialogCtrl($scope, productRepository, $mdDialog, forProductId) {
            this.$scope = $scope;
            this.productRepository = productRepository;
            this.$mdDialog = $mdDialog;
            //закрытие диалога
            $scope.close = function () {
                $mdDialog.cancel();
            };
            //добавление товара
            $scope.addProduct = function (product) {
                $mdDialog.hide(product);
            };
            //загрузим товары
            $scope.isLoading = true;
            productRepository
                .getManyForProduct(forProductId)
                .then(function (products) {
                $scope.products = products;
                $scope.isLoading = false;
            });
        }
        //DI
        AddProductDialogCtrl.$inject = [
            "$scope",
            "productRepository",
            "$mdDialog",
            "forProductId"
        ];
        return AddProductDialogCtrl;
    })();
    ShopAdmin.AddProductDialogCtrl = AddProductDialogCtrl;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=AddProductDialogCtrl.js.map
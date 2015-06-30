module ShopAdmin {
    "use strict";

    export class AddProductDialogCtrl {

        //DI
        static $inject = [
            "$scope",
            "productRepository",
            "$mdDialog",
            "forProductId"
        ];



        constructor(private $scope: IAddProductDialogScope, private productRepository: IProductRepository, private $mdDialog: any, forProductId: number) {
            //закрытие диалога
            $scope.close = () => {
                $mdDialog.cancel();
            };

            //добавление товара
            $scope.addProduct = (product: Product) => {
                $mdDialog.hide(product);
            };

            //загрузим товары
            $scope.isLoading = true;
            productRepository
                .getManyForProduct(forProductId)
                .then((products: Product[]) => {
                $scope.products = products;
                $scope.isLoading = false;
            });
        }
    }
}
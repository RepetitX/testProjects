module ShopAdmin {
    "use strict";

    export class ProductRepository implements IProductRepository {
        //DI
        static $inject = [
            "$http"
        ];


        constructor(private $http: ng.IHttpService) {
        }

        getManyForProduct(id: number): ng.IPromise<Product[]> {
            var result = this.$http.get<Product[]>("/api/products", {
                params: {
                    forProductId: id
                }
            })
                .then(
                (response: any): Product[]=> {
                    return response.data;
                });
            return result;
        }

        getMany(): angular.IPromise<Product[]> {
            return this.getManyForProduct(undefined);
        }
    }
}
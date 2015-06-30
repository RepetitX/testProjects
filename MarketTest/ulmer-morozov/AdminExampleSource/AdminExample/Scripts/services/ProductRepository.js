var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var ProductRepository = (function () {
        function ProductRepository($http) {
            this.$http = $http;
        }
        ProductRepository.prototype.getManyForProduct = function (id) {
            var result = this.$http.get("/api/products", {
                params: {
                    forProductId: id
                }
            })
                .then(function (response) {
                return response.data;
            });
            return result;
        };
        ProductRepository.prototype.getMany = function () {
            return this.getManyForProduct(undefined);
        };
        //DI
        ProductRepository.$inject = [
            "$http"
        ];
        return ProductRepository;
    })();
    ShopAdmin.ProductRepository = ProductRepository;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=ProductRepository.js.map
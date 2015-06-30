var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var OrderRepository = (function () {
        function OrderRepository($http) {
            this.$http = $http;
        }
        OrderRepository.prototype.getMany = function (serquenceLength, offset) {
            var result = this.$http.get("/api/orders", {
                params: {
                    pageSize: serquenceLength,
                    offset: offset
                }
            })
                .then(function (response) {
                return response.data;
            });
            return result;
        };
        OrderRepository.prototype.getOne = function (id) {
            var result = this.$http.get("/api/orders/" + id)
                .then(function (response) {
                return response.data;
            });
            return result;
        };
        OrderRepository.prototype.store = function (order) {
            var result = this.$http({
                method: "PUT",
                url: "/api/orders/",
                data: order
            }).then(function (response) {
                return response.data;
            });
            return result;
        };
        OrderRepository.prototype.deleteOne = function (id) {
            var result = this.$http({
                method: "DELETE",
                url: "/api/orders/" + id
            }).then(function (response) {
                return response;
            });
            return result;
        };
        //DI
        OrderRepository.$inject = [
            "$http"
        ];
        return OrderRepository;
    })();
    ShopAdmin.OrderRepository = OrderRepository;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=OrderRepository.js.map
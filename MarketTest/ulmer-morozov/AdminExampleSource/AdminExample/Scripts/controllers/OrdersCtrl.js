var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var OrdersCtrl = (function () {
        function OrdersCtrl($scope, orderRepository, $location, orders) {
            this.$scope = $scope;
            this.orderRepository = orderRepository;
            this.$location = $location;
            this.pageSize = 200;
            $scope.orders = orders;
            //редактирование заказа
            $scope.goto = function (order) {
                $location.url("/Orders/" + order.id);
            };
            //orderRepository
            //    .getMany(this.pageSize, 0)
            //    .then(
            //    (data: Order[]) => {
            //        $scope.orders = data;
            //    });
        }
        //DI
        OrdersCtrl.$inject = [
            "$scope",
            "orderRepository",
            "$location",
            "orders"
        ];
        return OrdersCtrl;
    })();
    ShopAdmin.OrdersCtrl = OrdersCtrl;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=OrdersCtrl.js.map
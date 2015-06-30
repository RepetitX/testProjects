module ShopAdmin {
    "use strict";

    export class OrdersCtrl {

        //DI
        static $inject = [
            "$scope",
            "orderRepository",
            "$location",
            "orders"
        ];

        private pageSize = 200;

        constructor(
            private $scope: IOrdersScope,
            private orderRepository: IOrderRepository,
            private $location: ng.ILocationService,
            orders: Order[]) {
            $scope.orders = orders;

            //редактирование заказа
            $scope.goto = (order: Order) => {
                $location.url(`/Orders/${order.id}`);
            };
            //orderRepository
            //    .getMany(this.pageSize, 0)
            //    .then(
            //    (data: Order[]) => {
            //        $scope.orders = data;
            //    });
        }

    }
}
module ShopAdmin {
    "use strict";

    export class OrderCtrl {

        //DI
        static $inject = [
            "$scope",
            "managerRepository",
            "orderRepository",
            "$mdDialog",
            "$location",
            "order"
        ];

        private synchroDelay = 1000;
        private timerId: number;

        constructor(
            private $scope: IOrderScope,
            private managerRepository: IManagerRepository,
            private orderRepository: IOrderRepository,
            private $mdDialog: any,
            private $location: ng.ILocationService,
            private order: Order
            ) {
            $scope.order = order;
            //
            if (order == null) {
                $scope.order = this.order = order = new Order();
            }

            //поиск по менеджерам
            $scope.search = (text: string) => managerRepository.getMany(text);

            //функция получающая текст из записи менеджера
            $scope.getText = (item: Manager) => `${item.name.last} ${item.name.first} ${item.name.middle}`;

            //открытие диалога с товарами
            $scope.showAddItemDialog = (event) => {
                $scope.showAddAccessoriseDialog(undefined, event);
            };

            //открытие диалога с товарами (аксессуарами)
            $scope.showAddAccessoriseDialog = (parentProductId: number, event) => {
                $mdDialog.show({
                    controller: "AddProductDialogCtrl",
                    templateUrl: "/Scripts/controllers/views/AddProductDialogCtrl.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    locals: {
                        forProductId: parentProductId
                    }
                })
                    .then(
                    (product: Product) => {
                        var productAllreadyAdded = false;
                        angular.forEach(order.items, (item: OrderItem) => {
                            if (item.product.id === product.id) {
                                item.count++;
                                productAllreadyAdded = true;
                                return;
                            }
                        });
                        if (productAllreadyAdded)
                            return;
                        //добавим новый заказ
                        var newOrderItem = new OrderItem();
                        newOrderItem.product = product;
                        newOrderItem.count = 1;
                        newOrderItem.orderId = order.id;
                        order.items.push(newOrderItem);
                    });
            };

            //удаление товара
            $scope.removeItem = (orderItem: OrderItem, ev: Event) => {
                var confirm = $mdDialog.confirm()
                    .parent(angular.element(document.body))
                    .title(`Удалить ${orderItem.product.title}?`)
                    .content(`Вы уверены, что хотите удалить из заказа ${orderItem.product.title}?`)
                    .ariaLabel("Удаление")
                    .ok("Да, удалить")
                    .cancel("Нет")
                    .targetEvent(ev);
                $mdDialog.show(confirm).then(() => {
                    order.items.splice(order.items.indexOf(orderItem), 1);
                });
            };

            //будем вести пересчёт суммы
            $scope.$watch("order.items", () => {
                var summary = 0;
                angular.forEach(order.items, (item: OrderItem) => {
                    summary += item.product.price * item.count;
                });
                $scope.summary = summary;
            }, true);

            $scope.synchroTime = () => {
                $scope.isLoading = true;
                orderRepository.store(order).then(() => {
                    $scope.isLoading = false;
                });
            }

            //сохранение нового заказа
            $scope.createOrder = () => {
                if (!$scope.orderForm.$valid) {
                    return;
                }
                $scope.isLoading = true;
                orderRepository.store(order)
                    .then((order: Order) => {
                    //перейдем в режим редактирования сохраненного заказа
                    $location.url(`/Orders/${order.id}`);
                });
            }

            //удаление заказа
            $scope.deleteOrder = (ev:Event) => {
                var confirm = $mdDialog.confirm()
                    .parent(angular.element(document.body))
                    .title(`Удалить весь заказ №${order.id}?`)
                    .content(`Вы уверены, что хотите удалить заказ №${order.id}? Данное действие нельзя отменить`)
                    .ariaLabel("Удаление")
                    .ok("Удалить заказ")
                    .cancel("Сохранить заказ")
                    .targetEvent(ev);
                $mdDialog.show(confirm).then(() => {
                    $scope.isLoading = true;
                    orderRepository.deleteOne(order.id)
                        .then(() => {
                        //перейдем ко всем заказам
                        $location.url("/Orders/");
                        $scope.isLoading = false;
                    });
                });

            }

            //только если заказ новый
            if (order.id != undefined) {
                $scope.$watch("order", (newValue: Order, oldValue: Order) => {
                    if (newValue === oldValue) {
                        return;
                    }
                    clearTimeout(this.timerId);
                    this.timerId = setTimeout($scope.synchroTime, this.synchroDelay);
                }, true);
            }

        }


    }
}
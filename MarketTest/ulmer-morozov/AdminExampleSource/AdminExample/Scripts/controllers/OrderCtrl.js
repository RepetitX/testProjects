var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var OrderCtrl = (function () {
        function OrderCtrl($scope, managerRepository, orderRepository, $mdDialog, $location, order) {
            var _this = this;
            this.$scope = $scope;
            this.managerRepository = managerRepository;
            this.orderRepository = orderRepository;
            this.$mdDialog = $mdDialog;
            this.$location = $location;
            this.order = order;
            this.synchroDelay = 1000;
            $scope.order = order;
            //
            if (order == null) {
                $scope.order = this.order = order = new ShopAdmin.Order();
            }
            //поиск по менеджерам
            $scope.search = function (text) { return managerRepository.getMany(text); };
            //функция получающая текст из записи менеджера
            $scope.getText = function (item) { return (item.name.last + " " + item.name.first + " " + item.name.middle); };
            //открытие диалога с товарами
            $scope.showAddItemDialog = function (event) {
                $scope.showAddAccessoriseDialog(undefined, event);
            };
            //открытие диалога с товарами (аксессуарами)
            $scope.showAddAccessoriseDialog = function (parentProductId, event) {
                $mdDialog.show({
                    controller: "AddProductDialogCtrl",
                    templateUrl: "/Scripts/controllers/views/AddProductDialogCtrl.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    locals: {
                        forProductId: parentProductId
                    }
                })
                    .then(function (product) {
                    var productAllreadyAdded = false;
                    angular.forEach(order.items, function (item) {
                        if (item.product.id === product.id) {
                            item.count++;
                            productAllreadyAdded = true;
                            return;
                        }
                    });
                    if (productAllreadyAdded)
                        return;
                    //добавим новый заказ
                    var newOrderItem = new ShopAdmin.OrderItem();
                    newOrderItem.product = product;
                    newOrderItem.count = 1;
                    newOrderItem.orderId = order.id;
                    order.items.push(newOrderItem);
                });
            };
            //удаление товара
            $scope.removeItem = function (orderItem, ev) {
                var confirm = $mdDialog.confirm()
                    .parent(angular.element(document.body))
                    .title("\u0423\u0434\u0430\u043B\u0438\u0442\u044C " + orderItem.product.title + "?")
                    .content("\u0412\u044B \u0443\u0432\u0435\u0440\u0435\u043D\u044B, \u0447\u0442\u043E \u0445\u043E\u0442\u0438\u0442\u0435 \u0443\u0434\u0430\u043B\u0438\u0442\u044C \u0438\u0437 \u0437\u0430\u043A\u0430\u0437\u0430 " + orderItem.product.title + "?")
                    .ariaLabel("Удаление")
                    .ok("Да, удалить")
                    .cancel("Нет")
                    .targetEvent(ev);
                $mdDialog.show(confirm).then(function () {
                    order.items.splice(order.items.indexOf(orderItem), 1);
                });
            };
            //будем вести пересчёт суммы
            $scope.$watch("order.items", function () {
                var summary = 0;
                angular.forEach(order.items, function (item) {
                    summary += item.product.price * item.count;
                });
                $scope.summary = summary;
            }, true);
            $scope.synchroTime = function () {
                $scope.isLoading = true;
                orderRepository.store(order).then(function () {
                    $scope.isLoading = false;
                });
            };
            //сохранение нового заказа
            $scope.createOrder = function () {
                if (!$scope.orderForm.$valid) {
                    return;
                }
                $scope.isLoading = true;
                orderRepository.store(order)
                    .then(function (order) {
                    //перейдем в режим редактирования сохраненного заказа
                    $location.url("/Orders/" + order.id);
                });
            };
            //удаление заказа
            $scope.deleteOrder = function (ev) {
                var confirm = $mdDialog.confirm()
                    .parent(angular.element(document.body))
                    .title("\u0423\u0434\u0430\u043B\u0438\u0442\u044C \u0432\u0435\u0441\u044C \u0437\u0430\u043A\u0430\u0437 \u2116" + order.id + "?")
                    .content("\u0412\u044B \u0443\u0432\u0435\u0440\u0435\u043D\u044B, \u0447\u0442\u043E \u0445\u043E\u0442\u0438\u0442\u0435 \u0443\u0434\u0430\u043B\u0438\u0442\u044C \u0437\u0430\u043A\u0430\u0437 \u2116" + order.id + "? \u0414\u0430\u043D\u043D\u043E\u0435 \u0434\u0435\u0439\u0441\u0442\u0432\u0438\u0435 \u043D\u0435\u043B\u044C\u0437\u044F \u043E\u0442\u043C\u0435\u043D\u0438\u0442\u044C")
                    .ariaLabel("Удаление")
                    .ok("Удалить заказ")
                    .cancel("Сохранить заказ")
                    .targetEvent(ev);
                $mdDialog.show(confirm).then(function () {
                    $scope.isLoading = true;
                    orderRepository.deleteOne(order.id)
                        .then(function () {
                        //перейдем ко всем заказам
                        $location.url("/Orders/");
                        $scope.isLoading = false;
                    });
                });
            };
            //только если заказ новый
            if (order.id != undefined) {
                $scope.$watch("order", function (newValue, oldValue) {
                    if (newValue === oldValue) {
                        return;
                    }
                    clearTimeout(_this.timerId);
                    _this.timerId = setTimeout($scope.synchroTime, _this.synchroDelay);
                }, true);
            }
        }
        //DI
        OrderCtrl.$inject = [
            "$scope",
            "managerRepository",
            "orderRepository",
            "$mdDialog",
            "$location",
            "order"
        ];
        return OrderCtrl;
    })();
    ShopAdmin.OrderCtrl = OrderCtrl;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=OrderCtrl.js.map
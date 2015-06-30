var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var Order = (function () {
        function Order() {
            this.items = new Array();
        }
        return Order;
    })();
    ShopAdmin.Order = Order;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=Order.js.map
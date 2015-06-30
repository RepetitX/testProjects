module ShopAdmin {
    "use strict";

    export class OrderItem {
        product: Product;
        orderId: number;
        productId: number;
        count: number;
    }
}
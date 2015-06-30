module ShopAdmin {
    export interface IOrdersScope extends ng.IScope {
        orders: Order[];
        goto(order: Order):void;
    }
}
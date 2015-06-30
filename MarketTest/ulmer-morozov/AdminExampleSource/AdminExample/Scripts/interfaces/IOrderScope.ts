module ShopAdmin {
    export interface IOrderScope extends ng.IScope {
        order: Order;
        searchText: string;
        summary: number;
        search(text: string): ng.IPromise<Manager[]>;
        getText(manager: Manager): string;
        showAddItemDialog(event: Event): void;
        removeItem(item: OrderItem, ev: Event): void;
        showAddAccessoriseDialog(parentProductId: number, event: any): void;
        synchroTime(): void;
        createOrder(): void;
        deleteOrder(ev: Event): void;
        isLoading: boolean;
        orderForm: ng.IFormController;
    }
}
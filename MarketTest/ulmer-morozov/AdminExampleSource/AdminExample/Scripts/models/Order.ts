module ShopAdmin {
    "use strict";

    export class Order {
        id: number;
        clientName: Name;
        creationDateTime: Date;
        address: Address;
        contacts: string;
        note: string;
        manager: Manager;
        items: OrderItem[];

        constructor() {
            this.items = new Array<OrderItem>();
        }
    }
}
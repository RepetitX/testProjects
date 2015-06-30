module ShopAdmin {
    "use strict";

    export class Product {
        id: number;
        title: string;
        description: string;
        image: string;
        price: number;
        haveAccessorises: boolean;
        type:number;
        accessorises:Product[];
    }
}
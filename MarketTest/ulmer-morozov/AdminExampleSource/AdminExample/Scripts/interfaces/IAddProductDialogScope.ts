module ShopAdmin {
    export interface IAddProductDialogScope extends ng.IScope {
        products: Product[];
        addProduct(product: Product): void;
        close():void;
        isLoading:boolean;
    }
}
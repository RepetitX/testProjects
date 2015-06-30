module ShopAdmin {
    export interface IProductRepository {
        getMany(): ng.IPromise<Product[]>;
        getManyForProduct(id: number): ng.IPromise<Product[]>;
    }
}
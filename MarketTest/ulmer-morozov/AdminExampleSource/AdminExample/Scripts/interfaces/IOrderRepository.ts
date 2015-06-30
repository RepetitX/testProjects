module ShopAdmin {
    export interface IOrderRepository {
        getMany(serquenceLength: number, offset: number): ng.IPromise<Order[]>;
        getOne(id: number): ng.IPromise<Order>;
        store(order: Order): ng.IPromise<Order>;
        deleteOne(id: number): ng.IPromise<any>;
    }
}
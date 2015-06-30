module ShopAdmin {
    export interface IManagerRepository {
        getMany(query: string): ng.IPromise<Manager[]>;
    }
}
module ShopAdmin {
    "use strict";

    export class OrderRepository implements IOrderRepository {
        //DI
        static $inject = [
            "$http"
        ];

        constructor(private $http: ng.IHttpService) {
        }

        getMany(serquenceLength: number, offset: number): ng.IPromise<Order[]> {
            var result = this.$http.get<Order[]>("/api/orders",
                {
                    params: {
                        pageSize: serquenceLength,
                        offset: offset
                    }
                })
                .then(
                (response: any): Order[]=> {
                    return response.data;
                });
            return result;
        }

        getOne(id: number): angular.IPromise<Order> {
            var result = this.$http.get<Order>(`/api/orders/${id}`)
                .then(
                (response: any): Order=> {
                    return response.data;
                });
            return result;
        }

        store(order: Order): angular.IPromise<Order> {
            var result = this.$http({
                method: "PUT",
                url: "/api/orders/",
                data: order
            }).then(
                (response: any): Order => {
                    return response.data;
                });
            return result;
        }

        deleteOne(id: number): ng.IPromise<any> {
            var result = this.$http({
                method: "DELETE",
                url: `/api/orders/${id}`
            }).then(
                (response: any): any => {
                    return response;
                });
            return result;
        }

    }
}
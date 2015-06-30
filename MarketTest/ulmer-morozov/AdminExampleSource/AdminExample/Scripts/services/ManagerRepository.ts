module ShopAdmin {
    "use strict";

    export class ManagerRepository implements IManagerRepository {
        //DI
        static $inject = [
            "$http"
        ];

        constructor(private $http: ng.IHttpService) {
        }

        getMany(query: string): ng.IPromise<Manager[]> {
            var result = this.$http
                .get<Manager[]>("/api/managers",
                {
                    params:
                    {
                        search: query
                    }
                })
                .then(
                (response: any): Manager[]=> {
                    return response.data;
                });
            return result;
        }
    }
}
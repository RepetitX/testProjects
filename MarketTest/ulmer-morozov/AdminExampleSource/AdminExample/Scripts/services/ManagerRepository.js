var ShopAdmin;
(function (ShopAdmin) {
    "use strict";
    var ManagerRepository = (function () {
        function ManagerRepository($http) {
            this.$http = $http;
        }
        ManagerRepository.prototype.getMany = function (query) {
            var result = this.$http
                .get("/api/managers", {
                params: {
                    search: query
                }
            })
                .then(function (response) {
                return response.data;
            });
            return result;
        };
        //DI
        ManagerRepository.$inject = [
            "$http"
        ];
        return ManagerRepository;
    })();
    ShopAdmin.ManagerRepository = ManagerRepository;
})(ShopAdmin || (ShopAdmin = {}));
//# sourceMappingURL=ManagerRepository.js.map
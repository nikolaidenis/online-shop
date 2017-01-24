var srv = angular.module('ProductService', ["ShopApp.config"]);

srv.factory('ProductApi', ['$http', 'AppVariables', function ($http, AppVariables) {
    var ProductApi = {};
    ProductApi.getProducts = function() {
        return $http.get(AppVariables.base_url + '/product');
    };
    return ProductApi;
}]);
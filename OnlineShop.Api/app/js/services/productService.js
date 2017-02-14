var srv = angular.module('ShopApp.Main', ["ShopApp.Constants"]);

srv.factory('ProductApi', ['$http', 'AppVariables', function ($http, AppVariables) {
    var ProductApi = {};
    ProductApi.getProducts = function() {
        return $http.get(AppVariables.base_url + '/product');
    };
    return ProductApi;
}]);
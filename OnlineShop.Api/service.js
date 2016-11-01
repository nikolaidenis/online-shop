var srv = angular.module('ProductService', []);

srv.factory('ProductApi', function($http) {
    var url = "http://localhost:3315/api";
    var ProductApi = {};
    ProductApi.getProducts = function() {
        return $http.get(url + '/product');
    }
    return ProductApi;
});
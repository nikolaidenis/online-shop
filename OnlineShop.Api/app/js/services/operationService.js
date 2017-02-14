var srv = angular.module('ShopApp.Main', ["ShopApp.Constants"]);

srv.factory('OperationApi', ['$http', 'AppVariables', function ($http, AppVariables) {
    var OperationApi = {};

    OperationApi.getOperations = function (userId) {
        return $http.get(AppVariables.base_url + '/operation/count_rows/' + userId);
    };
    OperationApi.postPurchase = function(purchase) {
        return $http.post(AppVariables.base_url + '/operation_purchase', purchase);
    };
    OperationApi.postSale= function (sale) {
        return $http.post(AppVariables.base_url + '/operation_sale', sale);
    };

    return OperationApi;
}]);
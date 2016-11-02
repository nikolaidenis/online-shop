var srv = angular.module('OperationService', ["ShopApp.config"]);

srv.factory('OperationApi', ['$http', 'AppVariables', function ($http, AppVariables) {
    var OperationApi = {};
    OperationApi.getOperations = function (userId) {
        return $http.get(AppVariables.base_url + '/operation/' + userId);
    }

    return OperationApi;
}]);
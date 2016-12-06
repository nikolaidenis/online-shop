var srv = angular.module("UserService", ["ShopApp.config"]);

srv.factory('UserApi', [
    '$http', 'AppVariables', function($http, AppVariables) {
        var UserApi = {};

        UserApi.debitBalance = function (payment) {
            return $http.post(AppVariables.base_url + '/user_debit_balance', payment);
        };
        UserApi.chargeBalance = function (payment) {
            return $http.post(AppVariables.base_url + '/user_charge_balance', payment);
        };
        UserApi.getUserData = function(userId) {
            return $http.get(AppVariables.base_url + '/user_info/' + userId);
        }

        return UserApi;
    }
]);
(function () {
    'use strict';

    angular.module("ShopApp.Main", ["ShopApp.Constants"])
        .factory('UserApi', UserApi);

    UserApi.$injector = ['$http', 'AppVariables'];
    
    function UserApi($http, AppVariables) {
        var userApi = {};

        userApi.debitBalance = function (payment) {
            return $http.post(AppVariables.base_url + '/user_debit_balance', payment);
        };
        userApi.chargeBalance = function (payment) {
            return $http.post(AppVariables.base_url + '/user_charge_balance', payment);
        };
        userApi.getUserData = function (userId) {
            return $http.get(AppVariables.base_url + '/user_info/' + userId);
        };
        userApi.getAccounts = function () {
            return $http.get(AppVariables.base_url + '/get_users');
        };
        userApi.changeUserLocking = function (username, block) {
            var obj = {
                Username: username,
                IsBlocking: block
            };
            return $http.post(AppVariables.base_url + '/block_user/', obj);
        };

        return userApi;
    }
})();

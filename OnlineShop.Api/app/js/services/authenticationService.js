﻿var app = angular.module("AuthService", ["ShopApp.config"]);

app.factory('AuthApi', ['$http', '$q', 'AppVariables', function ($http, $q, AppVariables) {
    var authServiceFactory = {};

    authServiceFactory.authentication = {
        isAuthenticated: false,
        userName: ""
    };

    authServiceFactory.setAuthenticatedUser = function(userName) {
        authentication.isAuthenticated = true;
        authentication.userName = userName;
    }
    authServiceFactory.setUnauthenticatedUser = function () {
        authentication.isAuthenticated = false;
        authentication.userName = "";
    }

    authServiceFactory.register = function (userInfo) {
        return $http.post(AppVariables.base_url + '/account/register', userInfo).then(function (response) {
            return response;
        });
    };

    authServiceFactory.login = function (userData) {
        var deffer = $q.defer();
        $http.post(AppVariables.base_url + '/account/register', userData, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
            .success(function (response) {
                localStorage.setItem('authorizationData', { token: response.access_token, username: userData.userName });
                setAuthenticatedUser(userData.userName);
                deffer.resolve(response);
            });
        return deffer.promise;
    }

    authServiceFactory.logout = function() {
        localStorage.removeItem('authorizationData');
        setUnauthenticatedUser();
    }

    authServiceFactory.fillAuth = function() {
        var data = localStorage.getItem('authorizationData');
        if (data) {
            setAuthenticatedUser(data.username);
        }
    }

    return authServiceFactory;
}]);
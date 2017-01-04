var app = angular.module("AuthService", ["ShopApp.config"]);

app.factory('AuthApi', ['$http', '$q', 'AppVariables', 'localStorageService', function ($http, $q, AppVariables, localStorageService) {
    var authServiceFactory = {};

    authServiceFactory.authentication = {
        isAuthenticated: false,
        userName: ""
    };

    authServiceFactory.setAuthenticatedUser = function(userName) {
        authServiceFactory.authentication.isAuthenticated = true;
        authServiceFactory.authentication.userName = userName;
    }
    authServiceFactory.setUnauthenticatedUser = function () {
        authentication.isAuthenticated = false;
        authentication.userName = "";
    }

    authServiceFactory.register = function (userInfo) {
        return $http.post(AppVariables.base_url + '/account/signup', userInfo).then(function (response) {
            return response;
        });
    };

    authServiceFactory.login = function (userData) {
        var deffer = $q.defer();
        var obj = {
            Username:userData.userName,
            Password:userData.password
        }
        $http.post(AppVariables.base_url + '/account/login', JSON.stringify(obj), { headers: { 'Content-Type': 'application/JSON' } })
        //$http.post(AppVariables.base_url + '/token', JSON.stringify(obj), { headers: { 'Content-Type': 'application/JSON' } })
            .then(function (response) {
                localStorageService.add('authorizationData', { token: response.data, username: userData.userName });
                authServiceFactory.setAuthenticatedUser(userData.userName);
                deffer.resolve(response);
            });
        return deffer.promise;
    }

    authServiceFactory.logout = function() {
        localStorageService.remove('authorizationData');
        setUnauthenticatedUser();
    }

    authServiceFactory.fillAuth = function() {
        var data = localStorageService.get('authorizationData');
        if (data) {
            authServiceFactory.setAuthenticatedUser(data.username);
        }
    }

    return authServiceFactory;
}]);
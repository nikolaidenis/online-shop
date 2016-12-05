var app = angular.module("authService", ["ShopApp.config"]);

app.factory('AuthApi', ['$http', '$q', 'AppVariables', 'localStorageService', function ($http, $q, AppVariables, localStorageService) {
    var authServiceFactory = {};

    var authentication = {
        isAuthenticated: false,
        userName: ""
    };

    var setAuthenticatedUser = function(userName) {
        authentication.isAuthenticated = true;
        authentication.userName = userName;
    }
    var setUnauthenticatedUser = function () {
        authentication.isAuthenticated = false;
        authentication.userName = "";
    }

    var register = function (userInfo) {
        return $http.post(AppVariables.base_url + '/account/register', userInfo).then(function (response) {
            return response;
        });
    };

    var login = function (userData) {
        var deffer = $q.defer();
        $http.post(AppVariables.base_url + '/account/register', userData, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
            .success(function (response) {
                localStorageService.set('authorizationData', { token: response.access_token, username: userData.userName });
                setAuthenticatedUser(userData.userName);
                deffer.resolve(response);
            });
        return deffer.promise;
    }

    var logout = function() {
        localStorageService.remove('authorizationData');
        setUnauthenticatedUser();
    }

    var fillAuth = function() {
        var data = localStorageService.get('authorizationData');
        if (data) {
            setAuthenticatedUser(data.username);
        }
    }

    return authServiceFactory;
}])
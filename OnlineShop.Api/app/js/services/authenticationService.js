var app = angular.module("AuthService", ["ShopApp.config"]);

app.factory('AuthApi', ['$http', '$q', 'AppVariables', 'localStorageService', function ($http, $q, AppVariables, localStorageService) {
    var authServiceFactory = {};

    authServiceFactory.user = {
        isAuthenticated: false,
        name: "",
        role:"",
        id: 0
    };

    authServiceFactory.getUserId = function () {
        return authServiceFactory.user.id;
    };

    authServiceFactory.setUserId = function (id) {
        authServiceFactory.user.id = id;
    };

    authServiceFactory.isAuthenticated = function () {
        return authServiceFactory.user.isAuthenticated
            && localStorageService.get('authorizationData')
            && authServiceFactory.user.id !== 0;
    };


    authServiceFactory.setAuthenticatedUser = function (username, role) {
        authServiceFactory.user.isAuthenticated = true;
        authServiceFactory.user.name = username;
        authServiceFactory.user.role = role;
    };
    authServiceFactory.setUnauthenticatedUser = function () {
        authServiceFactory.user.isAuthenticated = false;
        authServiceFactory.user.name = "";
    };

    authServiceFactory.register = function (userInfo) {
        return $http.post(AppVariables.base_url + '/account/signup', userInfo).then(function (response) {
            return response;
        });
    };

    authServiceFactory.login = function (userData) {
        var deffer = $q.defer();

        var data = 'grant_type=password&username=' + userData.userName + '&password=' + userData.password;
        $http.post(AppVariables.base_url + '/token', data, { headers: { 'Content-Type': 'x-www-form-urlencoded' } })
            .then(function (response) {
                localStorageService.add('authorizationData', { token: response.data.access_token, username: userData.userName, role: response.data.role });
                authServiceFactory.fillIdentityInfo().then(function () {
                    authServiceFactory.setAuthenticatedUser(userData.userName,response.data.role);
                    alert('Login success');
                    deffer.resolve(response);
                }, function(error) {
                    alert('Could not find additional info about user. Abort..');
                    localStorageService.remove('authorizationData');
                    authServiceFactory.setUnauthenticatedUser();
                });
            }, function (error) {
                alert('Authorization fails');
                deffer.reject();
            }
        );

        return deffer.promise;
    }

    authServiceFactory.logout = function () {
        localStorageService.remove('authorizationData');
        authServiceFactory.setUnauthenticatedUser();
    }

    authServiceFactory.fillIdentityInfo = function () {
        var deffer = $q.defer();
        var data = localStorageService.get('authorizationData');
        if (data) {
            $http.get(AppVariables.base_url + '/account/' + data.username)
                .then(function(response) {
                    authServiceFactory.setUserId(response.data);
                    authServiceFactory.setAuthenticatedUser(data.username, data.role);
                    deffer.resolve(response);
                }, function(error) {
                    alert('Authorization fails');
                    deffer.reject();
                });
        } else {
            authServiceFactory.setUnauthenticatedUser();
            deffer.reject();
        }
        return deffer.promise;
    }

    return authServiceFactory;
}]);
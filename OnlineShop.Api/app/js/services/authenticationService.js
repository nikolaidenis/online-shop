var app = angular.module("AuthService", ["ShopApp.config"]);

app.factory('AuthApi', ['$http', '$q', 'AppVariables', 'localStorageService', function ($http, $q, AppVariables, localStorageService) {
    var authServiceFactory = {};

    authServiceFactory.authentication = {
        isAuthenticated: false,
        userName: "",
        userId:0,
    };

    authServiceFactory.userId = function(){
        return this.authentication.userId;
    };

    authServiceFactory.isAuthenticated = function(){

        return this.authentication.isAuthenticated 
            && localStorageService.get('authorizationData')
            && this.authentication.userId != 0;  
    };

    authServiceFactory.checkAuthenticationToken = function(){
        var token = localStorageService.get('authorizationData').token;
        $http.get(AppVariables.base_url + '/account/'+userId+'/'+token)
            .then(function(status){
                return true;
            },function(status){
                return false;
            });
    };

    authServiceFactory.setAuthenticatedUser = function(userName) {
        var token = localStorageService.get('authorizationData').token;
        $http.get(AppVariables.base_url + '/account/'+token)
            .then(function(response){
                authServiceFactory.authentication.userId = response.data;
                authServiceFactory.authentication.isAuthenticated = true;
                authServiceFactory.authentication.userName = userName;
            });
    };  
    authServiceFactory.setUnauthenticatedUser = function () {
        authentication.isAuthenticated = false;
        authentication.userName = "";
    };

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
        var token = localStorageService.get('authorizationData').token;
        $http.get(AppVariables.base_url + '/account/logout/'+token)
            .then(function(status){
                localStorageService.remove('authorizationData');
                setUnauthenticatedUser();
            },function(status){
                if(status == 300){
                    localStorageService.remove('authorizationData');
                    setUnauthenticatedUser();
                }
            });        
    }

    authServiceFactory.fillAuth = function() {
        var data = localStorageService.get('authorizationData');
        if (data) {
            authServiceFactory.setAuthenticatedUser(data.username);
        }
    }

    return authServiceFactory;
}]);
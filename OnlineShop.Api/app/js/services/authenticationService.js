var app = angular.module("AuthService", ["ShopApp.config"]);

app.factory('AuthApi', ['$http', '$q', 'AppVariables', 'localStorageService', function ($http, $q, AppVariables, localStorageService) {
    var authServiceFactory = {};

    authServiceFactory.authentication = {
        isAuthenticated: false,
        userName: ""
    };

    authServiceFactory.getUserId = function(){
        return authServiceFactory.authentication.userId;
    };

    authServiceFactory.isAuthenticated = function(){
        return authServiceFactory.authentication.isAuthenticated 
            && localStorageService.get('authorizationData')
            && authServiceFactory.authentication.userId != 0;  
    };


    authServiceFactory.setAuthenticatedUser = function(userName) {
        authServiceFactory.authentication.isAuthenticated = true;
        authServiceFactory.authentication.userName = userName;
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
        
        var data = 'grant_type=password&username='+userData.userName+'&password='+userData.password;
        $http.post(AppVariables.base_url + '/token', data, { headers: { 'Content-Type': 'x-www-form-urlencoded'  } })
            .then(function(response){
                localStorageService.add('authorizationData', { token: response.data.access_token, username: userData.userName });
                authServiceFactory.setAuthenticatedUser(userData.userName);
                alert('Login success');
                deffer.resolve(response);
            },function(error){ 
                alert('Authorization fails');
                deffer.reject();
            }
        );
                
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
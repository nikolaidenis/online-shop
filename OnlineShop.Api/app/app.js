﻿var shopApp = angular.module("ShopApp", ["ngRoute", "ShopApp.config", "ProductService", "OperationService", 
                                                        "UserService","AuthInterceptorService","AuthService","LocalStorageModule"]);

shopApp.config([
    '$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/operations', {
                templateUrl: 'app/templates/operations.html',
                controller: 'OperationsController as operations'
            })
            .when('/archives', {
                templateUrl: 'app/templates/archives.html',
                controller: 'ArchivesController as archives'
            })
            .when('/login', {
                templateUrl: 'app/templates/login.html',
                controller: 'LoginController as loginCtrl'
            })
            .when('/signup', {
                templateUrl: 'app/templates/signup.html',
                controller: 'SignupController as signupCtrl'
            })
            .otherwise({
                redirectTo: '/login'
            });
    }
]);

shopApp.config(function($httpProvider){
});

shopApp.run(['AuthApi', function(AuthApi){
    AuthApi.fillAuth();
}]);
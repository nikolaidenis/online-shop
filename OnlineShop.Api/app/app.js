var shopApp = angular.module("ShopApp", ["ngRoute", "ShopApp.config", "ProductService", "OperationService", "UserService"]);

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
            .when('/', {
                templateUrl: 'app/templates/login.html',
                controller: ''
            })
            .otherwise({
                redirectTo: '/'
            });
    }
]);

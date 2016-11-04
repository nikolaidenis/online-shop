var shopApp = angular.module("ShopApp", ["ngRoute", "ShopApp.config", "ProductService", "OperationService", "UserService"]);

shopApp.config([
    '$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/operations', {
                templateUrl: 'templates/operations.html',
                controller: 'OperationsController'
            })
            .when('/archives', {
                templateUrl: 'templates/archives.html',
                controller: 'ArchivesController'
            })
            .otherwise({
                redirectTo: '/operations'
            });
    }
]);

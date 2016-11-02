var shopApp = angular.module("ShopApp", ["ngRoute","ShopApp.config", "ProductService", "OperationService"]);

shopApp.config([
    '$routeProvider', function($routeProvider) {
        $routeProvider
            .when('/operations', {
                templateUrl: 'operations.html',
                controller: 'OperationsController'
            })
            .when('/archives', {
                templateUrl: 'archives.html',
                controller: 'ArchivesController'
            })
            .otherwise({
                redirectTo: '/operations'
            });
    }
]);

shopApp.controller('TabsController', function ($scope) {
    $scope.tabs = [
        { link: '#/operations', label: 'Operations' },
        { link: '#/archives', label: 'Archive' }
    ];
    
    $scope.selectedTab = $scope.tabs[0];
    $scope.setSelectedTab = function (tab) {
        $scope.selectedTab = tab;
    }

    $scope.tabClass = function(tab) {
        if ($scope.selectedTab === tab) {
            return 'active';
        } else {
            return '';
        }
    }
});

shopApp.controller('OperationsController', ['$scope', 'ProductApi', 'OperationApi', 'TempVariables',
                                    function ($scope, ProductApi, OperationApi, TempVariables) {
    getProducts();
    getUserOperations();

    function getProducts() {
        ProductApi.getProducts().success(function(products) {
            $scope.products = products;
        }).error(function(error) {
            $scope.status = "Unable to retrieve products data: " + error;
        });
    }
    function getUserOperations() {
        OperationApi.getOperations(TempVariables.test_user).success(function (operations) {
            $scope.operations = operations;
        }).error(function(error) {
            $scope.status = "Unable to retrieve operations data: " + error;
        });
    }
}]);
shopApp.controller('ArchivesController', function ($scope) {
    $scope.message = Date.now();
});
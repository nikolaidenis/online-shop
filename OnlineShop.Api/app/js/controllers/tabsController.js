angular.module("ShopApp")
.controller('TabsController', ['$scope', '$state', function ($scope, $state) {
    $state.current.name = 'main';
    $scope.tabs = [
        { route:'operations', label: 'Operations'},
        { route:'archives', label: 'Archive' }
    ];

    $scope.selectedTab = $scope.tabs[0];

    $scope.setSelectedTab = function (tab) {
        $scope.selectedTab = tab;
    }
    
    $scope.go = function(route) {
      $state.go(route);
    };

    $scope.tabClass = function (tab) {
        if ($scope.selectedTab === tab) {
            return 'active';
        } else {
            return '';
        }
    }
}]);
angular.module("ShopApp")
.controller('TabsController', ['$scope', function ($scope) {
    $scope.tabs = [
        { name:'operations', label: 'Operations'},
        { name:'archives', label: 'Archive' }
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
    $scope.isSet = function(tab){
        return $scope.selectedTab.name === tab.name;
    }
}]);
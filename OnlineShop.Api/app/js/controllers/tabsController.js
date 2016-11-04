angular.module("ShopApp")
.controller('TabsController', function ($scope) {
    $scope.tabs = [
        { link: '#/operations', label: 'Operations' },
        { link: '#/archives', label: 'Archive' }
    ];

    $scope.selectedTab = $scope.tabs[0];
    $scope.setSelectedTab = function (tab) {
        $scope.selectedTab = tab;
    }

    $scope.tabClass = function (tab) {
        if ($scope.selectedTab === tab) {
            return 'active';
        } else {
            return '';
        }
    }
});
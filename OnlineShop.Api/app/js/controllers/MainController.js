angular.module("ShopApp")
    .controller('MainController', ['$location', 'AuthApi', function ($location, AuthApi) {
//        $rootScope.isAuthenticated = AuthApi.authentication.isAuthenticated;
		var vm = this;
        vm.isAuthenticated = AuthApi.user.id === 0 ? false : true;
        vm.logout = function () {
            AuthApi.logout();
            $location.path('/login');
        };
}]);
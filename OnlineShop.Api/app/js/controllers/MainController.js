angular.module("ShopApp")
    .controller('MainController', ['$rootScope', '$window', 'AuthApi', function ($rootScope, $window, AuthApi) {
//        $rootScope.isAuthenticated = AuthApi.authentication.isAuthenticated;

        $rootScope.logout = function () {
            AuthApi.logout();
            $window.location.reload();
        };
}]);
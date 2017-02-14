(function() {
    'use strict';
    angular.module('ShopApp.Main')
        .controller('MainController', MainController);

    MainController.$injector = ['$location', 'AuthApi'];

    function MainController($location, AuthApi) {
        var vm = this;

        vm.logout = logout;
        vm.isAuthenticated = AuthApi.user.id === 0 ? false : true;

        function logout() {
            AuthApi.logout();
            $location.path('/login');
        };
    }
})();

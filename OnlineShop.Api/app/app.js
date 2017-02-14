(function () {
    'use strict';

    angular.module("ShopApp", ["ngRoute", 'ui.bootstrap', 'chart.js', "ShopApp.Constants", "ShopApp.Main", "ShopApp.Demo", "ShopApp.Auth"])
        .config(configuration)
        .run(run);

    configuration.$injector = ['$routeProvider'];
    run.$injector = ['$window', '$rootScope', 'localStorageService', 'AuthApi'];


    function configuration($routeProvider) {
        $routeProvider
            .when('/', {
                controller: 'TabsController as ctrl',
                templateUrl: 'app/templates/shopPage.html',
                roles: ["User"]
            })
            .when('/demo', {
                controller: 'TabsController',
                templateUrl: 'app/templates/shopDemoPage.html'
            })
            .when('/admin_panel', {
                url: '/admin_panel',
                controller: 'AdminController as ctrl',
                templateUrl: 'app/templates/adminPage.html',
                roles: ["Admin"]
            })
            .when('/blocked', {
                templateUrl: 'app/templates/blockPage.html',
                controller: 'MainController as ctrl'

            })
            .when('/login', {
                templateUrl: 'app/templates/loginPage.html',
                controller: 'LoginController as ctrl'
            })
            .when('/signup', {
                templateUrl: 'app/templates/signupPage.html',
                controller: 'SignupController as ctrl'
            })
            .when('/notFound', {
                templateUrl: 'app/templates/notFoundPage.html'
            })
            .otherwise({
                redirectTo: '/login'
            });
    };

    function run($window, $rootScope, localStorageService, AuthApi) {
        AuthApi.fillIdentityInfo();

        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            var roles = next.$$route ? next.$$route.roles : [];

            var checkResult = false;
            next.resolve = angular.extend(next.resolve || {}, {
                __authenticating__: authenticate
            });
            $rootScope.$evalAsync(function () {
                $location.path("/notFound");
            });
            function authenticate() {
                var deferred = $q.defer();
                session.getAuthUserInfo.then(function (result) {
                    roles.forEach(function (item, i, arr) {
                        checkResult = checkResult || (result.data.roles.map(function (elem) { return elem.name; }).indexOf(item) !== -1);
                    });
                    if (roles.length !== 0 && !checkResult) {

                    }
                    deferred.resolve();
                });
                return deferred.promise;
            }
        });
    };
})();

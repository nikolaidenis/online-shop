var shopApp = angular.module("ShopApp", ["ngRoute",'chart.js', 'ui.router', "ShopApp.config", "ProductService", "OperationService",
                                                        "UserService", "AuthInterceptorService", "AuthService", "LocalStorageModule"]);

shopApp.config([
    '$routeProvider', '$stateProvider', '$urlRouterProvider', function ($routeProvider, $stateProvider, $urlRouterProvider) {
        $routeProvider
            .when('/main', {
                controller: 'TabsController',
                templateUrl: 'app/templates/general_window.html'
            })
            .when('/login', {
                templateUrl: 'app/templates/login.html',
                controller: 'LoginController as loginCtrl'
            })
            .when('/admin_panel', {
                controller: 'AdminController',
                templateUrl: 'app/templates/admin_window.html'
            })
            .when('/signup', {
                templateUrl: 'app/templates/signup.html',
                controller: 'SignupController as signupCtrl'
            }).otherwise('/main');

        $stateProvider.state('operations', {
            views:{
                "":{
                    url: '/operations',
                    controller: 'OperationsController',                        
                    templateUrl: 'app/templates/operations.html'
                }
            }
        }).state('archives', {
            url: '/archives',
            controller: 'ArchivesController',
            templateUrl: 'app/templates/archives.html'
        });
    }
]);

shopApp.run(['$window', '$rootScope', '$location', 'localStorageService','AuthApi',
        function ($window, $rootScope, $location, localStorageService,AuthApi) {
            AuthApi.fillIdentityInfo();            
//            AuthApi.fillAuth();

            //$rootScope.$on('$routeChangeStart', function (e, next, current) {
            //    if (!localStorageService.get('authorizationData')
            //            && next.originalPath && next.originalPath.indexOf('/login') === -1) {
            //        e.preventDefault();
            //        $location.path('/login');
            //    } else if (localStorageService.get('authorizationData')
            //        &&  next.originalPath && next.originalPath.indexOf('/login') !== -1) {
            //        alert('Authoorized user, log out first!');
            //        e.preventDefault();
            //    }
            //});

            $window.onunload = function () {
                sessionStorage.remove('authorizationData');
            };
        }
]);
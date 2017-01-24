var shopApp = angular.module("ShopApp", ["ngRoute",'ui.bootstrap','chart.js', "ShopApp.config", "ProductService", "OperationService",
                                                        "UserService", "AuthInterceptorService", "AuthService", "LocalStorageModule", "IndexedDBService"]);

shopApp.config([
    '$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/main', {
                controller: 'TabsController',
                templateUrl:  'app/templates/general_window.html'
            })
            .when('/main_demo', {
                controller: 'TabsController',
                templateUrl:  'app/templates/general_window_demo.html'
            })
            .when('/login', {
                templateUrl: 'app/templates/login.html',
                controller: 'LoginController as loginCtrl'
            })
            .when('/admin_panel', {
                templateUrl: 'app/templates/admin_window.html'
            })
            .when('/blocked', {
                templateUrl: 'app/templates/blockPage.html',
                controller: 'MainController as ctrl'
            })
            .when('/signup', {
                templateUrl: 'app/templates/signup.html',
                controller: 'SignupController as signupCtrl'
            }).otherwise('/login');
}]);

shopApp.run(['$window', '$rootScope', '$location', 'localStorageService','AuthApi',
        function ($window, $rootScope, $location, localStorageService,AuthApi) {
            AuthApi.fillIdentityInfo();

            $rootScope.$on('$routeChangeStart', function(){   
                if(AuthApi.user.isBlocked){
                    $location.path('/blocked');
                    return;
                }             
                if(AuthApi.user.role){
                    switch(AuthApi.user.role){
                        case 'Admin': $location.path('/admin_panel'); break;
                        default: $location.path('/main'); break;
                    }
                }
            });
        }
]);
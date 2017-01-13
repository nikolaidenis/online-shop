var shopApp = angular.module("ShopApp", ["ngRoute",'ui.router', "ShopApp.config", "ProductService", "OperationService", 
                                                        "UserService","AuthInterceptorService","AuthService","LocalStorageModule"]);

shopApp.config([
    '$routeProvider','$stateProvider','$urlRouterProvider', function ($routeProvider, $stateProvider,$urlRouterProvider) {
        $routeProvider
            .when('/main',{
                    controller: 'TabsController',
                    templateUrl:'app/templates/general_window.html'
            })
            .when('/login', {
                templateUrl: 'app/templates/login.html',
                controller: 'LoginController as loginCtrl'
            })
            .when('/signup', {
                templateUrl: 'app/templates/signup.html',
                controller: 'SignupController as signupCtrl'
            }).otherwise('/main');

        $stateProvider.state('operations',{
                url:'/operations',
                controller:'OperationsController',
                templateUrl:'app/templates/operations.html'
            }).state('archives',{
                url:'/archives',
                controller:'ArchivesController',
                templateUrl:'app/templates/archives.html'
            });
    }
]);

shopApp.run(['$window', 'localStorageService', function($window, localStorageService){
    $window.onbeforeunload = function () {
          localStorageService.remove('authorizationData');
      };
}]);
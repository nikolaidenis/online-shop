var shopApp = angular.module("ShopApp", ["ngRoute",'ui.bootstrap','ui.router', 'chart.js', "ShopApp.config", "ProductService", "OperationService",
                                                        "UserService", "AuthInterceptorService", "AuthService", "LocalStorageModule", "IndexedDBService"]);

shopApp.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('main',{
                url: '/',
                controller: 'TabsController',
                templateUrl:  'app/templates/shopPage.html',
                resolve:{
                    user: function($state, AuthApi){
                    AuthApi.fillIdentityInfo().then(function(){
                        if(AuthApi.user.isBlocked){
                            $state.go('block');
                        }
                    });
                }
            }
            })
            .state('demo',{
                url:'/demo',
                controller: 'TabsController',
                templateUrl:  'app/templates/shopDemoPage.html'
            })
            .state('admin',{
                url:'/admin_panel', 
                templateUrl: 'app/templates/adminPage.html'
            })
            .state('block',{
                url:'/blocked',
                templateUrl: 'app/templates/blockPage.html',
                controller: 'MainController as ctrl',
                redirectTo: 'login',
                resolve:{
                    auth: function(AuthApi, $q){
                        var defer = $q.defer();
                        AuthApi.user.isBlocked ? defer.resolve():defer.reject({message:"rejected"});
                        return defer.promise;
                    }
                }
            })
            .state('login',{
                url:'/login',
                templateUrl: 'app/templates/loginPage.html',
                controller: 'LoginController as loginCtrl',
                resolve: {
                    security: function(localStorageService, $state){
                        var data = localStorageService.get('authorizationData');
                        if(data){
                            if(data.role === 'User'){
                                $state.go('main');
                            }else if(data.role === 'Admin'){
                                $state.go('admin');
                            }                            
                        }
                    }
                }
            })
            .state('signup',{
                url:'/signup',
                templateUrl: 'app/templates/signupPage.html',
                controller: 'SignupController as signupCtrl'
            });
        $urlRouterProvider.otherwise('login');
}]);

shopApp.run(['$window', '$rootScope', '$state', 'localStorageService','AuthApi',
    function ($window, $rootScope, $state, localStorageService,AuthApi) {
        AuthApi.fillIdentityInfo();

        $rootScope.$on('$stateChangeError', function(evt, to, toParams, from, fromParams, error) {
            if (error) {
                $state.go(to.redirectTo);
            }
        });
    }
]);
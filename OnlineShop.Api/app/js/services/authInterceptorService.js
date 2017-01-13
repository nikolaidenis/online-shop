var app = angular.module('AuthInterceptorService', ['ShopApp.config']);

app.factory('InterceptorApi', ['$q', '$injector', '$location', 'localStorageService',function($q,$injector, $location,localStorageService){
		
		var InterceptorApi = {};
		
		InterceptorApi.request = function(config){
			config.headers = config.headers || {};
			
			var authData = localStorageService.get('authorizationData');
			if(authData){
				if(config.url.indexOf('/api/token') > 0){
					alert('Authenticated! Redirect...');
					$location.path('/main');					
				}
				config.headers.Authorization = 'Bearer ' + authData.token;
			}

			return config;
		}
		
		InterceptorApi.responseError = function(rejection){
			if(rejection.status === 401){
				localStorageService.remove('authorizationData');
				$location.path('/login');
			}

			return $q.reject(rejection);
		}

		return InterceptorApi;
	}]);

app.config(function($httpProvider){
    $httpProvider.interceptors.push('InterceptorApi');    
});
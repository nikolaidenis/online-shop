var app = angular.module('AuthInterceptorService', ['ShopApp.config']);

app.factory('InterceptorApi', ['$q', '$injector', '$location', function($q,$injector, $location){
		
		var InterceptorApi = {};
		
		InterceptorApi.request = function(config){
			config.headers = config.headers || {};
			
			var authData = localStorage.getItem('authorizationData');
			if(authData){
				config.headers.Authorization = 'Bearer ' + authData.token;
			}

			return config;
		}

		InterceptorApi.responseError = function(rejection){
			if(rejection.status === 401){
				$location.path('/login');
			}

			return $q.reject(rejection);
		}

		return InterceptorApi;
	}]);

app.config(function($httpProvider){
    $httpProvider.interceptors.push('InterceptorApi');    
});
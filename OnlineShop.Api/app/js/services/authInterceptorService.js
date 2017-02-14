var app = angular.module('ShopApp.Auth', ['ShopApp.Constants']);

app.factory('InterceptorApi', ['$q', '$injector', '$location', 'localStorageService',function($q,$injector, $location,localStorageService){	
	var InterceptorApi = {};
	
	InterceptorApi.request = function(config){
		config.headers = config.headers || {};
		
		var authData = localStorageService.get('authorizationData');
		if(authData){
			config.headers.Authorization = 'Bearer ' + authData.token;
			return config;
		}

		return config;
	};
	
	InterceptorApi.responseError = function(rejection){
		if(rejection.status === 401){
			localStorageService.remove('authorizationData');
			$location.path('/login');
		}

		return $q.reject(rejection);
	};

	return InterceptorApi;
}]);

app.config(function($httpProvider){
    $httpProvider.interceptors.push('InterceptorApi');    
});
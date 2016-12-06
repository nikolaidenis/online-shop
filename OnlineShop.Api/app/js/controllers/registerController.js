angular.module('ShopApp')
	.controller('RegisterController', ['$scope', '$location','AuthApi', function($scope,$location,AuthApi){
		
		$scope.message = "";

		$scope.data = {
			username: "",
			password: "",
			confirmPassword:""
		};

		$scope.signup = function(){
			AuthApi.register($scope.data).then(function(response){

			}, function (error){
				$scope.message = response.Message;
			});
		}

	}]);
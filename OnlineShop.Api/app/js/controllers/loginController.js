angular.module("ShopApp")
    .controller('LoginController', ['$scope', '$location', 'AuthApi', function ($scope, $location, AuthApi) {

        $scope.loginData = {
            userName: "",
            password: ""
        };
        
        $scope.message = "";

        $scope.submit = function(){
        	AuthApi.login($scope.loginData).then(function(response){
        		$location.path('/operations');
        	}, function(error){
        		//toDo:
        		$scope.message = error.Message;
        	});
        }

    }]);
angular.module("ShopApp")
    .controller('LoginController', ['$scope', '$location', 'AuthApi', function ($scope, $location, AuthApi) {

        $scope.loginData = {
            userName: "",
            password: ""
        };
        
        $scope.message = "";

        $scope.submit = function(){
        	AuthApi.login($scope.loginData).then(function(response){
                alert('Login completed!');
        		$location.path('/operations');
        	}, function(error){
        		//toDo:
        		alert('Login failed. Check credentials');
        	});
        }

    }]);
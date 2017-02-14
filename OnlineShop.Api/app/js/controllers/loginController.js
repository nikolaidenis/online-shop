angular.module("ShopApp.Auth")
    .controller('LoginController', ['$scope', '$location', 'AuthApi', function ($scope, $location, AuthApi) {

        $scope.loginData = {
            username: "",
            password: ""
        };
        
        $scope.message = "";

        $scope.submit = function(){
        	AuthApi.login($scope.loginData)
            .then(function(response){
        		$location.path('/');
        	});
        }
    }]);
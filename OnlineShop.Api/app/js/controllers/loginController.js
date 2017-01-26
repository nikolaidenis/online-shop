angular.module("ShopApp")
    .controller('LoginController', ['$scope', '$state', 'AuthApi', function ($scope, $state, AuthApi) {

        $scope.loginData = {
            username: "",
            password: ""
        };
        
        $scope.message = "";

        $scope.submit = function(){
        	AuthApi.login($scope.loginData)
            .then(function(response){
        		$state.go('main');
        	});
        }
    }]);
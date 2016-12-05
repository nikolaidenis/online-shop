angular.module("ShopApp")
    .controller('LoginController', ['$scope', '$location', 'AuthApi', function ($scope, $location, AuthApi) {

        $scope.loginData = {
            userName: "",
            password: ""
        };

        $scope.login = function(){
        	AuthApi.login($scope.loginData).then(function(response){
        		
        	});
        }

    }]);
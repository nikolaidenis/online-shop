angular.module("ShopApp")
    .controller('SignupController', ['$scope', '$location', 'AuthApi', function ($scope, $location, AuthApi) {

        $scope.signupData = {
            userName: "",
            password: "",
            confirmPassword:"",
            email:""
        };
        
        $scope.message = "";

        $scope.submit = function(){

        	if(!isValidModel()) {
        		$scope.message = "User data not valid";
        		return;
        	}
        	AuthApi.register($scope.signupData).then(function(response){
        		alert('Please, check your inbox for confirmation email');        		
        	}, function(error){        		//toDo:
        		$scope.message = error.Message;
        	});
        }

        var isValidModel = function(){
            return $scope.signupData.password == $scope.signupData.confirmPassword && $scope.signupData.userName && $scope.signupData.email && validateEmail($scope.signupData.email);
        }

        var validateEmail = function(email) {
		    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
		    return re.test(email);
		}

    }]);
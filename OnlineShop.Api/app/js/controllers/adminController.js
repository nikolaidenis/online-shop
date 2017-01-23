angular.module("ShopApp")
    .controller('AdminController', ['$scope','$window', 'UserApi', function ($scope,$window, UserApi) {
    
    $scope.accounts = [],
    $scope.filtered = [],
    $scope.currentPage = 1,
    $scope.numPerPage = 10,
    $scope.maxPaginationSize = 5;


    function getAccounts(){
    	var accounts = UserApi.getAccounts()
    		.then(function(response){    					
	    			$scope.accounts = $scope.accounts.concat(response.data);
    		});    	
    };

    function countPage(){
    	if($scope.accounts.length != 0){
		    var begin = (($scope.currentPage - 1) * $scope.numPerPage),
		    	  end = begin + $scope.numPerPage;
		    $scope.filtered = $scope.accounts.slice(begin, end);
		}
    }

    getAccounts();

  	$scope.$watch("accounts", function(){
  		countPage();
  	});

  	$scope.$watch("currentPage + numPerPage", function() {
	    	countPage();
	});

    
    $scope.changeLocking = function(account){
    	UserApi.changeUserLocking(account.username, !account.blocked)
    		.then(function(response){
    			account.blocked = !account.blocked;
    			alert('Successed!');
    		}, function(error){
    			alert('Bad');
    		});
    }


}]);
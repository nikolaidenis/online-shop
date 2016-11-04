angular.module("ShopApp")
    .controller('OperationsController',
    ['$scope', 'ProductApi', 'OperationApi', 'TempVariables', 'UserApi',
     function ($scope, ProductApi, OperationApi, TempVariables, UserApi) {
         getProducts();
         getUser();
         getUserOperations();

         function getProducts() {
             ProductApi.getProducts().success(function (products) {
                 $scope.products = products;
             }).error(function (error) {
                 $scope.status = "Unable to retrieve products data: " + error.Message;
             });
         }
         function getUser() {
             UserApi.getUserData(TempVariables.test_user).success(function (user) {
                 $scope.user = user;
             }).error(function (error) {
                 $scope.status = "Unable to retrieve user data: " + error.Message;
             });
         }
         function getUserOperations() {
             OperationApi.getOperations(TempVariables.test_user).success(function (operations) {
                 $scope.operations = operations;
             }).error(function (error) {
                 $scope.status = "Unable to retrieve operations data: " + error.Message;
             });
         }

         $scope.purchase = function (product) {
             var purchaseObj = {
                 'UserId': TempVariables.test_user,
                 'ProductId': product.Id,
                 'Amount': product.Cost,
                 'IsSelled': false
             }
             var paymentObj = {
                 'UserId': TempVariables.test_user,
                 'Amount': product.Cost
             }
             UserApi.debitBalance(paymentObj).then(
                 function () {
                     OperationApi.postPurchase(purchaseObj).success(function () {
                         alert("Purchase completed");
                     }).error(function (error) {
                         alert("Something is wrong: " + error.Message);
                     });
                 }, function () {
                     alert("Payment is wrong");
                 }
             );
         }
         $scope.chargeUserBalance = function (userId) {
             var amount = prompt("Enter amount:", "0");
             if (!amount) {
                 return;
             }
             if (!amount.match(/^\d+$/)) {
                 alert("Numbers only!");
                 return;
             }
             var payment = {
                 'UserId': userId,
                 'Amount': amount
             }

             UserApi.chargeBalance(payment).then(function () {
                 alert("Balance charged! Yeaaaa");
             }, function () {
                 alert("Charge invalid!");
             });
         }
         $scope.countOperations = function (productId) {
             var collection = [];
             for (var i = 0; i < $scope.operations.length; i++) {
                 if ($scope.operations[i].ProductId === productId
                        && $scope.operations[i].IsSelled == false) {
                     collection.push($scope.operations[i]);
                 }
             }
             return collection;
         }
         $scope.sellLastProduct = function (productId) {
             for (var i = $scope.operations.length - 1; i >= 0; i--) {
                 if ($scope.operations[i].ProductId === productId
                        && $scope.operations[i].IsSelled == false) {
                     var payment = {
                         'UserId': $scope.operations[i].UserId,
                         'Amount': $scope.operations[i].Amount
                     }
                     UserApi.chargeBalance(payment)
                         .success(function () {
                             OperationApi.postSale($scope.operations[i])
                                 .success(function () {
                                     alert("Sell succeed! Yeaaa");
                                     $scope.countOperations($scope.operations[i].ProductId);
                                 })
                                 .error(function () {
                                     alert("Sell invalid!");
                                 });
                         }).error(function (error) {
                             alert("Error seling item: Returning money problem, " + error.Message);
                         });

                     return;
                 }
             }

         }
     }]);
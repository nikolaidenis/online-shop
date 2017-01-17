
    angular.module("ShopApp")
        .controller('OperationsController', [
            '$scope', 'ProductApi', 'OperationApi', 'AuthApi', 'UserApi', '$q',
            function($scope, ProductApi, OperationApi, AuthApi, UserApi, $q) {
                $scope.productCountList = [];
                var userId = AuthApi.user.id;

//                getAllProducts();
//                getUser();

                $scope.prodHub = $.connection.customHub; // initializes hub

                $scope.prodHub.client.updateCosts = function () {
                    alert('Updated');
                };

                $.connection.hub.start().done(function() {
                    alert('Hub started');

                });


                function updateUserData() {
                    getUser();
                    getUserOperations();
                };

                function getAllProducts() {
                    ProductApi.getProducts()
                        .success(function(products) {
                            $scope.products = products;
                            getUserOperations();
                        })
                        .error(function(error) {
                            $scope.status = "Unable to retrieve products data: " + error.Message;
                        });
                };

                function getUser() {
                    UserApi.getUserData(userId).success(function(user) {
                        $scope.user = user;
                    }).error(function(error) {
                        $scope.status = "Unable to retrieve user data: " + error.Message;
                    });
                };

                function getUserOperations() {
                    OperationApi.getOperations(userId).success(function(operations) {
                        $scope.operations = operations;
                        countOperations();
                    }).error(function(error) {
                        $scope.status = "Unable to retrieve operations data: " + error.Message;
                    });
                };

                function countOperations() {
                    for (var productIndex = 0; productIndex < $scope.products.length; productIndex++) {
                        var collection = [];
                        var id = $scope.products[productIndex].id;
                        for (var i = 0; i < $scope.operations.length; i++) {
                            if ($scope.operations[i].productId === id
                                && $scope.operations[i].isSelled == false) {
                                collection.push($scope.operations[i]);
                            }
                        }
                        $scope.productCountList[id] = collection;
                    }
                };

                $scope.purchase = function(product) {
                    var purchaseObj = {
                        'UserId': userId,
                        'ProductId': product.id,
                        'Amount': product.cost,
                        'IsSelled': false
                    }
                    var paymentObj = {
                        'UserId': userId,
                        'Amount': product.cost
                    }
                    UserApi.debitBalance(paymentObj).then(
                        function() {
                            OperationApi.postPurchase(purchaseObj).success(function() {
                                updateUserData();
                                alert("Purchase completed");
                            }).error(function(error) {
                                alert("Something is wrong: " + error.Message);
                            });
                        }, function() {
                            alert("Payment is wrong");
                        }
                    );
                };

                $scope.chargeUserBalance = function() {
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

                    UserApi.chargeBalance(payment).then(function() {
                        getUser();
                        alert("Balance charged! Yeaaaa");
                    }, function() {
                        alert("Charge invalid!");
                    });
                };

                $scope.sellLastProduct = function(productId) {
                    for (var i = $scope.operations.length - 1; i >= 0; i--) {

                        if ($scope.operations[i].productId === productId
                            && $scope.operations[i].isSelled == false) {
                            var payment = {
                                'UserId': $scope.operations[i].userID,
                                'Amount': $scope.operations[i].amount
                            }
                            UserApi.chargeBalance(payment)
                                .success(function() {
                                    OperationApi.postSale($scope.operations[i])
                                        .success(function() {
                                            updateUserData();
                                            alert("Sell succeed! Yeaaa");
                                        })
                                        .error(function() {
                                            alert("Sell invalid!");
                                        });
                                }).error(function(error) {
                                    alert("Error seling item: Returning money problem, " + error.Message);
                                });
                            return;
                        }
                    }
                };
            }
        ]);
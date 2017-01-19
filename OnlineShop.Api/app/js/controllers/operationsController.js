    angular.module("ShopApp")
        .controller('OperationsController', [
            '$scope', '$rootScope', 'ProductApi', 'OperationApi', 'AuthApi', 'UserApi', '$q',
            function($scope, $rootScope,ProductApi, OperationApi, AuthApi, UserApi, $q) {
                var scope = $scope;

                scope.productCountList = [];
                scope.vm = {};
                scope.chart = {};
                scope.vm.products = [];

                scope.$on('chart-create', function(e, cht){
                    
                });

                var userId = AuthApi.user.id;

                function init(){
                    var defer = $q.defer();

                    scope.getAllProducts();
                    getUser().then(function(){
                        getUserOperations().then(function(){
                            scope.drawCanvas();
                        });
                    });
                    defer.resolve(true);

                    return defer.promise;
                };

                scope.drawCanvas = function(){
                    scope.productLabels = [];
                    scope.productData = [];
                    for(var i = 0; i <scope.vm.products.length; i++){
                        scope.productLabels.push(scope.vm.products[i].name);
                        var id = scope.vm.products[i].id;
                        scope.productData.push(scope.productCountList[id].length);
                    }
                        
                    scope.options =  {
                          responsive: false,
                          maintainAspectRatio: false
                        }
                };

                function updateUserData() {
                    getUser().then(function(){
                        getUserOperations().then(function(){
                            scope.drawCanvas();
                        });
                    });
                };

                function enableHub(){
                    scope.prodHub = $.connection.customHub; 
                    scope.prodHub.client.updateCosts = function () {
                        scope.getAllProducts().then(function(){
                            scope.$apply();
                        });
                        
                    };
                    $.connection.hub.start();
                    //alert('HUB_ENABLED');
                };

                scope.getAllProducts = function () {
                    var defer = $q.defer();
                    ProductApi.getProducts()
                        .success(function(products) {
                            scope.vm.products = products;                            
                            defer.resolve(products);
                        })
                        .error(function(error) {
                            scope.status = "Unable to retrieve products data: " + error.Message;
                            defer.reject();
                        });
                    return defer.promise;
                };

                function getUser() {
                    var defer = $q.defer();
                    UserApi.getUserData(userId).success(function(user) {
                        scope.user = user;
                        defer.resolve(user);
                    }).error(function(error) {
                        scope.status = "Unable to retrieve user data: " + error.Message;
                        defer.reject();
                    });
                    return defer.promise;
                };

                function getUserOperations() {
                    var defer = $q.defer();
                    OperationApi.getOperations(userId).success(function(operations) {
                        scope.operations = operations;
                        defer.resolve(operations);
                        countOperations();
                    }).error(function(error) {
                        scope.status = "Unable to retrieve operations data: " + error.Message;
                        defer.reject();
                    });
                    return defer.promise;
                };

                function countOperations() {
                    for (var productIndex = 0; productIndex < scope.vm.products.length; productIndex++) {
                        var collection = [];
                        var id = scope.vm.products[productIndex].id;
                        for (var i = 0; i < scope.operations.length; i++) {
                            if (scope.operations[i].productId === id
                                && scope.operations[i].isSelled == false) {
                                collection.push(scope.operations[i]);
                            }
                        }
                        scope.productCountList[id] = collection;
                    }
                };

                scope.purchase = function(product) {
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

                scope.chargeUserBalance = function() {
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

                scope.sellLastProduct = function(productId) {
                    for (var i = scope.operations.length - 1; i >= 0; i--) {
                        if (scope.operations[i].productId === productId
                            && scope.operations[i].isSelled == false) {
                            var payment = {
                                'UserId': scope.operations[i].userID,
                                'Amount': scope.operations[i].amount
                            }
                            UserApi.chargeBalance(payment)
                                .success(function() {
                                    OperationApi.postSale(scope.operations[i])
                                        .success(function() {
                                            updateUserData();
                                            alert("Sell succeed!");
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

                init().then(function(){
                    enableHub();
                });
            }
        ]);
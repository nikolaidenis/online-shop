    angular.module("ShopApp")
        .controller('OperationsController', [
            '$scope', '$rootScope', 'ProductApi', 'OperationApi', 'AuthApi', 'UserApi', '$q',
            function($scope, $rootScope,ProductApi, OperationApi, AuthApi, UserApi, $q) {
                var scope = $scope;

                scope.productCountList = [];

                scope.productStatisticList = {};    
                scope.productProfitList = {};            

                scope.chart = {};
                scope.products = [];

                var userId = AuthApi.user.id;

                init();

                function initializeProfit(){
                    for(var i=0; i<scope.products.length; i++){
                        var obj = {
                            productName:scope.products[i].name,
                            profit:0
                        };
                        scope.productProfitList[scope.products[i].id] = obj;
                    }
                };

                function initializeStatistic(){
                    for(var i=0; i<scope.products.length; i++){
                        var obj = {
                            productName:scope.products[i].name,
                            numBought:0,
                            numSold:0
                        };
                        scope.productStatisticList[scope.products[i].id] = obj;
                    }
                };

                
                scope.trackSold = function(productId){
                    ++scope.productStatisticList[productId].numSold;
                };
                scope.trackBought = function(productId){
                    ++scope.productStatisticList[productId].numBought;
                };

                scope.countProfit = function(productId, productCost){
                    var obj = scope.getProduct(productId);
                    scope.productProfitList[productId].profit += obj.cost - productCost;
                };

                function init(){
                    getAllProducts(function(){   
                        getUser();
                        getUserOperations(function(){                            
                            countOperations();          
                            drawCanvas();
                        });                              
                        enableHub();
                        initializeStatistic();
                        initializeProfit();
                    });
                };

                function drawCanvas(){
                    scope.productLabels = [];
                    scope.productData = [];
                    for(var i = 0; i <scope.products.length; i++){
                        scope.productLabels.push(scope.products[i].name);
                        var id = scope.products[i].id;
                        scope.productData.push(scope.productCountList[id].length);
                    }
                        
                    scope.options =  {
                          responsive: false,
                          maintainAspectRatio: false
                        }
                };

                function updateUserData() {
                    getUser();
                    getUserOperations(function(){                            
                        countOperations();          
                        drawCanvas();
                    });                
                };

                function enableHub(){
                    scope.prodHub = $.connection.customHub; 
                    scope.prodHub.client.updateCosts = function () {
                        getAllProducts(function(){
                            $rootScope.$apply();
                        });                        
                    };
                    $.connection.hub.start();
                };

                function getAllProducts(callback) {
                    var defer = $q.defer();
                    ProductApi.getProducts()
                        .success(function(products) {
                            scope.products = products;                            
                            defer.resolve(products);
                            callback();
                            return defer.promise;
                        })
                        .error(function(error) {
                            scope.status = "Unable to retrieve products data: " + error.Message;
                            defer.reject();                            
                            return defer.promise;
                        });
                };

                scope.getProduct = function(id){
                    for(var i = 0; i < scope.products.length; i++){
                        if(scope.products[i].id === id){
                            return scope.products[i];
                        }
                    }
                }

                function getUser() {
                    var defer = $q.defer();
                    UserApi.getUserData(userId).success(function(user) {
                        scope.user = user;
                        defer.resolve(user);
                        return defer.promise;
                    }).error(function(error) {
                        scope.status = "Unable to retrieve user data: " + error.Message;
                        defer.reject();
                        return defer.promise;
                    });
                };

                function getUserOperations(callback) {
                    var defer = $q.defer();
                    OperationApi.getOperations(userId).success(function(operations) {
                        scope.operations = operations;
                        defer.resolve(operations);
                        callback();
                        return defer.promise;
                    }).error(function(error) {
                        scope.status = "Unable to retrieve operations data: " + error.Message;
                        defer.reject();
                        return defer.promise;
                    });
                };

                function countOperations() {
                    for (var productIndex = 0; productIndex < scope.products.length; productIndex++) {
                        var collection = [];
                        var id = scope.products[productIndex].id;
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
                                scope.trackBought(product.id);
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
                                            scope.trackSold(productId);
                                            updateUserData();
                                            scope.countProfit(productId, scope.operations[i].amount);
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

            }
        ]);
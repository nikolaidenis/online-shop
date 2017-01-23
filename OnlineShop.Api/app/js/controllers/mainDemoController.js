angular.module("ShopApp")
    .controller('MainDemoController', [
        '$rootScope', 'ProductApi', 'OperationApi', 'AuthApi', 'UserApi', '$q',
        function ($rootScope, ProductApi, OperationApi, AuthApi, UserApi, $q) {

            var vm = this;

            vm.productCountList = [];

            vm.productStatisticList = {};
            vm.productProfitList = {};

            vm.products = [];

            var userId = AuthApi.user.id;


            function initializeProfit() {
                for (var i = 0; i < vm.products.length; i++) {
                    var obj = {
                        productName: vm.products[i].name,
                        profit: 0
                    };
                    vm.productProfitList[vm.products[i].id] = obj;
                }
            };

            function initializeStatistic() {
                for (var i = 0; i < vm.products.length; i++) {
                    var obj = {
                        productName: vm.products[i].name,
                        numBought: 0,
                        numSold: 0
                    };
                    vm.productStatisticList[vm.products[i].id] = obj;
                }
            };


            vm.trackSold = function (productId) {
                ++vm.productStatisticList[productId].numSold;
            };
            vm.trackBought = function (productId) {
                ++vm.productStatisticList[productId].numBought;
            };

            vm.countProfit = function (productId, productCost) {
                var obj = vm.getProduct(productId);
                vm.productProfitList[productId].profit += obj.cost - productCost;
            };

            function init() {
                getAllProducts(function () {
                    initializeUser();
                    enableHub();
                    
                });
            };

            function updateUser(){
                countOperations();
                drawCanvas();
                initializeStatistic();
                initializeProfit();
            }

            function initMoney() {
                if(vm.user){
                    vm.user.balance = 10000000;
                }
            }

            function chargeBalance(value){
                var defer = $q.defer();
                if(vm.user){
                    defer.resolve();
                    vm.user.balance += value;
                }
                return defer.promise;
            }

            function debitBalance(value){
                var defer = $q.defer();
                if(vm.user){
                    vm.user.balance >= value ? vm.user.balance -= value : defer.reject();
                    defer.resolve();
                }else{
                    defer.reject();
                }
                return defer.promise;
            }

            function drawCanvas() {
                vm.productLabels = [];
                vm.productData = [];
                for (var i = 0; i < vm.products.length; i++) {
                    vm.productLabels.push(vm.products[i].name);
                    var id = vm.products[i].id;
                    vm.productData.push(vm.productCountList[id].length);
                }

                vm.options = {
                    responsive: false,
                    maintainAspectRatio: false
                }
            };

            function updateUserData() {
                getUserOperations(function () {
                    updateUser();                    
                });
            };

            function enableHub() {
                vm.prodHub = $.connection.customHub;
                vm.prodHub.client.updateCosts = function () {
                    getAllProducts(function () {
                        $rootvm.$apply();
                    });
                };
                $.connection.hub.start();
            };

            function getAllProducts(callback) {
                var defer = $q.defer();
                ProductApi.getProducts()
                    .success(function (products) {
                        vm.products = products;
                        defer.resolve(products);
                        callback();
                        return defer.promise;
                    })
                    .error(function (error) {
                        vm.status = "Unable to retrieve products data: " + error.Message;
                        defer.reject();
                        return defer.promise;
                    });
            };

            vm.getProduct = function (id) {
                for (var i = 0; i < vm.products.length; i++) {
                    if (vm.products[i].id === id) {
                        return vm.products[i];
                    }
                }
            }

            function initializeUser() {
                var defer = $q.defer();
                vm.user = {};
                initMoney();
                defer.resolve();
                return defer.promise;
            };

            function getUserOperations(callback) {
                var defer = $q.defer();
                refreshOperations().success(function (operations) {
                    vm.operations = operations;
                    defer.resolve(operations);
                    callback();
                    return defer.promise;
                }).error(function (error) {
                    vm.status = "Unable to retrieve operations data: " + error.Message;
                    defer.reject();
                    return defer.promise;
                });
            };

            function countOperations() {
                for (var productIndex = 0; productIndex < vm.products.length; productIndex++) {
                    var collection = [];
                    var id = vm.products[productIndex].id;
                    for (var i = 0; i < vm.operations.length; i++) {
                        if (vm.operations[i].productId === id
                            && vm.operations[i].isSelled == false) {
                            collection.push(vm.operations[i]);
                        }
                    }
                    vm.productCountList[id] = collection;
                }
            };




            vm.purchase = function (product) {
                var purchaseObj = {
                    'ProductId': product.id,
                    'Amount': product.cost,
                    'IsSelled': false
                }

                debitBalance(product.cost).then(
                    function () {
                        addOperation(purchaseObj).success(function () {
                            vm.trackBought(product.id);
                            updateUserData();
                            alert("Purchase completed");
                        }).error(function (error) {
                            alert("Something is wrong: " + error.Message);
                        });
                    }, function () {
                        alert("Payment is wrong");
                    }
                );
            };




            vm.chargeUserBalance = function () {
                var amount = prompt("Enter amount:", "0");
                if (!amount) {
                    return;
                }
                if (!amount.match(/^\d+$/)) {
                    alert("Numbers only!");
                    return;
                }

                chargeBalance(amount).then(function () {
                    getUser();
                    alert("Balance charged! Yeaaaa");
                }, function () {
                    alert("Charge invalid!");
                });
            };

            vm.sellLastProduct = function (productId) {
                for (var i = vm.operations.length - 1; i >= 0; i--) {
                    if (vm.operations[i].productId === productId
                        && vm.operations[i].isSelled == false) {
                        var payment = {
                            'UserId': vm.operations[i].userID,
                            'Amount': vm.operations[i].amount
                        }
                        UserApi.chargeBalance(payment)
                            .success(function () {
                                OperationApi.postSale(vm.operations[i])
                                    .success(function () {
                                        vm.trackSold(productId);
                                        updateUserData();
                                        vm.countProfit(productId, vm.operations[i].amount);
                                        alert("Sell succeed!");
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
            };





            vm.refreshOperations = function(){
                indexedDBDataSvc.getOperations().then(function(data){
                    vm.productCountList=data;
                }, function(err){
                    $window.alert(err);
                });
            };
                   
                  vm.addOperation = function(obj){
                    indexedDBDataSvc.addTodo(vm.todoText).then(function(){
                      vm.refreshList();
                      vm.todoText="";
                    }, function(err){
                      $window.alert(err);
                    });
                  };
                   
                  
                   
                  function initDB(){
                    indexedDBDataSvc.open().then(function(){
                      vm.refreshList();
                    });
                  }
                   
                  initDB();

            init();
        }
    ]);
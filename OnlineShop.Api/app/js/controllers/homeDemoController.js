angular.module("ShopApp.Demo")
    .controller('MainDemoController', [
        '$rootScope', 'ProductApi',  '$q', 'DBInterfaceApi',
        function ($rootScope, ProductApi, $q, DBInterfaceApi) {

            var vm = this;

            vm.productCountList = [];
            vm.productStatisticList = {};
            vm.productProfitList = {};
            vm.products = [];

            init();

            function init() {
                initDB(function(){
                    getAllProducts(function () {                        
                        updateUser();
                        initializeUser();
                        initializeStatistic();
                        initializeProfit();
                        enableHub();                    
                    });
                });
            };

            function updateUser(){
                getOperations(function(){
                    countOperations();
                    drawCanvas();
                });
            };

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

            function trackSold(productId) {
                ++vm.productStatisticList[productId].numSold;
            };

            function trackBought(productId) {
                ++vm.productStatisticList[productId].numBought;
            };

            function countProfit(productId, productCost) {
                var obj = vm.getProduct(productId);
                vm.productProfitList[productId].profit += obj.cost - productCost;
            };

            function initMoney() {
                if(vm.user){
                    vm.user.balance = 10000000;
                }
            };

            function initializeUser() {
                var defer = $q.defer();
                vm.user = {};
                initMoney();
                defer.resolve();

                return defer.promise;
            };

            function chargeBalance(value){
                var defer = $q.defer();
                if(vm.user){
                    vm.user.balance += parseInt(value);
                    defer.resolve(true);
                }else{
                    defer.reject();
                }

                return defer.promise;
            };

            function debitBalance(value){
                var defer = $q.defer();
                if(vm.user){
                    vm.user.balance >= value ? vm.user.balance -= value : defer.reject();
                    defer.resolve();
                }else{
                    defer.reject();
                }

                return defer.promise;
            };

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

            function refreshOperations(){
                var defer = $q.defer();
                DBInterfaceApi.getOperations()
                    .then(function(data){
                        vm.operations=data;
                        defer.resolve(data);
                    }, function(err){
                        $window.alert(err);
                        defer.reject();
                    });

                return defer.promise;
            };
                   
            function putOperation(obj){
                var defer = $q.defer();
                DBInterfaceApi.putOperation(obj)
                    .then(function(){
                        refreshOperations();
                        defer.resolve(true);
                    }, function(err){
                        $window.alert(err);
                        defer.reject();
                    });

                return defer.promise;
            };

            function initDB(fn){
                var defer = $q.defer();
                DBInterfaceApi.open().then(function(){
                    defer.resolve(true);
                    fn();
                }, function(){
                    defer.reject();
                });

                return defer.promise;
            };   

            function enableHub() {
                vm.prodHub = $.connection.customHub;
                vm.prodHub.client.updateCosts = function () {
                    getAllProducts(function () {
                        $rootScope.$apply();
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

            function getOperations(callback) {
                var defer = $q.defer();
                refreshOperations().then(function (operations) {
                    vm.operations = operations;
                    defer.resolve(operations);
                    callback();
                    return defer.promise;
                },function (error) {
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
                        if (vm.operations[i].ProductId === id
                            && vm.operations[i].IsSelled == false) {
                            collection.push(vm.operations[i]);
                        }
                    }
                    vm.productCountList[id] = collection;
                }
            };

            vm.getProduct = function (id) {
                for (var i = 0; i < vm.products.length; i++) {
                    if (vm.products[i].id === id) {
                        return vm.products[i];
                    }
                }
            };

            vm.purchase = function (product) {
                var purchaseObj = {
                    'Id':0,
                    'ProductId': product.id,
                    'Amount': product.cost,
                    'IsSelled': false,
                    'Quantity':1,
                    'Date': Date.now()
                };

                debitBalance(product.cost).then(
                    function () {
                        putOperation(purchaseObj).then(function () {
                            trackBought(product.id);
                            updateUser();
                            alert("Purchase completed");
                        },function () {
                            alert("Something is wrong");
                        });
                    }, function () {
                        alert("Payment is wrong");
                    }
                );
            };

            vm.sellLastProduct = function (productId) {
                for (var i = vm.operations.length - 1; i >= 0; i--) {
                    if (vm.operations[i].ProductId === productId
                        && vm.operations[i].IsSelled == false) {
                        chargeBalance(vm.operations[i].Amount)
                            .then(function () {
                                vm.operations[i].IsSelled = true;
                                putOperation(vm.operations[i])
                                    .then(function () {
                                        trackSold(productId);
                                        updateUser();
                                        countProfit(productId, vm.operations[i].Amount);
                                        alert("Sell succeed!");
                                    },
                                    function () {
                                        alert("Sell invalid!");
                                    });
                            },function (error) {
                                alert("Error seling item: Returning money problem, " + error.Message);
                            });
                        return;
                    }
                }
            };                    
        }
    ]);
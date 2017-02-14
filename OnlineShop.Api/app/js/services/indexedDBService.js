var app = angular.module('ShopApp.Demo', ['ShopApp.Constants']);
app.factory('DBInterfaceApi', ['$window', '$q', function($window, $q) {
	var factory = {};

    factory.indexedDB = $window.indexedDB;
    factory.db = null;
    factory.lastIndex = 0;

    factory.open = function(){
    	var defer = $q.defer();
    	var version = 3;
    	var request = factory.indexedDB.open('operations_data', version);

    	request.onsuccess = function(e){
    		factory.db = e.target.result;
    		defer.resolve();
    	};
    	request.onerror = function(){
    		alert("Couldn't open database");
    		defer.reject();
    	};
    	request.onupgradeneeded = function(e) {
			factory.db = e.target.result;
			e.target.transaction.onerror = factory.indexedDB.onerror;
			if(factory.db.objectStoreNames.contains("operations")) {
				factory.db.deleteObjectStore("operations");
			}
			factory.initDb();
		};

    	return defer.promise;
    };

    factory.initDb = function(){
    	var defer = $q.defer();
    	var objectStore = factory.db.createObjectStore("operations", {keyPath:'Id'});
    	objectStore.createIndex('Id', 'Id', {unique:true});
    	objectStore.createIndex('ProductId', 'ProductId', {unique:false});
    	objectStore.createIndex('Amount', 'Amount', {unique:false});
    	objectStore.createIndex('IsSelled', 'IsSelled', {unique:false});
    	objectStore.createIndex('Quantity', 'Quantity', {unique:false});
    	objectStore.createIndex('Date', 'Date', {unique:false});
    	defer.resolve(true);

    	return defer.promise;
    };

    factory.getOperations = function(){
    	var deferred = $q.defer();
    	if(factory.db===null){
    		deferred.resolve("database not opened yet");
    	}else{
    		var transaction = factory.db.transaction(["operations"], "readwrite");
    		var store = transaction.objectStore("operations");
    		var operations = [];
    		var keyRange = IDBKeyRange.lowerBound(0);
    		var cursorRequest = store.openCursor(keyRange);

    		cursorRequest.onsuccess = function(e) {
				var result = e.target.result;
				if(!result){
					deferred.resolve(operations);
				}else{
					operations.push(result.value);
					if(result.value.Id > factory.lastIndex){
						factory.lastIndex=result.value.Id;
					}
					result.continue();
				}
			};
			cursorRequest.onerror = function(e){
				console.log(e.value);
				deferred.reject("can't get operations");
			};
    	}

    	return deferred.promise;
    };

    factory.putOperation = function(obj){
		var deferred = $q.defer();
		if(factory.db === null){
			deferred.reject("database is not opened yet!");
		}else{
			var trans = factory.db.transaction(["operations"], "readwrite");
			var store = trans.objectStore("operations");
			if(obj.Id === 0){
				factory.lastIndex++;		    
				obj.Id = factory.lastIndex;
			}
			var request = store.put(obj);
			request.onsuccess = function(e) {
				deferred.resolve();
			};
			request.onerror = function(e) {
				console.log(e.value);
				deferred.reject("operation not added");
			};
		}

		return deferred.promise;
	};

	return factory;
}]);
var app = angular.module('IndexedDBService', ['ShopApp.config']);
app.factory('DBInterfaceApi', [
    '$window', '$q', function($window, $q) {
        var indexedDB = $window.indexedDB;
        var db = null;
        var lastIndex = 0;

        var open = function(){
        	var defer = $q.defer();
        	var version = 1;
        	var request = indexedDB.open('operations_data', version);
        	request.onsuccess = function(e){
        		db = e.target.result;
        		defer.resolve();
        	}
        	request.onerror = function(){
        		alert("Couldn't open database");
        		defer.reject();
        	}
        	return defer.promise;
        };

        var getOperations = function(){
        	var defer = $q.defer();
        	if(db===null){
        		defer.resolve("database not opened yet");
        	}else{
        		var transaction = db.transaction(["operation"], "read");
        		var store = transaction.objectStore("operation");
        		var operations = [];
        		var keyRange = IDBKeyRange.lowerBound(0);
        		var cursorRequest = store.openCursor(keyRange);

        		cursorRequest.onsuccess = function(e) {
				      var result = e.target.result;
				      if(result === null || result === undefined)
				      {
				        defer.resolve(operations);
				      }
				      else{
				        operations.push(result.value);
				        if(result.value.id > lastIndex){
				          lastIndex=result.value.id;
				        }
				        result.continue();
				      }
    				};
    			cursorRequest.onerror = function(e){
				      console.log(e.value);
				      deferred.reject("can't get operations");
    				};
        	}
        	return defer.promise;
        };

        var addOperation = function(obj){
			  var deferred = $q.defer();
			   
			  if(db === null){
			    deferred.reject("database is not opened yet!");
			  }
			  else{
			    var trans = db.transaction(["operation"], "readwrite");
			    var store = trans.objectStore("operation");
			    lastIndex++;
			    var request = store.put({
			      "id": lastIndex,
			      "operation": obj
			    });
			   
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
    }
]);
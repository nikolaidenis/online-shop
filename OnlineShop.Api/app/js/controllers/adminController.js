(function() {
    angular.module("ShopApp")
    .controller('AdminController', ['$scope', '$window', 'UserApi', 'AuthApi', function ($scope, $window, UserApi, AuthApi) {

        $scope.accounts = [],
        $scope.filtered = [],
        $scope.currentPage = 1,
        $scope.numPerPage = 10,
        $scope.maxPaginationSize = 5;

        getAccounts();

        function getAccounts() {
            var accounts = UserApi.getAccounts()
                .then(function (response) {
                    $scope.accounts = $scope.accounts.concat(response.data);
                });
        };

        function countPage() {
            if ($scope.accounts.length != 0) {
                var begin = (($scope.currentPage - 1) * $scope.numPerPage),
                      end = begin + $scope.numPerPage;
                $scope.filtered = $scope.accounts.slice(begin, end);
            }
        }

        function changeLocking(account) {
            UserApi.changeUserLocking(account.username, !account.blocked)
                .then(function (response) {
                    account.blocked = !account.blocked;
                    alert('Successed!');
                }, function (error) {
                    alert('Bad');
                });
        }

        $scope.$watch("accounts", function () {
            countPage();
        });

        $scope.$watch("currentPage + numPerPage", function () {
            countPage();
        });

        $scope.logout = function () {
            AuthApi.logout();
            $window.location.reload();
        };
    }]);
})();

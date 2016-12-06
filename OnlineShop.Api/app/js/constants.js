var app = angular.module('ShopApp.config', []);
app.constant('AppVariables', {
    "base_url": "http://localhost:3315/api",
    "rowsCount": 5,
    "currentPage":1
});
app.constant('TempVariables', {
    "test_user": "1"
})
var AssetDashboardapp = angular.module('AssetDashboardapp', [])

AssetDashboardapp.controller('AssetDashboardController', function ($scope, $filter, $http) {
    // $scope.demo = "sandi";

    var d = new Date();
    console.log(d);
    $scope.historydate = $filter('date')(d, 'MM/dd/yyyy');

    // call apply asset page
    $scope.addAsset = function () {
        Pageredirect("/ApplyAsset/Index");
    }


});
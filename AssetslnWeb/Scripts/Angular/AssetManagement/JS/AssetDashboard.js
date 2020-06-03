var AssetDashboardapp = angular.module('AssetDashboardapp', ['CommonAppUtility'])

AssetDashboardapp.controller('AssetDashboardController', function ($scope, $filter, $http, CommonAppUtilityService) {
    // $scope.demo = "sandi";

    var d = new Date();
    console.log(d);
    $scope.historydate = $filter('date')(d, 'MM/dd/yyyy');

    // call apply asset page
    $scope.addAsset = function () {
        Pageredirect("/ApplyAsset/Index");
    }

    // call view page
    $scope.getViewData = function (id) {
        //sessionStorage.setItem("Lid", id);
        //Pageredirect("/AssetView/Index");

        var data = {
            'ID': id,
        }
        
        CommonAppUtilityService.CreateItem("/AssetView/getAssetId", data).then(function (response) {
            Pageredirect("/AssetView/Index");
        });
        
    }

    // call view page
    $scope.getEditData = function (id, internalstatus) {
        console.log(id);
        console.log(internalstatus);

        var data = {
            'ID': id,
        }

        if (internalstatus == "pendingatmanager") {
            CommonAppUtilityService.CreateItem("/ApproveAsset/getAssetId", data).then(function (response) {
                Pageredirect("/ApproveAsset/Index");
            });
        }
    }


});


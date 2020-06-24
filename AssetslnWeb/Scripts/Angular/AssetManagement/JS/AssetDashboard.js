var AssetDashboardapp = angular.module('AssetDashboardapp', ['CommonAppUtility'])

AssetDashboardapp.controller('AssetDashboardController', function ($scope, $filter, $http, CommonAppUtilityService) {

    // set current date
    var d = new Date();
    $scope.currentdate = $filter('date')(d, 'dd-MM-yyyy');
    console.log($scope.currentdate);

    // call apply asset page
    $scope.ApplyAsset = function () {
        var data = { 'ID': 1, }

        CommonAppUtilityService.CreateItem("/AssetDashboard/ApplyAssetView", data).then(function (response) {
            $('#ModalPopUp').modal('show');
            $(".modal-title").html('Apply For Asset');
            $(".modal-body").html(response.data);
        });
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

        if (internalstatus == "pendingatmanager" || internalstatus == "pendingatmanagerhead" || internalstatus == "pendingatdepartmenthead") {
            CommonAppUtilityService.CreateItem("/ApproveAsset/getAssetId", data).then(function (response) {
                Pageredirect("/ApproveAsset/Index");
            });
        }
        else if (internalstatus == "pendingallocated") {
            CommonAppUtilityService.CreateItem("/AllocateAsset/getAssetId", data).then(function (response) {
                Pageredirect("/AllocateAsset/Index");
            });
        }
        else if (internalstatus == "assetsallocated") {
            CommonAppUtilityService.CreateItem("/ReturnAsset/getAssetId", data).then(function (response) {
                Pageredirect("/ReturnAsset/Index");
            });
        }
    }


});


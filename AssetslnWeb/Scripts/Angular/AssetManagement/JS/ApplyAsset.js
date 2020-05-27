var ApplyAssetapp = angular.module('ApplyAssetapp', ['ApplyAssetServiceModule'])

ApplyAssetapp.controller('ApplyAssetController', function ($scope, $http, ApplyAssetService) {
   // $scope.demo = "sandi";

    $("#ddlAsset").change(function () {
        var selectedText = ($(this).find("option:selected").text()).trim();
        var selectedValue = $(this).val();
        console.log(selectedText);
        console.log(selectedValue);

        var AssetObj = {
            AssetType: selectedValue
        }

        ApplyAssetService.getAssetType(AssetObj).then(function (response) {
            console.log(response);
            $scope.AssetType = response.data;
        });
    });

    $scope.SubmitData = function ()
    {
        var ApplyAssetObj = {
            EmployeeCode: $scope.ngddlAllUser,
            Asset: $("#ddlAsset").val(),
            AssetType: $scope.ngddlAssetType,
            AssetCount: $scope.ngtxtAssetCount,
            Warranty: $scope.ngtxtWarranty,
            AssetDetails: $scope.ngtxtAssetDetails,
            ReasonToApply: $scope.ngtxtReasonToApply,
            RequestDate: $scope.ngtxtRequestDate,
            ReturnDate: $scope.ngtxtReturnDate
        }
        console.log(ApplyAssetObj);
        ApplyAssetService.getDemo().then(function (response) {
            
           // $('#modaldemo4').modal('show');
        });
    }

    $scope.PerformAction = function ()
    {
        alert('actionperfoem');
    }

});
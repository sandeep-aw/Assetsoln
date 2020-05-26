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

});
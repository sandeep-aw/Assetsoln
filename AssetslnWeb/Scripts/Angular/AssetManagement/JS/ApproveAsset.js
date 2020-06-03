var ApproveAssetapp = angular.module('ApproveAssetapp', ['CommonAppUtility'])

ApproveAssetapp.controller('ApproveAssetController', function ($scope, $filter, $http, CommonAppUtilityService) {

    // validate data and call submit data function
    $(function () {
        'use strict'
        //validation
        $('#ApproveAssetForm').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
            $('.bs-callout-info').toggleClass('hidden', !ok);
            $('.bs-callout-warning').toggleClass('hidden', ok);
        })
            .on('form:submit', function () {
                $scope.ApproveData();
                return false;
            });
    });

    // submit data
    $scope.ApproveData = function () {
        var ApplyAssetObj = {
            Comment: $scope.ngtxtComment,
        }

        console.log(ApplyAssetObj);

        CommonAppUtilityService.CreateItem("/ApproveAsset/ApproveFunc", ApplyAssetObj).then(function (response) {
        });
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

});
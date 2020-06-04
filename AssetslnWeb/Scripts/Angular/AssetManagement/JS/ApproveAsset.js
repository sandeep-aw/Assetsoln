var ApproveAssetapp = angular.module('ApproveAssetapp', ['CommonAppUtility'])

ApproveAssetapp.controller('ApproveAssetController', function ($scope, $filter, $http, CommonAppUtilityService) {

    var d = new Date();
    console.log(d);
    $scope.historydate = $filter('date')(d, 'MM/dd/yyyy');

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
            CurrentDate: $scope.historydate
        }

        console.log(ApplyAssetObj);

        CommonAppUtilityService.CreateItem("/ApproveAsset/ApproveFunc", ApplyAssetObj).then(function (response) {
            if (response.status == 200) {
                $('#modaldemo4').modal('show');
            } else {
                alert("Error");
            }
        });
    }

    $scope.PerformAction = function () {
        Pageredirect("/AssetDashboard/Index");
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

});
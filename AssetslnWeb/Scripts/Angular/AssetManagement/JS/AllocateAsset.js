var AllocateAssetapp = angular.module('AllocateAssetapp', ['CommonAppUtility'])

AllocateAssetapp.controller('AllocateAssetController', function ($scope, $filter, $http, CommonAppUtilityService) {

    var d = new Date();
    console.log(d);
    $scope.historydate = $filter('date')(d, 'MM/dd/yyyy');

    // validate data and call submit data function
    $(function () {
        'use strict'
        //validation
        $('#AllocateAssetForm').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
            $('.bs-callout-info').toggleClass('hidden', !ok);
            $('.bs-callout-warning').toggleClass('hidden', ok);
        })
            .on('form:submit', function () {
                $scope.AllocateData();
                return false;
            });
    });

    // submit data
    $scope.AllocateData = function () {
       // $("#global-loader").show();

        var ApplyAssetObj = {
            Quantity: $scope.AssignQty,
            CurrentDate: $scope.historydate
        }

        console.log(ApplyAssetObj);

        CommonAppUtilityService.CreateItem("/AllocateAsset/AllocateFunc", ApplyAssetObj).then(function (response) {
            if (response.status == 200) {
                //$("#global-loader").hide();
               // $('#modaldemo4').modal('show');
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
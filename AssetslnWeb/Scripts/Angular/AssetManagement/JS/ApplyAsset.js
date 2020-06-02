var ApplyAssetapp = angular.module('ApplyAssetapp', ['ApplyAssetServiceModule'])

ApplyAssetapp.controller('ApplyAssetController', function ($scope, $filter, $http, ApplyAssetService) {
   // $scope.demo = "sandi";

    var d = new Date();
    console.log(d);
    $scope.historydate = $filter('date')(d, 'MM/dd/yyyy');

    // create request no
    function getRequestNo() {
        var tday = new Date();
        var d = tday.getDate();
        var m = tday.getMonth() + 1;
        var y = tday.getFullYear();
        var hr = tday.getHours();
        var min = tday.getMinutes();
        var sec = tday.getMilliseconds();
        if (d < 10) {
            d = '0' + d;
        }
        if (m < 10) {
            m = '0' + m;
        }
        $scope.RequestNo = "AM" + "-" + y + m + d + hr + min + sec;
        console.log($scope.RequestNo);
    }

    // load function
    getRequestNo();


    $scope.getAsset = function()
    {
        /*var selectedText = ($(this).find("option:selected").text()).trim();
        var selectedValue = $(this).val();
        console.log(selectedText);
        console.log(selectedValue);*/

        var AssetObj = {
            AssetType: $scope.ngddlAsset
        }

        ApplyAssetService.getAssetType(AssetObj).then(function (response) {
            console.log(response);
            $scope.AssetType = response.data;
        });
    }


    // validate data and call submit data function
    $(function () {
        'use strict'
        //validation
        $('#ApplyAssetForm').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
            $('.bs-callout-info').toggleClass('hidden', !ok);
            $('.bs-callout-warning').toggleClass('hidden', ok);
        })
            .on('form:submit', function () {
                $scope.SubmitData();
                return false;
            });
    });


    // submit data
    $scope.SubmitData = function ()
    {
        var ApplyAssetObj = {
            RequestNo: $scope.RequestNo,
            EmployeeCode: $scope.ngddlAllUser,
            Asset: $scope.ngddlAsset,
            AssetType: $scope.ngddlAssetType,
            AssetCount: $scope.ngtxtAssetCount,
            Warranty: $scope.ngtxtWarranty,
            AssetDetails: $scope.ngtxtAssetDetails,
            ReasonToApply: $scope.ngtxtReasonToApply,
            RequestDate: moment($scope.ngtxtRequestDate, 'DD-MM-YYYY').format("MM/DD/YYYY"),
            ReturnDate: moment($scope.ngtxtReturnDate, 'DD-MM-YYYY').format("MM/DD/YYYY"),
            CurrentDate : $scope.historydate
        }
        console.log(ApplyAssetObj);
        ApplyAssetService.getDemo(ApplyAssetObj).then(function (response) {
            console.log(response);
            if (response.status == 200) {
                $('#modaldemo4').modal('show');
            } else {
                alert("Error");
            }
        });
    }

    $scope.PerformAction = function ()
    {
        Pageredirect("/AssetDashboard/Index");
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

});
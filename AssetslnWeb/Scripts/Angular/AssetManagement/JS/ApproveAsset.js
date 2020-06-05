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

    $scope.loading = true;

    // submit data
    $scope.ApproveData = function () {
        $("#global-loader").show();
        
        var ApplyAssetObj = {
            Comment: $scope.ngtxtComment,
            CurrentDate: $scope.historydate
        }

        console.log(ApplyAssetObj);

        CommonAppUtilityService.CreateItem("/ApproveAsset/ApproveFunc", ApplyAssetObj).then(function (response) {
            if (response.status == 200) {
                $("#global-loader").hide();
                $('#modaldemo4').modal('show');
            } else {
                alert("Error");
            }
        });
    }

    $scope.flag = false;

    $scope.RejectData = function () {
        
        $scope.flag = true;
        if ($scope.ngtxtComment == "" || typeof ($scope.ngtxtComment) === "undefined") {
            // error validation

            var message = "<ul class='parsley-errors-list filled' id='parsley-id-5'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
            $('#txtComment').parent().append(message);
            $('#txtComment').addClass("parsley-error");
        }
        else
        {
            $("#global-loader").show();
            // success validation

            $('#parsley-id-5').remove();
            $('#txtComment').removeClass("parsley-error");
            $('#txtComment').addClass("parsley-success");

            var ApplyAssetObj = {
                Comment: $scope.ngtxtComment,
                CurrentDate: $scope.historydate
            }

            console.log(ApplyAssetObj);

            CommonAppUtilityService.CreateItem("/ApproveAsset/RejectFunc", ApplyAssetObj).then(function (response) {
                if (response.status == 200) {
                    $("#global-loader").hide();
                    $('#modaldemo4').modal('show');
                } else {
                    alert("Error");
                }
            });
        }        
    }

    $scope.getkeys = function (comment) {
        if ($scope.flag == true) {
            if (comment != undefined) {
                $('#parsley-id-5').remove();
                $('#txtComment').removeClass("parsley-error");
                $('#txtComment').addClass("parsley-success");
            }
            else {
                var message = "<ul class='parsley-errors-list filled' id='parsley-id-5'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#txtComment').parent().append(message);
                $('#txtComment').addClass("parsley-error");
            }
        }          
    }

    $scope.PerformAction = function () {
        Pageredirect("/AssetDashboard/Index");
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

});
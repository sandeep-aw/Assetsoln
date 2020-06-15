var ApplyAssetapp = angular.module('ApplyAssetapp', ['ApplyAssetServiceModule','CommonAppUtility'])

ApplyAssetapp.controller('ApplyAssetController', function ($scope, $filter, $http, ApplyAssetService, CommonAppUtilityService) {
   // $scope.demo = "sandi";

    var d = new Date();
    console.log(d);
    $scope.historydate = $filter('date')(d, 'dd/MM/yyyy');
    $scope.ngtxtRequestDate = $filter('date')(d, 'dd/MM/yyyy');

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

    //add multiple assets
    $scope.AssetsArr = [];

    $scope.AddAssets = function () {
        var isValid = Validation();
        if (isValid) {

            var item = {
                Asset: $("#ddlAsset :selected").text().trim(),
                AssetId: $scope.ngddlAsset,
                AssetType: $("#ddlAssetType :selected").text().trim(),
                AssetTypeId: $scope.ngddlAssetType,
                UserApplyQuantity: $scope.ngtxtQty,
                ReasonToApply: $scope.ngtxtReason,
                AssetDetails: $scope.ngtxtDesc,
                Replacement: $scope.ngddlReplacement
            }
            console.log(item);

            //$('#ddlAsset option:selected').removeAttr('selected'); 
            //$("#slWrapper1").find('#ddlAsset option:selected').removeAttr('selected');
            //$("#ddlAsset")[0].selectedIndex = 0;
            //$("#ddlAssetType").empty();
            //$scope.ngtxtQty = "";
            //$scope.ngtxtReason = "";
            //$scope.ngtxtDesc = "";

            //$('#ddlAsset > option').prop("selected", false);
            //$("li.select2-selection__choice").remove();

            //$('#ddlAsset option').attr('selected', false);
           
            $('#AssetDrop').load('Index?SPHostUrl=' + getUrlVars()["SPHostUrl"] + ' #AssetDrop');
            //$('#AssetDropMain').load('#AssetDrop');
            
            //$('#ddlReplacement').val('')
            //$('#ddlReplacement').val(0);

            // $("#ddlReplacement option:selected").empty();
            // $("#ddlReplacement :selected").prop("selected", false)
            // $('#ddlReplacement option').attr('selected', false);
            // $("#ddlReplacement").empty();
            // $('#ddlReplacement').val('').trigger("change");

            $scope.AssetsArr.push(item);
        }
    }

    $scope.RemoveAssets = function (index) {
        $scope.AssetsArr.splice(index, 1);
    }

    // get approvers info
    $scope.ApproverData = [];
    $scope.showApprover = false;

    $scope.getApproverInfo = function (uid) {
        $scope.ApproverData.length = 0;

        var ApplyAssetObj = {
            EmpId: uid
        }

        console.log(ApplyAssetObj);

        CommonAppUtilityService.CreateItem("/ApplyAsset/GetApproveInfo", ApplyAssetObj).then(function (response) {      
            console.log(response);

            if (response != null) {
                var obj = response.data;
                for (var i = 0; i < obj.length; i++) {
                    $scope.ApproverData.push(obj[i].Title);
                }
                $scope.showApprover = true;
            }
            console.log($scope.ApproverData);
        });
    }

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
        $("#global-loader").show();

        //$scope.RequestDate = $filter('date')($scope.ngtxtRequestDate, 'mm/dd/yyyy');
        //$scope.RequestDate = moment($scope.ngtxtRequestDate, 'DD/MM/YYYY').format("MM/DD/YYYY");

        //$scope.ReturnDate = $filter('date')($scope.ngtxtReturnDate, 'mm/dd/yyyy');
        //$scope.ReturnDate = moment($scope.ngtxtReturnDate, 'DD/MM/YYYY').format("MM/DD/YYYY");

        var ApplyAssetObj = {
            RequestNo: $scope.RequestNo,
            EmployeeCode: $scope.ngddlAllUser,
            RequestDate: $scope.ngtxtRequestDate,
            ReturnDate: $scope.ngtxtReturnDate,
            CurrentDate: $scope.historydate,
            applyDetailsModel: $scope.AssetsArr,
            Comments: $scope.ngtxtComment
        }


        console.log(ApplyAssetObj);
        ApplyAssetService.getDemo(ApplyAssetObj).then(function (response) {
            console.log(response);
            if (response.status == 200) {
                $("#global-loader").hide();
                $('#modaldemo4').modal('show');
            } else {
                alert("Error");
            }
        });
    }

    $scope.PerformAction = function ()
    {
        $("#global-loader").show();
        Pageredirect("/AssetDashboard/Index");
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

    // date format set
    $('#requestdate').datetimepicker({
        minView: 2,
        format: 'dd/mm/yyyy',
        autoclose: true
    });

    $('#returndate').datetimepicker({
        minView: 2,
        format: 'dd/mm/yyyy',
        autoclose: true
    });

    // bind placeholder to dropdowns
    $scope.loadPlaceHolder = function () {
        $('.EmpSelect').select2({
            placeholder: 'Select Employee Name',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });

        $('.SelAsset').select2({
            placeholder: 'Select Asset',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });

        $('.SelAssetType').select2({
            placeholder: 'Select Asset Type',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });

        $('.SelReplacement').select2({
            placeholder: 'Select Replacement',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });
    }


    // load placeholder function
    $scope.loadPlaceHolder();

    // validation

    function clearErrorClass() {
        $('#parsley1').remove();
        $('#parsley2').remove();
        $('#parsley3').remove();
        $('#parsley4').remove();
        $('#parsley5').remove();
        $('#parsley6').remove();
        $('.validate').removeClass("parsley-error");
    }


    function Validation() {

        clearErrorClass();
        
        var retval = true;

        try {

            if ($scope.ngddlAsset == "" || typeof ($scope.ngddlAsset) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley1'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#ddlAsset').parent().append(message);
                $('#ddlAsset').addClass("parsley-error");
                retval = false;
            }

            if ($scope.ngddlAssetType == "" || typeof ($scope.ngddlAssetType) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley2'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#ddlAssetType').parent().append(message);
                $('#ddlAssetType').addClass("parsley-error");
                retval = false;
            }

            if ($scope.ngtxtQty == "" || typeof ($scope.ngtxtQty) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley3'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#txtQty').parent().append(message);
                $('#txtQty').addClass("parsley-error");
                retval = false;
            }

            if ($scope.ngtxtReason == "" || typeof ($scope.ngtxtReason) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley4'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#txtReason').parent().append(message);
                $('#txtReason').addClass("parsley-error");
                retval = false;
            }

            if ($scope.ngtxtDesc == "" || typeof ($scope.ngtxtDesc) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley5'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#txtDesc').parent().append(message);
                $('#txtDesc').addClass("parsley-error");
                retval = false;
            }

            if ($scope.ngddlReplacement == "" || typeof ($scope.ngddlReplacement) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley6'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#ddlReplacement').parent().append(message);
                $('#ddlReplacement').addClass("parsley-error");
                retval = false;
            }

            return retval;
        }

        catch (err) {
            console.log(err.message);
            retval = false;
        }
    }

});
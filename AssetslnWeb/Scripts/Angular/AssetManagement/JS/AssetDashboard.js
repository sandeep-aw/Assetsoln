var AssetDashboardapp = angular.module('AssetDashboardapp', ['CommonAppUtility'])

AssetDashboardapp.controller('AssetDashboardController', function ($scope, $rootScope, $filter, $http, CommonAppUtilityService) {

    // set current date
    var d = new Date();
    var currentDate = $filter('date')(d, 'dd-MM-yyyy');
    console.log(currentDate);

    // call apply asset page
    ApplyAsset = function () {
            $("#showApprover").hide();
            //$scope.ngtxtRequestDate = currentDate;
            $("#requestdate").val(currentDate);

            $('#returndate').datetimepicker({
                minView: 2,
                format: 'dd-mm-yyyy',
                autoclose: true
            });        

            $scope.loadPlaceholders();
            $(".modal-dialog").width("70%");
            $('#ModalPopUp').modal('show');
    }

    // load select2 placeholders
    $scope.loadPlaceholders = function () {
        $('#EmpSelect').select2({
            placeholder: 'Select Employee Name',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });
        $('#ddlAsset').select2({
            placeholder: 'Select Asset',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });
        $('#ddlAssetType').select2({
            placeholder: 'Select Asset Type',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });
        $('#ddlReplacement').select2({
            placeholder: 'Replacement',
            searchInputPlaceholder: 'Search',
            width: '100%'
        });
    }

    // get approvers info
    getApproverInfo = function () {
        $("#approverData").html("");
        var ApplyAssetObj = {
            EmpId: $("#EmpSelect").val()
        }

        console.log(ApplyAssetObj);

        CommonAppUtilityService.CreateItem("/AssetDashboard/GetApproveInfo", ApplyAssetObj).then(function (response) {
            console.log(response);
            if (response != null) {
                var obj = response.data;
                $.each(obj, function (i, item) {
                    $("#approverData").append('<li><a href="#">'+item.Title+'</a></li>');
                });
                $("#showApprover").show();
            }
        });
    }

    // get asset type data
    getAsset = function () {
        $("#ddlAssetType").html("");
        var AssetObj = {
            AssetType: $("#ddlAsset").val()
        }

        CommonAppUtilityService.CreateItem("/AssetDashboard/GetAssetType", AssetObj).then(function (response) {
            console.log(response);
            if (response != null) {
                $scope.AssetType = response.data;
            }
        });
    }

    // add multiple assets in array
    $scope.AssetsArr = [];

    $scope.AddAssets = function () {
        var isValid = ValidationApplyAsset();
        if (isValid) {
            var item = {
                Asset: $("#ddlAsset :selected").text().trim(),
                AssetId: $("#ddlAsset :selected").val(),
                AssetType: $("#ddlAssetType :selected").text().trim(),
                AssetTypeId: $("#ddlAssetType :selected").val(),
                UserApplyQuantity: $("#txtQty").val(),
                ReasonToApply: $("#txtReason").val(),
                AssetDetails: $("#txtDesc").val(),
                Replacement: $("#ddlReplacement").val()
            }
            console.log(item);
            $scope.AssetsArr.push(item);
            $scope.clrAssetArr();
        }
    }

    $scope.clrAssetArr = function () {

        //clear values
        $("#select2-ddlAsset-container").html('<span class="select2-selection__placeholder">Select Asset</span>');
        $("#select2-ddlAsset-container").removeAttr("title");

        $("#select2-ddlAssetType-container").html('<span class="select2-selection__placeholder">Select Asset Type</span>');
        $("#select2-ddlAssetType-container").removeAttr("title");
        $("#select2-ddlAssetType-results").empty();

        $scope.ngtxtQty = "";
        $scope.ngtxtReason = "";
        $scope.ngtxtDesc = "";

        $("#select2-ddlReplacement-container").html('<span class="select2-selection__placeholder">Replacement</span>');
        $("#select2-ddlReplacement-container").removeAttr("title");
    }

    $scope.RemoveAssets = function (index) {
        $scope.AssetsArr.splice(index, 1);
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
                if ($scope.AssetsArr.length > 0) {
                    $scope.SubmitData();
                }
                else {
                    alert('Please add asset details');
                }
                return false;
            });
    });

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

    // submit data
    $scope.SubmitData = function () {

        var ApplyAssetObj = {
            RequestNo: $scope.RequestNo,
            EmployeeName: $scope.ngddlAllUser,
            RequestDate: $("#requestdate").val(),
            ReturnDate: $scope.ngtxtReturnDate,
            CurrentDate: currentDate,
            applyDetailsModel: $scope.AssetsArr,
            Comments: $scope.ngtxtComment
        }

        console.log(ApplyAssetObj);
        CommonAppUtilityService.CreateItem("/AssetDashboard/SaveAssetData", ApplyAssetObj).then(function (response) {
            console.log(response);

             //get response data
            var responsearr = JSON.parse(response.config.data);

            console.log(responsearr);

            if (response.status == 200) {
                $scope.clrApplyAsset();

                $('#ModalPopUp').modal('hide');

                $('#AssetDashboardModel').html(response.data);

                $scope.ReqNumber = responsearr.RequestNo;

                $scope.message = "Asset Request with Request No: " + $scope.ReqNumber + " is Successfully Submitted.";
                notif({
                    msg: $scope.message,
                    type: "success"
                });
            } else {
                alert("Error");
            }
        });
    }

    // clear apply asset data
    $scope.clrApplyAsset  = function()
    {
        $scope.ngddlAllUser = "";
        $scope.ngtxtReturnDate = "";
        $scope.ngtxtComment = "";
        $scope.AssetsArr.length = 0;
        $scope.ngddlAsset = "";
        $scope.ngddlAssetType = "";
        $scope.ngddlReplacement = "";

        $("#EmpSelect").select2("destroy");
        $("#ddlAsset").select2("destroy");
        $("#ddlAssetType").select2("destroy");
        $("#ddlReplacement").select2("destroy");

        Array.from(document.getElementsByClassName('parsley-success')).forEach(function (el) {
            el.classList.remove('parsley-success');
        });
    }

    // call view page
    getViewData = function (id, action) {
        var path = "";

        var data = {
            ID: id
        }

        if (action == "view") {
            path = "/AssetDashboard/GetAssetView";
        }

        CommonAppUtilityService.CreateItem(path, data).then(function (response) {
            console.log(response);
            $(".modal-dialog").width("auto");
            $('#ModalViewPopUp').html(response.data);
            $("#ModalViewPopUp").modal('show');
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


    // validation functions

    function clearErrorClass() {
        $('#parsley1').remove();
        $('#parsley2').remove();
        $('#parsley3').remove();
        $('#parsley4').remove();
        $('#parsley5').remove();
        $('#parsley6').remove();
        $('#parsley7').remove();
        $('.validate').removeClass("parsley-error");
    }

    function ValidationApplyAsset() {

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
            else {
                if (isNaN($scope.ngtxtQty) && ($scope.ngtxtQty != "" || $scope.ngtxtQty != "undefined")) {
                    var message = "<ul class='parsley-errors-list filled' id='parsley7'><li class='parsley-required'>" + "Please Enter Numeric Value Only." + "</li></ul>"
                    $('#txtQty').parent().append(message);
                    $('#txtQty').addClass("parsley-error");
                    retval = false;
                }
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

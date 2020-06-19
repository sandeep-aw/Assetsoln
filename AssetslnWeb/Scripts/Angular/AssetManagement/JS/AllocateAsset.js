var AllocateAssetapp = angular.module('AllocateAssetapp', ['CommonAppUtility'])

AllocateAssetapp.controller('AllocateAssetController', function ($scope, $filter, $http, CommonAppUtilityService) {

    var d = new Date();
    var todaydate = $filter('date')(d, 'yyyy-MM-dd');
    console.log(todaydate);

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
                if ($scope.assetInfo.length > 0) {
                    $scope.AllocateData();
                }
                else {
                    alert('Please add asset details');
                }
                return false;
            });
    });


    $scope.srno = "0";

    // set id on click of add button
    $scope.flag = "new";

 
    $scope.PrevAssignQty = 0;
    $scope.userQty = 0;
    

    $(document).on("click", ".popup", function () {
        $scope.tempPrevAssignQty = 0;
        $scope.tempPendingAssignQty = 0;

        $("#txtPrevAssignQty").val(0);
        $("#txtPendingAssignQty").val(0);

        $scope.srno = $(this).data('id');
        $scope.userQty = $(this).data('qty');
        $scope.asset = $(this).data('obj');
        $scope.assetType = $(this).data('item');
        $scope.tempPrevAssignQty = $(this).data('allotqty');

        if ($scope.tempPrevAssignQty != "") {
            $("#txtPrevAssignQty").val($scope.tempPrevAssignQty);
            $scope.PrevAssignQty = $scope.tempPrevAssignQty;
        }

        $scope.tempPendingAssignQty = $(this).data('balqty');

        if ($scope.tempPendingAssignQty != "") {
            $("#txtPendingAssignQty").val($scope.tempPendingAssignQty);
        } 

        if ($scope.assetInfo.length > 0) {
            for (var i = 0; i < $scope.assetInfo.length; i++) {
                if ($scope.assetInfo[i].SrNo == $scope.srno) {
                    $scope.bindData($scope.assetInfo[i]);
                    $scope.flag = "update";
                }
            }
        }

        $('#scrollmodal').modal('show');

        //$(".modal-body #bookId").val(myBookId);
        //$('#addBookDialog').modal('show');
        //alert(myBookId);
    });

    // savedata function of popup
    $scope.assetInfo = [];

    
    $scope.SaveData = function () {
        console.log('step1');
        var isValid = Validation();
        if (isValid) {
            console.log('step2');
            $('#scrollmodal').modal('hide');
            if ($scope.flag == "new") {
                var item = {
                    SrNo: $scope.srno,
                    UserQty: $scope.userQty,
                    asset: $scope.asset,
                    assetType: $scope.assetType,
                    AssetQuantity: $("#txtAssignQty").val(),
                    PrevAssignQty: $("#txtPrevAssignQty").val(),
                    PendingQuantity: $("#txtPendingAssignQty").val(),
                    ProdSrNo: $("#txtProdSrNo").val(),
                    ModelNo: $("#txtModelNo").val(),
                    Mon: $("#txtMon").val(),
                    WarrantyDate: $("#txtwarrantydate").val(),
                    Remark: $("#txtRemark").val()
                }
                $scope.assetInfo.push(item);
                $scope.clrData();
                console.log($scope.assetInfo);
            }
            else {
                var i = $scope.assetInfo.findIndex(x => x.SrNo === $scope.srno);

                $scope.assetInfo[i].SrNo = $scope.srno;
                $scope.assetInfo[i].AssetQuantity = $("#txtAssignQty").val();
                $scope.assetInfo[i].PrevAssignQty = $("#txtPrevAssignQty").val();
                $scope.assetInfo[i].PendingQuantity = $("#txtPendingAssignQty").val();
                $scope.assetInfo[i].ProdSrNo = $("#txtProdSrNo").val();
                $scope.assetInfo[i].ModelNo = $("#txtModelNo").val();
                $scope.assetInfo[i].Mon = $("#txtMon").val();
                $scope.assetInfo[i].Remark = $("#txtRemark").val();
                $scope.assetInfo[i].WarrantyDate = $("#txtwarrantydate").val();
                console.log($scope.assetInfo);
                $scope.clrData();
            }
        }
    }

    // clear add data in array
    $scope.clrData = function () {
        $("#txtAssignQty").val("");
        $("#txtPrevAssignQty").val(0);
        $("#txtPendingAssignQty").val(0);
        $("#txtProdSrNo").val("");
        $("#txtModelNo").val("");
        //$("#txtMon").val("");
        $("#txtRemark").val("");
        $("#txtwarrantydate").val("");
    }

    $scope.bindData = function (obj) {
        //$scope.SrNo = obj.SrNo;
        $("#txtAssignQty").val(obj.AssetQuantity);
        $("#txtwarrantydate").val(obj.WarrantyDate);
        $("#txtPrevAssignQty").val(obj.PrevAssignQty);
        $("#txtPendingAssignQty").val(obj.PendingQuantity);
        $("#txtProdSrNo").val(obj.ProdSrNo);
        $("#txtModelNo").val(obj.ModelNo);
        //$("#txtMon").val(obj.Mon);
        $("#txtRemark").val(obj.Remark);
        console.log($scope.assetInfo);
    }

    $scope.chkerrflag = true;

    $scope.getPendingQty = function () {
        $scope.tempqty = $("#txtAssignQty").val();
        if ($scope.tempqty != "") {           
            $scope.totalqty = $scope.PrevAssignQty + parseInt($scope.tempqty);

            if ($scope.totalqty > $scope.userQty) {
                var message = "<ul class='parsley-errors-list filled' id='parsley1'><li class='parsley-required'>" + "Please enter correct assign quantity." + "</li></ul>"
                $('#txtAssignQty').parent().append(message);
                $('#txtAssignQty').addClass("parsley-error");
                $scope.chkerrflag = false;
            }
            else {
                $scope.tempPendingQty = $scope.userQty - parseInt($scope.totalqty);
                $("#txtPendingAssignQty").val($scope.tempPendingQty);
                alert('Pending Assign quantity is ' + $scope.tempPendingQty);
                $('#parsley1').remove();
                $('#txtAssignQty').removeClass("parsley-error");
                $('#txtAssignQty').addClass("parsley-success");
                $scope.chkerrflag = true;
            }
        }
        else {
            $('#txtAssignQty').addClass("parsley-error");
            $('#txtAssignQty').removeClass("parsley-success");
            var message = "<ul class='parsley-errors-list filled' id='parsley1'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
            $('#txtAssignQty').parent().append(message);
            $('#txtAssignQty').addClass("parsley-error");
        }
    }

    $scope.Close = function () {
        clearErrorClass();
        $scope.clrData();
    }

    // date format set
    $('#txtwarrantydate').datetimepicker({
        minView: 2,
        format: 'dd-mm-yyyy',
        autoclose: true
    });

    //call function when date is selected
    //$("#txtwarrantydate").on("change", function () {
    //    //alert('date');
    //    console.log($("#txtwarrantydate").val());
    //    var tempdate = $("#txtwarrantydate").val();
    //    //var formatteddate = $filter('date')(tempdate, 'yyyy-MM-dd');
    //    var formatteddate = moment(tempdate, 'DD-MM-YYYY').format("YYYY-MM-DD");
        
    //    var diff = dateDiffInDays(todaydate, formatteddate);
    //    $("#txtMon").val(diff);
    //    console.log(diff);
    //})

    // calculate date difference
    //function dateDiffInDays(a, b) {
    //    const _MS_PER_DAY = 1000 * 60 * 60 * 24;

    //    // Discard the time and time-zone information.
    //    var date2 = new Date(b);
    //    var date1 = new Date(a);
    //    const utc1 = Date.UTC(date1.getFullYear(), date1.getMonth(), date1.getDate());
    //    const utc2 = Date.UTC(date2.getFullYear(), date2.getMonth(), date2.getDate());

    //    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
    //}


    // submit data
    $scope.AllocateData = function () {
        // $("#global-loader").show();

        var ApplyAssetObj = {
            todaydate: todaydate,
            comments: $scope.ngtxtComment,
            assetAllotmentHistoryModels: $scope.assetInfo
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

    function clearErrorClass() {
        $('#parsley1').remove();
        $('.validate').removeClass("parsley-error");
    }

    //validation
    function Validation() {

        if ($scope.chkerrflag == true) {
            clearErrorClass();
        }

        var retval = true;

        try {
            var assignqty = $("#txtAssignQty").val();

            retval = $scope.chkerrflag;

            if (assignqty == "" || typeof (assignqty) === "undefined") {
                var message = "<ul class='parsley-errors-list filled' id='parsley1'><li class='parsley-required'>" + "This value is required." + "</li></ul>"
                $('#txtAssignQty').parent().append(message);
                $('#txtAssignQty').addClass("parsley-error");
                retval = false;
            }

            return retval;
        }

        catch (err) {
            console.log(err.message);
            retval = false;
        }
    }

    $scope.PerformAction = function () {
        Pageredirect("/AssetDashboard/Index");
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

});
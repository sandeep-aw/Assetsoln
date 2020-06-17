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

    $scope.srno = "0";

    // set id on click of add button
    $scope.flag = "new";

    $(document).on("click", ".popup", function () {
        
        $scope.srno = $(this).data('id');
        $scope.userQty = $(this).data('qty');

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
        if ($scope.flag == "new") {
            var item = {
                SrNo: $scope.srno,
                AssignQty: $("#txtAssignQty").val(),
                PrevAssignQty: $("#txtPrevAssignQty").val(),
                PendingAssignQty: $("#txtPendingAssignQty").val(),
                ProdSrNo: $("#txtProdSrNo").val(),
                ModelNo: $("#txtModelNo").val(),
                Mon: $("#txtMon").val(),
                Date: $("#txtwarrantydate").val(),
                Remark: $("#txtRemark").val()
            }
            $scope.assetInfo.push(item);
            $scope.clrData();
            console.log($scope.assetInfo);
        }
        else {
            var i = $scope.assetInfo.findIndex(x => x.SrNo === $scope.srno);

            $scope.assetInfo[i].SrNo = $scope.srno;
            $scope.assetInfo[i].AssignQty = $("#txtAssignQty").val();
            $scope.assetInfo[i].PrevAssignQty = $("#txtPrevAssignQty").val();
            $scope.assetInfo[i].PendingAssignQty = $("#txtPendingAssignQty").val();
            $scope.assetInfo[i].ProdSrNo = $("#txtProdSrNo").val();
            $scope.assetInfo[i].ModelNo = $("#txtModelNo").val();
            $scope.assetInfo[i].Mon = $("#txtMon").val();
            //Date: $scope.ngtxtDate,
            $scope.assetInfo[i].Remark = $("#txtRemark").val();
            $scope.assetInfo[i].Date = $("#txtwarrantydate").val();
            console.log($scope.assetInfo);
            $scope.clrData();
        }
        
    }

    // clear add data in array
    $scope.clrData = function () {
        $("#txtAssignQty").val("");
        $("#txtPrevAssignQty").val("");
        $("#txtPendingAssignQty").val("");
        $("#txtProdSrNo").val("");
        $("#txtModelNo").val("");
        $("#txtMon").val("");
        $("#txtRemark").val("");
        $("#txtwarrantydate").val("");
    }

    $scope.bindData = function (obj) {
        $scope.SrNo = obj.SrNo;
        $("#txtAssignQty").val(obj.AssignQty);
        $("#txtwarrantydate").val(obj.Date);
        $("#txtPrevAssignQty").val(obj.PrevAssignQty);
        $("#txtPendingAssignQty").val(obj.PendingAssignQty);
        $("#txtProdSrNo").val(obj.ProdSrNo);
        $("#txtModelNo").val(obj.ModelNo);
        $("#txtMon").val(obj.Mon);
        $("#txtRemark").val(obj.Remark);
        console.log($scope.assetInfo);
    }

    $scope.ngtxtPrevAssignQty = 0;
    $scope.ngtxtPendingAssignQty = 0;
    $scope.userQty = 0;

    $scope.getPendingQty = function () {
        $scope.tempqty = $("#txtAssignQty").val();
        $scope.tempPendingQty = $scope.userQty - $scope.tempqty;
        $("#txtPendingAssignQty").val($scope.tempPendingQty);
    }

    $scope.Close = function () {
        $scope.clrData();
    }

    // date format set
    $('#txtwarrantydate').datetimepicker({
        minView: 2,
        format: 'dd-mm-yyyy',
        autoclose: true
    });

    //call function when date is selected
    $scope.getDays = function () {
        var diffdays = $scope.days($("#txtwarrantydate").val());
    }

    // calculate date difference
    $scope.days = function (date) {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd
        }
        if (mm < 10) {
            mm = '0' + mm
        }
        today = yyyy + '/' + mm + '/' + dd;
        $scope.today = today;
        var date2 = new Date(today);
        var date1 = new Date(date);
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
        return $scope.dayDifference;
    }

    $scope.PerformAction = function () {
        Pageredirect("/AssetDashboard/Index");
    }

    $scope.Cancel = function () {
        Pageredirect("/AssetDashboard/Index");
    }

});
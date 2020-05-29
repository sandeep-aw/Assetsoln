var ApplyAssetServiceModule = angular.module('ApplyAssetServiceModule', ['CommonAppUtility']);

var jsonheaders = { 'headers': { 'accept': 'application/json;odata=verbose' } };

ApplyAssetServiceModule.service('ApplyAssetService', function ($http, $q, CommonAppUtilityService) {

    this.getAssetType = function (option) {
        console.log(option);
        
        return CommonAppUtilityService.CreateItem("/ApplyAsset/GetAssetType", option);
    }

    this.getDemo = function (option) {
        /*var deferred = $q.defer();
        //alert("started");
        var obj = "get data";
        deferred.resolve(obj);
        return deferred.promise;*/

        return CommonAppUtilityService.CreateItem("/ApplyAsset/SaveAssetData", option);
    }

});
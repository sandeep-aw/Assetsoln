var ApplyAssetServiceModule = angular.module('ApplyAssetServiceModule', ['CommonAppUtility']);

var jsonheaders = { 'headers': { 'accept': 'application/json;odata=verbose' } };

ApplyAssetServiceModule.service('ApplyAssetService', function ($http, CommonAppUtilityService) {

    this.getAssetType = function (option) {
        console.log(option);
        
        return CommonAppUtilityService.CreateItem("/ApplyAsset/GetAssetType", option);
    }

});
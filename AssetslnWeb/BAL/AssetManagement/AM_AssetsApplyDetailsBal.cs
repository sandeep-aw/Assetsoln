using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL.AssetManagement
{
    public class AM_AssetsApplyDetailsBal
    {
        public string SaveAssetsApplyDetails(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        private JArray RESTGet(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            string selectqry = "ID,LID/ID,Asset/Id,Asset/Assets,AssetType/Id,AssetType/AssetType,AssetType/Stock,UserApplyQuantity,AssetDetails,ReasonToApply,";
            selectqry += "Replacement,UserAllottedtQuantity,UserReturnQuantity,UserBalanceQuantity";

            rESTOption.select = selectqry;
            rESTOption.expand = "LID,Asset,AssetType";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetsApplyDetails", rESTOption);

            return jArray;
        }

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetsApplyDetails", ItemData);
        }
    }
}
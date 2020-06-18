using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using AssetslnWeb.Models.AssetManagement;
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
        public List<AM_AssetsApplyDetailsModel> GetDetailsById(ClientContext clientContext, int id)
        {
            List<AM_AssetsApplyDetailsModel> assetsApplyDetailsBals = new List<AM_AssetsApplyDetailsModel>();

            string filter = "LID eq '" + id + "'";

            JArray jArray = RESTGet(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetsApplyDetailsBals.Add(new AM_AssetsApplyDetailsModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    UserApplyQuantity = j["UserApplyQuantity"] == null ? "" : Convert.ToString(j["UserApplyQuantity"]),
                    UserAllottedQuantity = j["UserAllottedtQuantity"] == null ? "" : Convert.ToString(j["UserAllottedtQuantity"]),
                    UserReturntQuantity = j["UserReturnQuantity"] == null ? "" : Convert.ToString(j["UserReturnQuantity"]),
                    UserBalancetQuantity = j["UserBalanceQuantity"] == null ? "" : Convert.ToString(j["UserBalanceQuantity"]),
                    AssetDetails = j["AssetDetails"] == null ? "" : Convert.ToString(j["AssetDetails"]),
                    ReasonToApply = j["ReasonToApply"] == null ? "" : Convert.ToString(j["ReasonToApply"]),
                    AssetId = j["Asset"]["Id"] == null ? "" : Convert.ToString(j["Asset"]["Id"]),
                    Asset = j["Asset"]["Assets"] == null ? "" : Convert.ToString(j["Asset"]["Assets"]),
                    AssetTypeId = j["AssetType"]["Id"] == null ? "" : Convert.ToString(j["AssetType"]["Id"]),
                    AssetType = j["AssetType"]["AssetType"] == null ? "" : Convert.ToString(j["AssetType"]["AssetType"]),
                    Replacement = j["Replacement"] == null ? "" : Convert.ToString(j["Replacement"]),
                    AssetTypeStock = j["AssetType"]["Stock"] == null ? "" : Convert.ToString(j["AssetType"]["Stock"]),
                });
            }


            return assetsApplyDetailsBals;
        }

        public string SaveAssetsApplyDetails(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        public string UpdateAssetsDetails(ClientContext clientContext, string ItemData, string ID)
        {

            string response = RESTUpdate(clientContext, ItemData, ID);

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

        private string RESTUpdate(ClientContext clientContext, string ItemData, string ID)
        {
            RestService restService = new RestService();

            return restService.UpdateItem(clientContext, "AM_AssetsApplyDetails", ItemData, ID);
        }
    }
}
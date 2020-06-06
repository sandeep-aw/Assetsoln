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
    public class AM_AssetCountHistoryBal
    {
        public List<AM_AssetCountHistoryModel> GetAssetCountHistory(ClientContext clientContext, string id)
        {
            List<AM_AssetCountHistoryModel> assetCountHistoryModels = new List<AM_AssetCountHistoryModel>();

            string filter = "LID/ID eq '" + id + "'";

            JArray jArray = RestGetCount(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetCountHistoryModels.Add(new AM_AssetCountHistoryModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    ActionType = j["ActionType"] == null ? "" : Convert.ToString(j["ActionType"]),
                    AssetCount = j["ActionTaken"] == null ? "" : Convert.ToString(j["ActionTaken"]),
                    LID = j["LID"]["ID"] == null ? "" : Convert.ToString(j["LID"]["ID"]),
                    AssetCountID = j["LID"]["AssetCount"] == null ? "" : Convert.ToString(j["LID"]["AssetCount"])
                });
            }

            return assetCountHistoryModels;
        }

        public string SaveAssetCountHistory(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        private JArray RestGetCount(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,ActionType,AssetCount,LID/ID,LID/AssetCount";
            rESTOption.expand = "LID";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetCountHistory", rESTOption);

            return jArray;
        }

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetCountHistory", ItemData);
        }
    }
}
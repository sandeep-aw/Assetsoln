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
    public class AM_AssetAllotmentHistoryBal
    {
        public List<AM_AssetAllotmentHistoryModel> GetHistoryById(ClientContext clientContext, int id)
        {
            List<AM_AssetAllotmentHistoryModel> assetAllotmentHistoryModels = new List<AM_AssetAllotmentHistoryModel>();

            string filter = "LID/ID eq '" + id + "'";

            JArray jArray = RESTGet(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetAllotmentHistoryModels.Add(new AM_AssetAllotmentHistoryModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    WarrantyDate = Convert.ToDateTime(j["WarrantyDate"]),
                    LID = j["LID"]["ID"] == null ? "" : Convert.ToString(j["LID"]["ID"]),
                    UserApplyQuantity = j["LID"]["UserApplyQuantity"] == null ? "" : Convert.ToString(j["LID"]["UserApplyQuantity"]),
                    UserAllottedtQuantity = j["LID"]["UserAllottedtQuantity"] == null ? "" : Convert.ToString(j["LID"]["UserAllottedtQuantity"]),
                    UserReturnQuantity = j["LID"]["UserReturnQuantity"] == null ? "" : Convert.ToString(j["LID"]["UserReturnQuantity"]),
                    UserBalanceQuantity = j["LID"]["UserBalanceQuantity"] == null ? "" : Convert.ToString(j["LID"]["UserBalanceQuantity"]),
                    ActionType = j["ActionType"] == null ? "" : Convert.ToString(j["ActionType"]),
                    AssetQuantity = j["AssetQuantity"] == null ? "" : Convert.ToString(j["AssetQuantity"]),
                    PendingQuantity = j["PendingQuantity"] == null ? "" : Convert.ToString(j["PendingQuantity"]),
                    ProdSrNo = j["ProdSrNo"] == null ? "" : Convert.ToString(j["ProdSrNo"]),
                    ModelNo = j["ModelNo"] == null ? "" : Convert.ToString(j["ModelNo"]),
                    Remark = j["Remark"] == null ? "" : Convert.ToString(j["Remark"]),
                });
            }

            return assetAllotmentHistoryModels;
        }

        public string SaveAllotmentHistory(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        private JArray RESTGet(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            string selectqry = "ID,LID/ID,LID/UserApplyQuantity,LID/UserAllottedtQuantity,LID/UserReturnQuantity,LID/UserBalanceQuantity,ActionType,AssetQuantity,";
            selectqry += "PendingQuantity,ProdSrNo,ModelNo,WarrantyDate,Remark";

            rESTOption.select = selectqry;
            rESTOption.expand = "LID";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetAllotmentHistory", rESTOption);

            return jArray;
        }

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetAllotmentHistory", ItemData);
        }

    }
}
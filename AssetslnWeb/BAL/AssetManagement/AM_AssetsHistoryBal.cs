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

    public class AM_AssetsHistoryBal
    {
        public List<AM_AssetsHistoryModel> GetHistoryById(ClientContext clientContext, int id)
        {
            List<AM_AssetsHistoryModel> assetsHistoryModels = new List<AM_AssetsHistoryModel>();

            string filter = "LID eq '" + id + "'";

            JArray jArray = RESTGet(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetsHistoryModels.Add(new AM_AssetsHistoryModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    CurrentDate = j["Date"] == null ? "" : Convert.ToString(j["Date"]),
                    Comments = j["Comments"] == null ? "" : Convert.ToString(j["Comments"]),
                    ActionTakenId = j["ActionTaken"]["Id"] == null ? "" : Convert.ToString(j["ActionTaken"]["Id"]),
                    ActionTaken = j["ActionTaken"]["EmpCode"] == null ? "" : Convert.ToString(j["ActionTaken"]["EmpCode"]),
                    StatusId = j["Status"]["Id"] == null ? "" : Convert.ToString(j["Status"]["Id"]),
                    Status = j["Status"]["StatusName"] == null ? "" : Convert.ToString(j["Status"]["StatusName"]),
                    LID = j["LID"]["ID"] == null ? "" : Convert.ToString(j["LID"]["ID"])
                });
            }


            return assetsHistoryModels;
        }

        private JArray RESTGet(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,ActionTaken/Id,ActionTaken/EmpCode,Status/Id,Status/StatusName,LID/ID,Date,Comments";
            rESTOption.expand = "ActionTaken,Status,LID";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetsHistory", rESTOption);

            return jArray;
        }

        public string SaveAssetsHistoryData(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetsHistory", ItemData);
        }
    }
}
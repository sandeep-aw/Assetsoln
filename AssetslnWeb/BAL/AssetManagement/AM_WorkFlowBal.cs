using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using AssetslnWeb.Models.AssetManagement;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL.AssetManagement
{
    public class AM_WorkFlowBal
    {
        public List<AM_WorkFlowModel> getWorkFlowData(ClientContext clientContext, string actionType)
        {
            List<AM_WorkFlowModel> workFlowBal = new List<AM_WorkFlowModel>();

            string filter = "ActionType eq '" + actionType + "'";

            JArray jArray = RestGetWorkFlow(clientContext, filter);

            foreach (JObject j in jArray)
            {
                workFlowBal.Add(new AM_WorkFlowModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    ActionType = j["ActionType"] == null ? "" : Convert.ToString(j["ActionType"]),
                    InternalStatus = j["InternalStatus"] == null ? "" : Convert.ToString(j["InternalStatus"]),
                    StatusId = j["Status"]["Id"] == null ? "" : Convert.ToString(j["Status"]["Id"]),
                    Status = j["Status"]["StatusName"] == null ? "" : Convert.ToString(j["Status"]["StatusName"])
                });
            }

            return workFlowBal;
        }

        private JArray RestGetWorkFlow(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,ActionType,Status/Id,Status/StatusName,InternalStatus";
            rESTOption.expand = "Status";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_WorkFlow", rESTOption);

            return jArray;
        }
    }
}
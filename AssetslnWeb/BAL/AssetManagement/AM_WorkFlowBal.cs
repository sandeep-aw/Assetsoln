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
        public List<AM_WorkFlowModel> getWorkFlowData(ClientContext clientContext, string actionType, string fromstatus, string title)
        {
            List<AM_WorkFlowModel> workFlowBal = new List<AM_WorkFlowModel>();

            string filter = "";

            if (title != "")
            {
                filter = "ActionType eq '" + actionType + "' and Title eq '" + title + "'";
            }
            else
            {
                filter = "ActionType eq '" + actionType + "' and FromStatus eq '" + fromstatus + "'";
            }

            JArray jArray = RestGetWorkFlow(clientContext, filter);

            foreach (JObject j in jArray)
            {
                workFlowBal.Add(new AM_WorkFlowModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    ActionType = j["ActionType"] == null ? "" : Convert.ToString(j["ActionType"]),
                    FromStatus = j["FromStatus"]["StatusName"] == null ? "" : Convert.ToString(j["FromStatus"]["StatusName"]),
                    ToStatus = j["ToStatus"]["StatusName"] == null ? "" : Convert.ToString(j["ToStatus"]["StatusName"]),
                    InternalStatus = j["ToStatus"]["InternalStatus"] == null ? "" : Convert.ToString(j["ToStatus"]["InternalStatus"]),
                    ApproverRoleName = j["ApproverRoleName"]["ApproverRoleName"] == null ? "" : Convert.ToString(j["ApproverRoleName"]["ApproverRoleName"]),
                    ToStatusId = j["ToStatus"]["Id"] == null ? "" : Convert.ToString(j["ToStatus"]["Id"]),
                    FromStatusId = j["FromStatus"]["Id"] == null ? "" : Convert.ToString(j["FromStatus"]["Id"]),
                    ApproverRoleInternalName = j["ApproverRoleName"]["ApproverRoleInternalName"] == null ? "" : Convert.ToString(j["ApproverRoleName"]["ApproverRoleInternalName"])
                });
            }

            return workFlowBal;
        }

        private JArray RestGetWorkFlow(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,ActionType,FromStatus/StatusName,FromStatus/Id,ToStatus/StatusName,ToStatus/Id,ToStatus/InternalStatus,ApproverRoleName/ApproverRoleName,ApproverRoleName/ApproverRoleInternalName";
            rESTOption.expand = "FromStatus,ToStatus,ApproverRoleName";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_WorkFlow", rESTOption);

            return jArray;
        }
    }
}
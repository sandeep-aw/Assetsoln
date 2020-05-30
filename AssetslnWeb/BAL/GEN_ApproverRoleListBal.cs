using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL
{
    public class GEN_ApproverRoleListBal
    {
        public List<GEN_ApproverRoleListModel> GetApproverRoleListBals(ClientContext clientContext)
        {
            List<GEN_ApproverRoleListModel> approverRoleListBals = new List<GEN_ApproverRoleListModel>();

            string filter = null;

            JArray jArray = RestGetApproverRole(clientContext, filter);

            foreach (JObject j in jArray)
            {
                approverRoleListBals.Add(new GEN_ApproverRoleListModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    ApproverRoleName = j["ApproverRoleName"] == null ? "" : Convert.ToString(j["ApproverRoleName"]),
                    ApproverRoleInternalName = j["ApproverRoleInternalName"] == null ? "" : Convert.ToString(j["ApproverRoleInternalName"])
                });
            }

            return approverRoleListBals;
        }

        private JArray RestGetApproverRole(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,ApproverRoleName,ApproverRoleInternalName,SystemDefined,EMP_Code";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "GEN_ApproverRoleList", rESTOption);

            return jArray;
        }
    }
}
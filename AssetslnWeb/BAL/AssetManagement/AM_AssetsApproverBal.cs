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
    public class AM_AssetsApproverBal
    {
        public List<AM_AssetsApproverModel> GetAllAssetsApprover(ClientContext clientContext, int id)
        {
            List<AM_AssetsApproverModel> assetsApproverModel = new List<AM_AssetsApproverModel>();

            string filter = "LID/ID eq '" + id + "'";

            JArray jArray = RESTGet(clientContext, filter);

            if (jArray.Count > 0)
            {
                foreach (JObject j in jArray)
                {
                    assetsApproverModel.Add(new AM_AssetsApproverModel
                    {
                        ID = Convert.ToInt32(j["ID"]),
                        LID = j["LID"]["ID"] == null ? "" : Convert.ToString(j["LID"]["ID"]),
                        ApproverID = j["ApproverID"]["ID"] == null ? "" : Convert.ToString(j["ApproverID"]["ID"]),
                        ApproverCode = j["ApproverCode"] == null ? "" : Convert.ToString(j["ApproverCode"]),
                        ApproverRoleInternalName = j["ApproverRoleInternalName"] == null ? "" : Convert.ToString(j["ApproverRoleInternalName"]),
                        Status = j["Status"] == null ? "" : Convert.ToString(j["Status"])
                    });
                }
            }

            //System.Diagnostics.Debug.WriteLine(assetsModelsBal);
            return assetsApproverModel;
        }

        public AM_AssetsApproverModel GetAssetsApprover(ClientContext clientContext, int id, string rolename)
        {
            AM_AssetsApproverModel assetsApproverModel = new AM_AssetsApproverModel();

            string filter = "LID/ID eq '" + id + "' and ApproverRoleInternalName eq '" + rolename + "'";

            JArray jArray = RESTGet(clientContext, filter);

            assetsApproverModel = new AM_AssetsApproverModel
            {
                ID = Convert.ToInt32(jArray[0]["ID"]),
                LID = jArray[0]["LID"]["ID"] == null ? "" : Convert.ToString(jArray[0]["LID"]["ID"]),
                ApproverID = jArray[0]["ApproverID"]["ID"] == null ? "" : Convert.ToString(jArray[0]["ApproverID"]["ID"]),
                ApproverCode = jArray[0]["ApproverCode"] == null ? "" : Convert.ToString(jArray[0]["ApproverCode"]),
                ApproverRoleInternalName = jArray[0]["ApproverRoleInternalName"] == null ? "" : Convert.ToString(jArray[0]["ApproverRoleInternalName"]),
                Status = jArray[0]["Status"] == null ? "" : Convert.ToString(jArray[0]["Status"])
            };

            //System.Diagnostics.Debug.WriteLine(assetsModelsBal);
            return assetsApproverModel;
        }

        private JArray RESTGet(ClientContext clientContext,string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,LID/ID,ApproverID/ID,ApproverCode,ApproverRoleInternalName,Status";
            rESTOption.expand = "LID,ApproverID";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetsApprover", rESTOption);

            return jArray;
        }

        public string SaveAssetsApproverData(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        //RESTUpdate
        public string UpdateApprover(ClientContext clientContext, string ItemData, string ID)
        {

            string response = RESTUpdate(clientContext, ItemData, ID);

            return response;
        }

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetsApprover", ItemData);
        }

        private string RESTUpdate(ClientContext clientContext, string ItemData, string ID)
        {
            RestService restService = new RestService();

            return restService.UpdateItem(clientContext, "AM_AssetsApprover", ItemData, ID);
        }
    }
}
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
    public class AM_AssetsApplyBal
    {
        public AM_AssetsApplyModel GetDataByID(ClientContext clientContext, int id)
        {
            AM_AssetsApplyModel assetsApplyModels = new AM_AssetsApplyModel();

            string filter = "ID eq '" + id + "'";

            JArray jArray = RestGetAssetsApply(clientContext, filter);

            assetsApplyModels = new AM_AssetsApplyModel
            {
                ID = Convert.ToInt32(jArray[0]["ID"]),
                AssetCount = jArray[0]["AssetCount"] == null ? "" : Convert.ToString(jArray[0]["AssetCount"]),
                Warranty = jArray[0]["Warranty"] == null ? "" : Convert.ToString(jArray[0]["Warranty"]),
                AssetDetails = jArray[0]["AssetDetails"] == null ? "" : Convert.ToString(jArray[0]["AssetDetails"]),
                ReasonToApply = jArray[0]["ReasonToApply"] == null ? "" : Convert.ToString(jArray[0]["ReasonToApply"]),
                RequestDate = jArray[0]["RequestDate"] == null ? "" : Convert.ToString(jArray[0]["RequestDate"]),
                ReturnDate = jArray[0]["ReturnDate"] == null ? "" : Convert.ToString(jArray[0]["ReturnDate"]),
                InternalStatus = jArray[0]["InternalStatus"] == null ? "" : Convert.ToString(jArray[0]["InternalStatus"]),
                RequestNo = jArray[0]["RequestNo"] == null ? "" : Convert.ToString(jArray[0]["RequestNo"]),
                AssetId = jArray[0]["Asset"]["Id"] == null ? "" : Convert.ToString(jArray[0]["Asset"]["Id"]),
                Asset = jArray[0]["Asset"]["Assets"] == null ? "" : Convert.ToString(jArray[0]["Asset"]["Assets"]),
                AssetTypeId = jArray[0]["AssetType"]["Id"] == null ? "" : Convert.ToString(jArray[0]["AssetType"]["Id"]),
                AssetType = jArray[0]["AssetType"]["AssetType"] == null ? "" : Convert.ToString(jArray[0]["AssetType"]["AssetType"]),
                StatusId = jArray[0]["Status"]["Id"] == null ? "" : Convert.ToString(jArray[0]["Status"]["Id"]),
                Status = jArray[0]["Status"]["StatusName"] == null ? "" : Convert.ToString(jArray[0]["Status"]["StatusName"]),
                EmployeeNameId = jArray[0]["EmployeeName"]["Id"] == null ? "" : Convert.ToString(jArray[0]["EmployeeName"]["Id"]),
                EmployeeName = jArray[0]["EmployeeName"]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["EmployeeName"]["EmpCode"]),
                EmployeeCode = jArray[0]["EmployeeCode"] == null ? "" : Convert.ToString(jArray[0]["EmployeeCode"]),
                fname = jArray[0]["EmployeeName"]["FirstName"] == null ? "" : Convert.ToString(jArray[0]["EmployeeName"]["FirstName"]),
                lname = jArray[0]["EmployeeName"]["LastName"] == null ? "" : Convert.ToString(jArray[0]["EmployeeName"]["LastName"]),
                CreatedNameId = jArray[0]["CreatedName"]["Id"] == null ? "" : Convert.ToString(jArray[0]["CreatedName"]["Id"]),
                CreatedName = jArray[0]["CreatedName"]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["CreatedName"]["EmpCode"]),
                CreatedCode = jArray[0]["CreatedCode"] == null ? "" : Convert.ToString(jArray[0]["CreatedCode"]),
                CurrentApprover = jArray[0]["CurrentApprover"] == null ? "" : Convert.ToString(jArray[0]["CurrentApprover"])
            };


            return assetsApplyModels;
        }

        public List<AM_AssetsApplyModel> GetMyRequest(ClientContext clientContext, string EmpCode)
        {
            List<AM_AssetsApplyModel> assetsApplyModels = new List<AM_AssetsApplyModel>();

            string filter = "CreatedCode eq '" + EmpCode + "'";

            JArray jArray = RestGetAssetsApply(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetsApplyModels.Add(new AM_AssetsApplyModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    AssetCount = j["AssetCount"] == null ? "" : Convert.ToString(j["AssetCount"]),
                    Warranty = j["Warranty"] == null ? "" : Convert.ToString(j["Warranty"]),
                    AssetDetails = j["AssetDetails"] == null ? "" : Convert.ToString(j["AssetDetails"]),
                    ReasonToApply = j["ReasonToApply"] == null ? "" : Convert.ToString(j["ReasonToApply"]),
                    RequestDate = j["RequestDate"] == null ? "" : Convert.ToString(j["RequestDate"]),
                    ReturnDate = j["ReturnDate"] == null ? "" : Convert.ToString(j["ReturnDate"]),
                    InternalStatus = j["InternalStatus"] == null ? "" : Convert.ToString(j["InternalStatus"]),
                    RequestNo = j["RequestNo"] == null ? "" : Convert.ToString(j["RequestNo"]),
                    AssetId = j["Asset"]["Id"] == null ? "" : Convert.ToString(j["Asset"]["Id"]),
                    Asset = j["Asset"]["Assets"] == null ? "" : Convert.ToString(j["Asset"]["Assets"]),
                    AssetTypeId = j["AssetType"]["Id"] == null ? "" : Convert.ToString(j["AssetType"]["Id"]),
                    AssetType = j["AssetType"]["AssetType"] == null ? "" : Convert.ToString(j["AssetType"]["AssetType"]),
                    StatusId = j["Status"]["Id"] == null ? "" : Convert.ToString(j["Status"]["Id"]),
                    Status = j["Status"]["StatusName"] == null ? "" : Convert.ToString(j["Status"]["StatusName"]),
                    EmployeeNameId = j["EmployeeName"]["Id"] == null ? "" : Convert.ToString(j["EmployeeName"]["Id"]),
                    EmployeeName = j["EmployeeName"]["EmpCode"] == null ? "" : Convert.ToString(j["EmployeeName"]["EmpCode"]),
                    EmployeeCode = j["EmployeeCode"] == null ? "" : Convert.ToString(j["EmployeeCode"]),
                    fname = j["EmployeeName"]["FirstName"] == null ? "" : Convert.ToString(j["EmployeeName"]["FirstName"]),
                    lname = j["EmployeeName"]["LastName"] == null ? "" : Convert.ToString(j["EmployeeName"]["LastName"]),
                    CreatedNameId = j["CreatedName"]["Id"] == null ? "" : Convert.ToString(j["CreatedName"]["Id"]),
                    CreatedName = j["CreatedName"]["EmpCode"] == null ? "" : Convert.ToString(j["CreatedName"]["EmpCode"]),
                    CreatedCode = j["CreatedCode"] == null ? "" : Convert.ToString(j["CreatedCode"]),
                    CurrentApprover = j["CurrentApprover"] == null ? "" : Convert.ToString(j["CurrentApprover"]),
                });
            }


            return assetsApplyModels;
        }

        public List<AM_AssetsApplyModel> GetDataByEmpcode(ClientContext clientContext, string EmpCode)
        {
            List<AM_AssetsApplyModel> assetsApplyModels = new List<AM_AssetsApplyModel>();

            string filter = "CurrentApprover eq '" + EmpCode + "'";

            JArray jArray = RestGetAssetsApply(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetsApplyModels.Add(new AM_AssetsApplyModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    AssetCount = j["AssetCount"] == null ? "" : Convert.ToString(j["AssetCount"]),
                    Warranty = j["Warranty"] == null ? "" : Convert.ToString(j["Warranty"]),
                    AssetDetails = j["AssetDetails"] == null ? "" : Convert.ToString(j["AssetDetails"]),
                    ReasonToApply = j["ReasonToApply"] == null ? "" : Convert.ToString(j["ReasonToApply"]),
                    RequestDate = j["RequestDate"] == null ? "" : Convert.ToString(j["RequestDate"]),
                    ReturnDate = j["ReturnDate"] == null ? "" : Convert.ToString(j["ReturnDate"]),
                    InternalStatus = j["InternalStatus"] == null ? "" : Convert.ToString(j["InternalStatus"]),
                    RequestNo = j["RequestNo"] == null ? "" : Convert.ToString(j["RequestNo"]),
                    AssetId = j["Asset"]["Id"] == null ? "" : Convert.ToString(j["Asset"]["Id"]),
                    Asset = j["Asset"]["Assets"] == null ? "" : Convert.ToString(j["Asset"]["Assets"]),
                    AssetTypeId = j["AssetType"]["Id"] == null ? "" : Convert.ToString(j["AssetType"]["Id"]),
                    AssetType = j["AssetType"]["AssetType"] == null ? "" : Convert.ToString(j["AssetType"]["AssetType"]),
                    StatusId = j["Status"]["Id"] == null ? "" : Convert.ToString(j["Status"]["Id"]),
                    Status = j["Status"]["StatusName"] == null ? "" : Convert.ToString(j["Status"]["StatusName"]),
                    EmployeeNameId = j["EmployeeName"]["Id"] == null ? "" : Convert.ToString(j["EmployeeName"]["Id"]),
                    EmployeeName = j["EmployeeName"]["EmpCode"] == null ? "" : Convert.ToString(j["EmployeeName"]["EmpCode"]),
                    EmployeeCode = j["EmployeeCode"] == null ? "" : Convert.ToString(j["EmployeeCode"]),
                    fname = j["EmployeeName"]["FirstName"] == null ? "" : Convert.ToString(j["EmployeeName"]["FirstName"]),
                    lname = j["EmployeeName"]["LastName"] == null ? "" : Convert.ToString(j["EmployeeName"]["LastName"]),
                    CreatedNameId = j["CreatedName"]["Id"] == null ? "" : Convert.ToString(j["CreatedName"]["Id"]),
                    CreatedName = j["CreatedName"]["EmpCode"] == null ? "" : Convert.ToString(j["CreatedName"]["EmpCode"]),
                    CreatedCode = j["CreatedCode"] == null ? "" : Convert.ToString(j["CreatedCode"]),
                    CurrentApprover = j["CurrentApprover"] == null ? "" : Convert.ToString(j["CurrentApprover"]),
                });
            }


            return assetsApplyModels;
        }


        public string SaveAssetsApplyData(ClientContext clientContext, string ItemData)
        {

            string response = RESTSave(clientContext, ItemData);

            return response;
        }

        private JArray RestGetAssetsApply(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            string selectqry = "ID,AssetCount,Warranty,AssetDetails,ReasonToApply,RequestDate,ReturnDate,InternalStatus,RequestNo,Asset/Id,Asset/Assets,AssetType/Id,";
            selectqry += "AssetType/AssetType,Status/Id,Status/StatusName,EmployeeName/Id,EmployeeName/EmpCode,EmployeeCode,CreatedName/Id,CreatedName/EmpCode,";
            selectqry += "CreatedCode,CurrentApprover,EmployeeName/FirstName,EmployeeName/LastName";

            rESTOption.select = selectqry;
            rESTOption.expand = "Asset,AssetType,Status,EmployeeName,CreatedName";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetsApply", rESTOption);

            return jArray;
        }

        //RESTUpdate
        public string UpdateAssets(ClientContext clientContext, string ItemData, string ID)
        {

            string response = RESTUpdate(clientContext, ItemData, ID);

            return response;
        }

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetsApply", ItemData);
        }

        private string RESTUpdate(ClientContext clientContext, string ItemData, string ID)
        {
            RestService restService = new RestService();

            return restService.UpdateItem(clientContext, "AM_AssetsApply", ItemData, ID);
        }
    }
}
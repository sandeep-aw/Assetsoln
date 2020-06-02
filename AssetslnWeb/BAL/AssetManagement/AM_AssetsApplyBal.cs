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

        private string RESTSave(ClientContext clientContext, string ItemData)
        {
            RestService restService = new RestService();

            return restService.SaveItem(clientContext, "AM_AssetsApply", ItemData);
        }
    }
}
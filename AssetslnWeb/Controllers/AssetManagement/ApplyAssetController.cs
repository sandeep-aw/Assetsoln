using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetslnWeb.BAL;
using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models;
using AssetslnWeb.Models.AssetManagement;
using AssetslnWeb.Models.EmployeeManagement;
using Microsoft.SharePoint.Client;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class ApplyAssetController : Controller
    {

        // GET: ApplyAsset
        [SharePointContextFilter]
        public ActionResult Index()
        {
            List<AM_BasicInfoModels> basicInfoModels = new List<AM_BasicInfoModels>();
            List<AM_AssetsModel> AssetModelValue = new List<AM_AssetsModel>();
            List<AM_AssetsTypeModel> AssetModelTypeValue = new List<AM_AssetsTypeModel>();

            // get Employee Data
            basicInfoModels = GetEmpInfo();

            // assign employee data to viewbag
            ViewBag.EmpArr = basicInfoModels;

            // set employee array in session object
            Session["EmpData"] = basicInfoModels;

            // get Asset Data
            AssetModelValue = GetAssetsModels();

            // assign asset data to viewbag
            ViewBag.AssetArr = AssetModelValue;

            return View();
        }

        // call assets data
        [SharePointContextFilter]
        [ActionName("GetAssetsModels")]
        private List<AM_AssetsModel> GetAssetsModels()
        {
            List<AM_AssetsModel> AssetModel = new List<AM_AssetsModel>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using(var clientContext=spContext.CreateUserClientContextForSPHost())
            {
                AM_AssetsBal assetsBal = new AM_AssetsBal();
                AssetModel = assetsBal.GetAssets(clientContext);
            }
            return AssetModel;
        }

        // call employee data
        [SharePointContextFilter]
        [ActionName("GetEmpInfo")]
        private List<AM_BasicInfoModels> GetEmpInfo()
        {
            List<AM_BasicInfoModels> EmpModel = new List<AM_BasicInfoModels>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
                EmpModel = basicInfoBal.GetEmpData(clientContext);
            }
            return EmpModel;
        }

        // call asset type data
        public ActionResult GetAssetType(AM_AssetsTypeModel assetTypeObj)
        {
            string assetTypeId = assetTypeObj.AssetType;

            List<AM_AssetsTypeModel> AssetModelType = new List<AM_AssetsTypeModel>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                AM_AssetsTypeBal assetsBal = new AM_AssetsTypeBal();
                AssetModelType = assetsBal.GetAssetsType(clientContext,assetTypeId);
            }

            return Json(AssetModelType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveAssetData(AM_AssetsApplyModel assetsApplyModel)
        {
            //string returnID = "0";

            List<AM_BasicInfoModels> EmployeeArr = (List<AM_BasicInfoModels>)Session["EmpData"];

            // get created name and created code
            string UserId = Session["UserID"].ToString();

            /*List<AM_BasicInfoModels> EmpModel = new List<AM_BasicInfoModels>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
                EmpModel = basicInfoBal.GetCreatedUserData(clientContext, UserId);
                assetsApplyModel.CreatedCode = EmpModel[0].EmpCode;
                assetsApplyModel.CreatedName = EmpModel[0].EmpCode;
            }*/

            GEN_ApproverRoleNameModel masterModel = new GEN_ApproverRoleNameModel();

            GEN_ApproverMasterBal approverMasterBal = new GEN_ApproverMasterBal();

            List<AM_WorkFlowModel> workFlowModels = new List<AM_WorkFlowModel>();
            
            string actionType = "Applied";

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {

                //AM_WorkFlowBal workFlowBal = new AM_WorkFlowBal();
                //workFlowModels = workFlowBal.getWorkFlowData(clientContext, actionType);
                //assetsApplyModel.InternalStatus = workFlowModels[0].InternalStatus;
                //assetsApplyModel.Status = workFlowModels[0].StatusId;
            

                foreach (AM_BasicInfoModels emparr in EmployeeArr)
                {
                    if (emparr.UserNameId == assetsApplyModel.EmployeeCode)
                    {
                        assetsApplyModel.EmployeeCode = emparr.EmpCode;
                        assetsApplyModel.EmployeeName = (emparr.Id).ToString();
                    }
                    else if (emparr.UserNameId == UserId)
                    {
                        assetsApplyModel.CreatedCode = emparr.EmpCode;
                        assetsApplyModel.CreatedName = (emparr.Id).ToString();
                    }
                }

                // get approver data start

                string modulename = "Asset Management";
                string approvertype = "Main";

                masterModel = approverMasterBal.getApproverData(clientContext,assetsApplyModel.EmployeeCode,modulename,approvertype);

                // get approver data end

                string itemdata = " 'RequestNo' : '" + assetsApplyModel.RequestNo + "',";
                itemdata += "'EmployeeNameId': '" + Convert.ToInt32(assetsApplyModel.EmployeeName) + "',";
                itemdata += "'EmployeeCode': '" + assetsApplyModel.EmployeeCode + "',";
                itemdata += "'CreatedNameId': '" + Convert.ToInt32(assetsApplyModel.CreatedName) + "',";
                itemdata += "'CreatedCode': '" + assetsApplyModel.CreatedCode + "',";
                itemdata += "'AssetId': '" + Convert.ToInt32(assetsApplyModel.Asset) + "',";
                itemdata += "'AssetTypeId': '" + Convert.ToInt32(assetsApplyModel.AssetType) + "',";
                itemdata += "'AssetCount': '" + assetsApplyModel.AssetCount + "',";
                itemdata += "'Warranty': '" + assetsApplyModel.Warranty + "',";
                itemdata += "'AssetDetails': '" + assetsApplyModel.AssetDetails + "',";
                itemdata += "'ReasonToApply': '" + assetsApplyModel.ReasonToApply + "',";
                itemdata += "'RequestDate': '" + assetsApplyModel.RequestDate + "',";
                itemdata += "'ReturnDate': '" + assetsApplyModel.ReturnDate + "',";
                itemdata += "'StatusId': '" + Convert.ToInt32(assetsApplyModel.Status) + "',";
                itemdata += "'InternalStatus': '" + assetsApplyModel.InternalStatus + "'";

                //string itemdata = "'InternalStatus': '" + assetsApplyModel.InternalStatus + "'";

                //AM_AssetsApplyBal assetsApplyBal = new AM_AssetsApplyBal();

                //returnID = assetsApplyBal.SaveAssetsApplyData(clientContext, itemdata);

                //if(returnID != "0")
                //{
                //    AM_AssetsHistoryModel historyModel = new AM_AssetsHistoryModel();

                //    var itemapprover = " 'LIDId' : '" + returnID + "',";
                //    itemapprover += "'ActionTakenId': '" + assetsApplyModel.CreatedName + "',";
                //    itemapprover += "'Date': '" + assetsApplyModel.CurrentDate + "',";
                //    itemapprover += "'StatusId': '" + assetsApplyModel.Status + "',";
                //    itemapprover += "'Comments': '" + assetsApplyModel.ReasonToApply + "'";

                //    AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();
                //    assetsHistoryBal.SaveAssetsHistoryData(clientContext, itemapprover);

                //    for(var i=0;i< ApproverArr.Count;i++)
                //    {
                //        AM_ApproverHistoryBal approverHistoryBal = new AM_ApproverHistoryBal();

                //        var itemapproverdata = " 'LIDId' : '" + returnID + "',";
                //        itemapproverdata += "'Status': '" + ApproverArr[i]. Status+ "',";
                //        itemapproverdata += "'ApproverNameId': '" + ApproverArr[i].ApproverName + "',";
                //        itemapproverdata += "'ApproverCode': '" + ApproverArr[i].ApproverCode + "'";

                //        approverHistoryBal.SaveApproverHistoryData(clientContext, itemapproverdata);
                //    }
                //}
            }

            //return Json(returnID, JsonRequestBehavior.AllowGet);
            return View();
        }

    }
}
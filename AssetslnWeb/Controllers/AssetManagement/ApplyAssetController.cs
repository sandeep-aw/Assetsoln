﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
            List<AM_BasicInfoModel> allEmpModel = new List<AM_BasicInfoModel>();
            List<AM_BasicInfoModel> basicInfoModels = new List<AM_BasicInfoModel>();
            List<AM_AssetsModel> AssetModelValue = new List<AM_AssetsModel>();
            List<AM_AssetsTypeModel> AssetModelTypeValue = new List<AM_AssetsTypeModel>();

            // get all Employee Data
            allEmpModel = GetAllEmpInfo();

            // get Employee Data as per manager
            basicInfoModels = GetEmpInfo();

            // assign employee data to viewbag
            ViewBag.EmpArr = basicInfoModels;

            // set employee array in session object
            Session["EmpData"] = allEmpModel;

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

        // call all employee data
        [SharePointContextFilter]
        [ActionName("GetAllEmpInfo")]
        private List<AM_BasicInfoModel> GetAllEmpInfo()
        {
            List<AM_BasicInfoModel> allEmpDataModel = new List<AM_BasicInfoModel>();
            AM_BasicInfoBal allEmpDataBal = new AM_BasicInfoBal();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                allEmpDataModel = allEmpDataBal.GetEmpData(clientContext);
            }
            return allEmpDataModel;
        }

        // call employee data as per manager
        [SharePointContextFilter]
        [ActionName("GetEmpInfo")]
        private List<AM_BasicInfoModel> GetEmpInfo()
        {
            string UserId = Session["UserID"].ToString();

            List<AM_BasicInfoModel> EmpModel = new List<AM_BasicInfoModel>();
            List<AM_BasicInfoModel> EmpByManager = new List<AM_BasicInfoModel>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
                AM_BasicInfoBal EmpDetails = new AM_BasicInfoBal();
                EmpModel = basicInfoBal.GetCurrentLoginUser(clientContext, UserId);
                EmpByManager = EmpDetails.GetEmpByManager(clientContext, (EmpModel[0].ID).ToString());
            }
            return EmpByManager;
        }

        // call approver data
        public ActionResult GetApproveInfo(string EmpId)
        {
            string ecode = null;

            // get all employee information stored in session
            List<AM_BasicInfoModel> EmployeeArr = (List<AM_BasicInfoModel>)Session["EmpData"];

            for (int i = 0; i < EmployeeArr.Count; i++)
            {
                if(EmployeeArr[i].UserNameId==EmpId)
                {
                    ecode = EmployeeArr[i].EmpCode;
                }
            }


            // get approver data start

            // declare ApproverRoleName list object to get approvers data
            List<GEN_ApproverRoleNameModel> masterModel = new List<GEN_ApproverRoleNameModel>();

            // declare object to call approvers bal class
            GEN_ApproverMasterBal approverMasterBal = new GEN_ApproverMasterBal();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {

                string modulename = "Asset Management";
                string approvertype = "Main";

                masterModel = approverMasterBal.getApproverData(clientContext, ecode, modulename, approvertype);


                //for (var i = 0; i < masterModel.Count; i++)
                //{
                //    AM_BasicInfoBal infoBalEmpCode = new AM_BasicInfoBal();

                //    AM_BasicInfoModel basicInfoModels = new AM_BasicInfoModel();

                //    basicInfoModels = infoBalEmpCode.GetDataByEmpcode(clientContext, masterModel[i].Empcode);

                //    if (masterModel[i].Sequence == 0)
                //    {
                //        assetsApplyModel.CurrentApprover = masterModel[i].Empcode;
                //    }

                //    masterModel[i].ApproverId = (basicInfoModels.ID).ToString();
                //}

                for (var i = 0; i < masterModel.Count; i++)
                {
                    for (int j = 0; j < EmployeeArr.Count; j++)
                    {
                        if (masterModel[i].Empcode == EmployeeArr[j].EmpCode)
                        {
                            masterModel[i].Title = EmployeeArr[j].User_Name;
                        }
                    }
                }                
            }
            // get approver data end

            return Json(masterModel, JsonRequestBehavior.AllowGet);
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
            string returnID = "0";

            HttpCookie cookie = Request.Cookies["AssetsArr"];

            //// get all employee information stored in session
            //List<AM_BasicInfoModel> EmployeeArr = (List<AM_BasicInfoModel>)Session["EmpData"];

            //// get created name and created code
            //string UserId = Session["UserID"].ToString();

            ///*List<AM_BasicInfoModel> EmpModel = new List<AM_BasicInfoModel>();

            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
            //    EmpModel = basicInfoBal.GetCreatedUserData(clientContext, UserId);
            //    assetsApplyModel.CreatedCode = EmpModel[0].EmpCode;
            //    assetsApplyModel.CreatedName = EmpModel[0].EmpCode;
            //}*/



            //// declare workflow list object to get workflow data
            //List<AM_WorkFlowModel> workFlowModels = new List<AM_WorkFlowModel>();

            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    // get workflow details by call workflowbal class method

            //    string title = "Main";
            //    string actionType = "Forward";
            //    string fromstatus = "";               

            //    AM_WorkFlowBal workFlowBal = new AM_WorkFlowBal();
            //    workFlowModels = workFlowBal.getWorkFlowData(clientContext, actionType, fromstatus, title);
            //    assetsApplyModel.InternalStatus = workFlowModels[0].InternalStatus;
            //    assetsApplyModel.Status = workFlowModels[0].ToStatusId;


            //    foreach (AM_BasicInfoModel emparr in EmployeeArr)
            //    {
            //        if (emparr.UserNameId == assetsApplyModel.EmployeeCode)
            //        {
            //            assetsApplyModel.EmployeeCode = emparr.EmpCode;
            //            assetsApplyModel.EmployeeName = (emparr.ID).ToString();
            //        }
            //        else if (emparr.UserNameId == UserId)
            //        {
            //            assetsApplyModel.CreatedCode = emparr.EmpCode;
            //            assetsApplyModel.CreatedName = (emparr.ID).ToString();
            //        }
            //    }



            //    var reqdatetime = (assetsApplyModel.RequestDate).ToString();
            //    DateTime reqdt = DateTime.ParseExact(reqdatetime, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            //    var reqdate = reqdt.ToString("MM/dd/yyyy");

            //    var returndatetime = (assetsApplyModel.ReturnDate).ToString();
            //    DateTime returndt = DateTime.ParseExact(returndatetime, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            //    var returndate = returndt.ToString("MM/dd/yyyy");

            //    // save assetsapply data
            //    string itemdata = " 'RequestNo' : '" + assetsApplyModel.RequestNo + "',";
            //    itemdata += "'EmployeeNameId': '" + Convert.ToInt32(assetsApplyModel.EmployeeName) + "',";
            //    itemdata += "'EmployeeCode': '" + assetsApplyModel.EmployeeCode + "',";
            //    itemdata += "'CreatedNameId': '" + Convert.ToInt32(assetsApplyModel.CreatedName) + "',";
            //    itemdata += "'CreatedCode': '" + assetsApplyModel.CreatedCode + "',";
            //    //itemdata += "'AssetId': '" + Convert.ToInt32(assetsApplyModel.Asset) + "',";
            //   // itemdata += "'AssetTypeId': '" + Convert.ToInt32(assetsApplyModel.AssetType) + "',";
            //   // itemdata += "'AssetCount': '" + assetsApplyModel.AssetCount + "',";
            //   // itemdata += "'Warranty': '" + assetsApplyModel.Warranty + "',";
            //    //itemdata += "'AssetDetails': '" + assetsApplyModel.AssetDetails + "',";
            //    //itemdata += "'ReasonToApply': '" + assetsApplyModel.ReasonToApply + "',";
            //    itemdata += "'RequestDate': '" + reqdate + "',";
            //    itemdata += "'ReturnDate': '" + returndate + "',";
            //    itemdata += "'StatusId': '" + Convert.ToInt32(assetsApplyModel.Status) + "',";
            //    itemdata += "'InternalStatus': '" + assetsApplyModel.InternalStatus + "',";
            //    itemdata += "'CurrentApprover': '" + assetsApplyModel.CurrentApprover + "'";

            //    // call assetsapply bal class method
            //    AM_AssetsApplyBal assetsApplyBal = new AM_AssetsApplyBal();

            //    returnID = assetsApplyBal.SaveAssetsApplyData(clientContext, itemdata);

            //    if (returnID != "0")
            //    {
            //        // save history data
            //        AM_AssetsHistoryModel historyModel = new AM_AssetsHistoryModel();

            //        var itemapprover = " 'LIDId' : '" + returnID + "',";
            //        itemapprover += "'ActionTakenId': '" + assetsApplyModel.CreatedName + "',";
            //        itemapprover += "'Date': '" + assetsApplyModel.CurrentDate + "',";
            //        itemapprover += "'StatusId': '" + assetsApplyModel.Status + "',";
            //        //itemapprover += "'Comments': '" + assetsApplyModel.ReasonToApply + "'";

            //        AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();
            //        assetsHistoryBal.SaveAssetsHistoryData(clientContext, itemapprover);

            //        // save approver data
            //        //for (var i = 0; i < masterModel.Count; i++)
            //        //{
            //        //    AM_AssetsApproverBal assetsApproverBal = new AM_AssetsApproverBal();

            //        //    string status = "Pending";

            //        //    var itemapproverdata = " 'LIDId' : '" + returnID + "',";
            //        //    itemapproverdata += "'ApproverIDId': '" + masterModel[i].ApproverId + "',";
            //        //    itemapproverdata += "'ApproverCode': '" + masterModel[i].Empcode + "',";
            //        //    itemapproverdata += "'ApproverRoleInternalName': '" + masterModel[i].Role + "',";
            //        //    itemapproverdata += "'Status': '" + status + "'";

            //        //    assetsApproverBal.SaveAssetsApproverData(clientContext, itemapproverdata);
            //        //}
            //    }
            //}

            //return Json(returnID, JsonRequestBehavior.AllowGet);
            return View();
        }

    }
}
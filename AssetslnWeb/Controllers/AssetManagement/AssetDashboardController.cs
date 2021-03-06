﻿using AssetslnWeb.BAL;
using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models;
using AssetslnWeb.Models.AssetManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class AssetDashboardController : Controller
    {
        AM_AssetMainModel assetMainModel = new AM_AssetMainModel();

        // declare object to get pending and my request data
        List<AM_AssetsApplyModel> PendingReqModels = new List<AM_AssetsApplyModel>();
        List<AM_AssetsApplyModel> MyRequestModels = new List<AM_AssetsApplyModel>();
        List<AM_BasicInfoModel> basicInfoModels = new List<AM_BasicInfoModel>();

        // call bal function to get pending and my request data
        AM_AssetsApplyBal PendingReqBals = new AM_AssetsApplyBal();
        AM_AssetsApplyBal MyRequestBal = new AM_AssetsApplyBal();

        // create asset apply model object
        AM_AssetsApplyModel applyModel = new AM_AssetsApplyModel();

        // call assetsapply bal class method
        AM_AssetsApplyBal assetsApplyBal = new AM_AssetsApplyBal();

        // create object of AssetApplyDetails model and bal
        List<AM_AssetsApplyDetailsModel> assetsApplyDetailsModels = new List<AM_AssetsApplyDetailsModel>();
        AM_AssetsApplyDetailsBal assetsApplyDetailsBal = new AM_AssetsApplyDetailsBal();

        // create object of AssetHistory model and bal
        List<AM_AssetsHistoryModel> assetsHistoryModel = new List<AM_AssetsHistoryModel>();
        AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();

        // create object to get AssetsApproverData
        List<AM_AssetsApproverModel> assetsApproverModels = new List<AM_AssetsApproverModel>();
        AM_AssetsApproverBal assetsApproverBal = new AM_AssetsApproverBal();
        List<AM_BasicInfoModel> allEmpModel = new List<AM_BasicInfoModel>();
        List<string> ApproverNames = new List<string>();

        // GET: AssetDashboard
        public ActionResult Index()
        {
            // get current use login id

            string UserId = Session["UserID"].ToString();

            // declare object to get current login user data
            List<AM_BasicInfoModel> EmpModel = new List<AM_BasicInfoModel>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
                EmpModel = basicInfoBal.GetCurrentLoginUser(clientContext, UserId);

                // get pending request data
                PendingReqModels = PendingReqBals.GetDataByEmpcode(clientContext,EmpModel[0].EmpCode);

                // get my request data
                MyRequestModels = MyRequestBal.GetMyRequest(clientContext, EmpModel[0].EmpCode);
            }

            // set current user empcode
            Session["CurUserEmpCode"] = EmpModel[0].EmpCode;

            /* add data to the model */
                // add pending request data
                assetMainModel.PendingRequest = PendingReqModels.OrderByDescending(i => i.ID);

                // add my request data
                assetMainModel.MyRequest = MyRequestModels.OrderByDescending(i => i.ID);

                // assign employee data get as per manager
                assetMainModel.BasicInfoModels = GetEmpInfo();

                // assign asset data
                assetMainModel.assetsModels = GetAssetsModels();

            return View(assetMainModel);
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

        // call assets data
        [SharePointContextFilter]
        [ActionName("GetAssetsModels")]
        private List<AM_AssetsModel> GetAssetsModels()
        {
            List<AM_AssetsModel> AssetModel = new List<AM_AssetsModel>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
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

        // call approver data
        public ActionResult GetApproveInfo(string EmpId)
        {
            string ecode = null;

            // get all employee information stored in session
            List<AM_BasicInfoModel> EmployeeArr = GetAllEmpInfo();

            for (int i = 0; i < EmployeeArr.Count; i++)
            {
                if (EmployeeArr[i].ID.ToString() == EmpId)
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

                for (var i = 0; i < masterModel.Count; i++)
                {
                    for (int j = 0; j < EmployeeArr.Count; j++)
                    {
                        if (masterModel[i].Empcode == EmployeeArr[j].EmpCode)
                        {
                            masterModel[i].Title = EmployeeArr[j].FirstName + " " + EmployeeArr[j].LastName;
                        }
                    }
                }
            }

            // get approver data end

            // set session for approver data
            Session["masterModel"] = masterModel;

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
                AssetModelType = assetsBal.GetAssetsType(clientContext, assetTypeId);
            }

            return Json(AssetModelType, JsonRequestBehavior.AllowGet);
        }

        // save asset data
        [HttpPost]
        public ActionResult SaveAssetData(AM_AssetsApplyModel assetsApplyModel)
        {
            string returnID = "0";

            // get all employee information stored in session
            List<AM_BasicInfoModel> EmployeeArr = GetAllEmpInfo();

            // get created name and created code
            string UserId = Session["UserID"].ToString();

            // get current user emp code
            string CurUserEmpCode = Session["CurUserEmpCode"].ToString();

            // declare ApproverRoleName list object to get approvers data
            List<GEN_ApproverRoleNameModel> masterModel = new List<GEN_ApproverRoleNameModel>();

            // get session of approver data
            masterModel = (List<GEN_ApproverRoleNameModel>)Session["masterModel"];

            // declare workflow list object to get workflow data
            List<AM_WorkFlowModel> workFlowModels = new List<AM_WorkFlowModel>();

            // store data in the applydetails array
            List<AM_AssetsApplyDetailsModel> assetsApplyDetailsModels = assetsApplyModel.applyDetailsModel;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {

                for (var i = 0; i < masterModel.Count; i++)
                {
                    AM_BasicInfoBal infoBalEmpCode = new AM_BasicInfoBal();

                    AM_BasicInfoModel basicInfoModels = new AM_BasicInfoModel();

                    basicInfoModels = infoBalEmpCode.GetDataByEmpcode(clientContext, masterModel[i].Empcode);

                    if (masterModel[i].Sequence == 0)
                    {
                        assetsApplyModel.CurrentApprover = masterModel[i].Empcode;
                    }

                    masterModel[i].ApproverId = (basicInfoModels.ID).ToString();
                }


                // get workflow details by call workflowbal class method

                string title = "Main";
                string actionType = "Forward";
                string fromstatus = "";

                AM_WorkFlowBal workFlowBal = new AM_WorkFlowBal();
                workFlowModels = workFlowBal.getWorkFlowData(clientContext, actionType, fromstatus, title);
                assetsApplyModel.InternalStatus = workFlowModels[0].InternalStatus;
                assetsApplyModel.Status = workFlowModels[0].ToStatusId;

                if(EmployeeArr.Count>0)
                {
                    foreach (AM_BasicInfoModel emparr in EmployeeArr)
                    {
                        if (emparr.ID.ToString() == assetsApplyModel.EmployeeName)
                        {
                            assetsApplyModel.EmployeeCode = emparr.EmpCode;
                            //assetsApplyModel.EmployeeName = (emparr.ID).ToString();
                        }
                        else if (emparr.UserNameId == UserId)
                        {
                            assetsApplyModel.CreatedCode = emparr.EmpCode;
                            assetsApplyModel.CreatedName = (emparr.ID).ToString();
                        }
                    }

                }

                var reqdate = (assetsApplyModel.RequestDate).ToString("MM/dd/yyyy");

                var returndate = (assetsApplyModel.ReturnDate).ToString("MM/dd/yyyy");

                // save assetsapply data
                string itemdata = " 'RequestNo' : '" + assetsApplyModel.RequestNo + "',";
                itemdata += "'EmployeeNameId': '" + Convert.ToInt32(assetsApplyModel.EmployeeName) + "',";
                itemdata += "'EmployeeCode': '" + assetsApplyModel.EmployeeCode + "',";
                itemdata += "'CreatedNameId': '" + Convert.ToInt32(assetsApplyModel.CreatedName) + "',";
                itemdata += "'CreatedCode': '" + assetsApplyModel.CreatedCode + "',";
                itemdata += "'RequestDate': '" + reqdate + "',";
                itemdata += "'ReturnDate': '" + returndate + "',";
                itemdata += "'StatusId': '" + Convert.ToInt32(assetsApplyModel.Status) + "',";
                itemdata += "'InternalStatus': '" + assetsApplyModel.InternalStatus + "',";
                itemdata += "'CurrentApprover': '" + assetsApplyModel.CurrentApprover + "',";
                itemdata += "'Comments': '" + assetsApplyModel.Comments + "'";

                returnID = assetsApplyBal.SaveAssetsApplyData(clientContext, itemdata);

                if (returnID != "0")
                {
                    // get request no

                    applyModel = assetsApplyBal.GetDataByID(clientContext, Convert.ToInt32(returnID));

                    if (assetsApplyDetailsModels != null)
                    {
                        for (int i = 0; i < assetsApplyDetailsModels.Count; i++)
                        {
                            // save assetsapplydetails data

                            string itemdetails = " 'LIDId' : '" + returnID + "',";
                            itemdetails += "'AssetId': '" + Convert.ToInt32(assetsApplyDetailsModels[i].AssetId) + "',";
                            itemdetails += "'AssetTypeId': '" + Convert.ToInt32(assetsApplyDetailsModels[i].AssetTypeId) + "',";
                            itemdetails += "'UserApplyQuantity': '" + assetsApplyDetailsModels[i].UserApplyQuantity + "',";
                            itemdetails += "'AssetDetails': '" + assetsApplyDetailsModels[i].AssetDetails + "',";
                            itemdetails += "'ReasonToApply': '" + assetsApplyDetailsModels[i].ReasonToApply + "',";
                            itemdetails += "'Replacement': '" + assetsApplyDetailsModels[i].Replacement + "'";

                            AM_AssetsApplyDetailsBal assetsApplyDetailsBal = new AM_AssetsApplyDetailsBal();
                            assetsApplyDetailsBal.SaveAssetsApplyDetails(clientContext, itemdetails);
                        }
                    }

                    var currentdate = (assetsApplyModel.CurrentDate).ToString("MM/dd/yyyy");

                    // save history data
                    AM_AssetsHistoryModel historyModel = new AM_AssetsHistoryModel();

                    string itemapprover = " 'LIDId' : '" + returnID + "',";
                    itemapprover += "'ActionTakenId': '" + assetsApplyModel.CreatedName + "',";
                    itemapprover += "'Date': '" + currentdate + "',";
                    itemapprover += "'StatusId': '" + assetsApplyModel.Status + "',";
                    itemapprover += "'Comments': '" + assetsApplyModel.Comments + "'";

                    AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();
                    assetsHistoryBal.SaveAssetsHistoryData(clientContext, itemapprover);

                    // save approver data
                    for (var i = 0; i < masterModel.Count; i++)
                    {
                        AM_AssetsApproverBal assetsApproverBal = new AM_AssetsApproverBal();

                        string status = "Pending";

                        var itemapproverdata = " 'LIDId' : '" + returnID + "',";
                        itemapproverdata += "'ApproverIDId': '" + masterModel[i].ApproverId + "',";
                        itemapproverdata += "'ApproverCode': '" + masterModel[i].Empcode + "',";
                        itemapproverdata += "'ApproverRoleInternalName': '" + masterModel[i].Role + "',";
                        itemapproverdata += "'Status': '" + status + "'";

                        assetsApproverBal.SaveAssetsApproverData(clientContext, itemapproverdata);
                    }

                    // get pending request data
                    PendingReqModels = PendingReqBals.GetDataByEmpcode(clientContext, CurUserEmpCode);

                    // get my request data
                    MyRequestModels = MyRequestBal.GetMyRequest(clientContext, CurUserEmpCode);
                }
            }

            // set save asset data
            assetMainModel.assetsApplyModel = applyModel;

            // set pending request data
            assetMainModel.PendingRequest = PendingReqModels.OrderByDescending(i => i.ID);

            // set my request data
            assetMainModel.MyRequest = MyRequestModels.OrderByDescending(i => i.ID);

            return PartialView("AM_DashboardView", assetMainModel);
            //return Json(assetMainModelreturn, JsonRequestBehavior.AllowGet);
            //return View();
        }

        // get asset data
        public ActionResult GetAssetView(string ID)
        {
            assetMainModel = GetAssetDataById(ID);

            return PartialView("AM_AssetView", assetMainModel);
        }

        public AM_AssetMainModel GetAssetDataById(string id)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                applyModel = assetsApplyBal.GetDataByID(clientContext, Convert.ToInt32(id));
                assetsApplyDetailsModels = assetsApplyDetailsBal.GetDetailsById(clientContext, Convert.ToInt32(id));
                assetsHistoryModel = assetsHistoryBal.GetHistoryById(clientContext, Convert.ToInt32(id));
                assetsApproverModels = assetsApproverBal.GetAllAssetsApprover(clientContext, Convert.ToInt32(id));
            }

            // get all Employee Data
            allEmpModel = GetAllEmpInfo();

            for (var i = 0; i < assetsApproverModels.Count; i++)
            {
                for (var j = 0; j < allEmpModel.Count; j++)
                {
                    if (assetsApproverModels[i].ApproverCode == allEmpModel[j].EmpCode)
                    {
                        ApproverNames.Add(allEmpModel[j].FirstName + " " + allEmpModel[j].LastName);
                    }
                }
            }

            assetMainModel.assetsApplyModel = applyModel;
            assetMainModel.assetsApplyDetailsModels = assetsApplyDetailsModels;
            assetMainModel.assetsHistoryModels = assetsHistoryModel;
            assetMainModel.ApproverNames = ApproverNames;
            return assetMainModel;
        }

    }
}
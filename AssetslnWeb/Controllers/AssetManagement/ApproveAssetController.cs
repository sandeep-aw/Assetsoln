using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;
using AssetslnWeb.Models.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class ApproveAssetController : Controller
    {
        // GET: ApproveAsset
        [SharePointContextFilter]
        [HttpGet]
        public ActionResult Index()
        {
            int id = Convert.ToInt32(TempData["MyTempData"]);

            AM_AssetsApplyModel assetsApplyModel = new AM_AssetsApplyModel();
            AM_AssetsApplyBal assetsApplyBal = new AM_AssetsApplyBal();

            List<AM_AssetsHistoryModel> assetsHistoryModel = new List<AM_AssetsHistoryModel>();
            AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();

            List<AM_AssetsApplyDetailsModel> assetsApplyDetailsModels = new List<AM_AssetsApplyDetailsModel>();
            AM_AssetsApplyDetailsBal assetsApplyDetailsBal = new AM_AssetsApplyDetailsBal();

            List<AM_AssetsApproverModel> assetsApproverModels = new List<AM_AssetsApproverModel>();
            AM_AssetsApproverBal assetsApproverBal = new AM_AssetsApproverBal();

            List<AM_BasicInfoModel> allEmpModel = new List<AM_BasicInfoModel>();
            List<string> ApproverNames = new List<string>();

            // get all Employee Data
            allEmpModel = GetAllEmpInfo();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                assetsApplyModel = assetsApplyBal.GetDataByID(clientContext, id);
                assetsHistoryModel = assetsHistoryBal.GetHistoryById(clientContext, id);
                assetsApplyDetailsModels = assetsApplyDetailsBal.GetDetailsById(clientContext, id);
                assetsApproverModels = assetsApproverBal.GetAllAssetsApprover(clientContext, id);
            }

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

            ViewBag.assetsView = assetsApplyModel;
            ViewBag.assetsHistory = assetsHistoryModel;
            ViewBag.assetsDetails = assetsApplyDetailsModels;
            ViewBag.assetsApprover = ApproverNames;

            Session["ApproveData"] = assetsApplyModel;

            return View();
        }

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult getAssetId(int ID)
        {
            TempData.Add("MyTempData", ID);
            return Json(ID, JsonRequestBehavior.AllowGet);
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

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult ApproveFunc(string Comment, string CurrentDate)
        {
            string returnID = "0";

            // get assets data by session
            AM_AssetsApplyModel approveapplymodel = new AM_AssetsApplyModel();

            approveapplymodel = (AM_AssetsApplyModel)Session["ApproveData"];

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {

                // call workflow data
                List<AM_WorkFlowModel> workFlowModelPrevious = new List<AM_WorkFlowModel>();
                List<AM_WorkFlowModel> workFlowModel = new List<AM_WorkFlowModel>();

                AM_WorkFlowBal workFlowBal = new AM_WorkFlowBal();


                string actiontype = "Forward";
                string fromstatus = approveapplymodel.StatusId;
                string title = "";

                workFlowModelPrevious = workFlowBal.getPreviousWorkflow(clientContext, fromstatus);
                workFlowModel = workFlowBal.getWorkFlowData(clientContext, actiontype, fromstatus, title);

                AM_AssetsApproverModel _AssetsApproverModelPrevious = new AM_AssetsApproverModel();
                AM_AssetsApproverModel _AssetsApproverModel = new AM_AssetsApproverModel();
                AM_AssetsApproverBal assetsApproverBal = new AM_AssetsApproverBal();

                _AssetsApproverModelPrevious = assetsApproverBal.GetAssetsApprover(clientContext, approveapplymodel.ID, workFlowModelPrevious[0].ApproverRoleInternalName);
                _AssetsApproverModel = assetsApproverBal.GetAssetsApprover(clientContext, approveapplymodel.ID, workFlowModel[0].ApproverRoleInternalName);

                //get current user empcode
                string UserId = Session["UserID"].ToString();

                List<AM_BasicInfoModel> basicInfoModels = new List<AM_BasicInfoModel>();
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();

                basicInfoModels = basicInfoBal.GetCurrentLoginUser(clientContext, UserId);

                // update asset data
                AM_AssetsApplyBal updateassets = new AM_AssetsApplyBal();

                string itemdata = " 'CurrentApprover' : '" + _AssetsApproverModel.ApproverCode + "',";
                itemdata += "'StatusId': '" + Convert.ToInt32(workFlowModel[0].ToStatusId) + "',";
                itemdata += "'InternalStatus': '" + workFlowModel[0].InternalStatus + "'";

                returnID = updateassets.UpdateAssets(clientContext, itemdata, (approveapplymodel.ID).ToString());

                // update approver data
                string itemapprover = "'Status' : 'Approve'";

                assetsApproverBal.UpdateApprover(clientContext, itemapprover, (_AssetsApproverModelPrevious.ID).ToString());

                // save history data
                AM_AssetsHistoryModel historyModel = new AM_AssetsHistoryModel();

                var itemhistory = " 'LIDId' : '" + approveapplymodel.ID + "',";
                itemhistory += "'ActionTakenId': '" + basicInfoModels[0].ID + "',";
                itemhistory += "'Date': '" + CurrentDate + "',";
                itemhistory += "'StatusId': '" + workFlowModel[0].ToStatusId + "',";
                itemhistory += "'Comments': '" + Comment + "'";

                AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();
                assetsHistoryBal.SaveAssetsHistoryData(clientContext, itemhistory);
            }

            return Json(returnID, JsonRequestBehavior.AllowGet);

            //return Json(Comment, JsonRequestBehavior.AllowGet);
        }

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult RejectFunc(string Comment, string CurrentDate)
        {
            string returnID = "0";

            // get assets data by session
            AM_AssetsApplyModel approveapplymodel = new AM_AssetsApplyModel();

            approveapplymodel = (AM_AssetsApplyModel)Session["ApproveData"];

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {

                // call workflow data
                List<AM_WorkFlowModel> workFlowModel = new List<AM_WorkFlowModel>();

                AM_WorkFlowBal workFlowBal = new AM_WorkFlowBal();


                string actiontype = "Backward";
                string fromstatus = approveapplymodel.StatusId;
                string title = "";


                workFlowModel = workFlowBal.getWorkFlowData(clientContext, actiontype, fromstatus, title);


                // get current user empcode
                string UserId = Session["UserID"].ToString();

                List<AM_BasicInfoModel> basicInfoModels = new List<AM_BasicInfoModel>();
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();

                basicInfoModels = basicInfoBal.GetCurrentLoginUser(clientContext, UserId);

                // update asset data
                AM_AssetsApplyBal updateassets = new AM_AssetsApplyBal();

                string itemdata = " 'CurrentApprover' : '',";
                itemdata += "'StatusId': '" + Convert.ToInt32(workFlowModel[0].ToStatusId) + "',";
                itemdata += "'InternalStatus': '" + workFlowModel[0].InternalStatus + "'";

                returnID = updateassets.UpdateAssets(clientContext, itemdata, (approveapplymodel.ID).ToString());

                // save history data
                AM_AssetsHistoryModel historyModel = new AM_AssetsHistoryModel();

                string itemhistory = " 'LIDId' : '" + approveapplymodel.ID + "',";
                itemhistory += "'ActionTakenId': '" + basicInfoModels[0].ID + "',";
                itemhistory += "'Date': '" + CurrentDate + "',";
                itemhistory += "'StatusId': '" + workFlowModel[0].ToStatusId + "',";
                itemhistory += "'Comments': '" + Comment + "'";

                AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();
                assetsHistoryBal.SaveAssetsHistoryData(clientContext, itemhistory);
            }

            return Json(returnID, JsonRequestBehavior.AllowGet);

            //return Json(Comment, JsonRequestBehavior.AllowGet);
        }

    }
}
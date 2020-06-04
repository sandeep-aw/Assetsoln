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

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                assetsApplyModel = assetsApplyBal.GetDataByID(clientContext, id);
                assetsHistoryModel = assetsHistoryBal.GetHistoryById(clientContext, id);
            }

            ViewBag.assetsView = assetsApplyModel;
            ViewBag.assetsHistory = assetsHistoryModel;

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

                List<AM_BasicInfoModels> basicInfoModels = new List<AM_BasicInfoModels>();
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
                itemhistory += "'ActionTakenId': '" + basicInfoModels[0].Id + "',";
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
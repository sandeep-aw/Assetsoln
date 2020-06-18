using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;
using AssetslnWeb.Models.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class AllocateAssetController : Controller
    {
        // GET: AllocateAsset
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

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                assetsApplyModel = assetsApplyBal.GetDataByID(clientContext, id);
                assetsHistoryModel = assetsHistoryBal.GetHistoryById(clientContext, id);
                assetsApplyDetailsModels = assetsApplyDetailsBal.GetDetailsById(clientContext, id);

                ViewBag.assetsView = assetsApplyModel;
                ViewBag.assetsHistory = assetsHistoryModel;
                ViewBag.assetsDetails = assetsApplyDetailsModels;
            }

            Session["assetsDetails"] = assetsApplyDetailsModels;
            Session["assetsMain"] = assetsApplyModel;

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
        public ActionResult AllocateFunc(AM_AssetAllotmentMainModel assetAllotmentMainModel)
        {
            string returnID = "0";

            List<AM_AssetsApplyDetailsModel> _AssetsApplyDetailsModel = new List<AM_AssetsApplyDetailsModel>();
            AM_AssetsApplyModel _AssetsApplyModel = new AM_AssetsApplyModel();

            // get asset main details
            _AssetsApplyModel = (AM_AssetsApplyModel)Session["assetsMain"];

            // get asset information details
            _AssetsApplyDetailsModel = (List<AM_AssetsApplyDetailsModel>)Session["assetsDetails"];

            //set variable to change flow
            Boolean changeFlow = true;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                List<AM_AssetAllotmentHistoryModel> assetAllotmentHistoryModels = assetAllotmentMainModel.assetAllotmentHistoryModels;

                for (var i = 0; i < assetAllotmentHistoryModels.Count; i++)
                {
                    for (var j = 0; j < _AssetsApplyDetailsModel.Count; j++)
                    {
                        if (assetAllotmentHistoryModels[i].asset == _AssetsApplyDetailsModel[j].Asset && assetAllotmentHistoryModels[i].assetType == _AssetsApplyDetailsModel[j].AssetType)
                        { 
                            // create to update stock in AssetType list
                            AM_AssetsTypeModel assetsTypeModel = new AM_AssetsTypeModel();
                            AM_AssetsTypeBal assetsTypeBal = new AM_AssetsTypeBal();

                            int count = Convert.ToInt32(_AssetsApplyDetailsModel[i].AssetTypeStock) - Convert.ToInt32(assetAllotmentHistoryModels[i].AssetQuantity);

                            string itemdata = " 'Stock' : '" + count + "'";

                            returnID = assetsTypeBal.UpdateAssetType(clientContext, itemdata, _AssetsApplyDetailsModel[j].AssetTypeId);

                            // add total of user allotted quantity
                            int userallotqty = 0;

                            if (_AssetsApplyDetailsModel[j].UserAllottedQuantity != null)
                            {
                                try {
                                    userallotqty = Convert.ToInt32(_AssetsApplyDetailsModel[j].UserAllottedQuantity) + Convert.ToInt32(assetAllotmentHistoryModels[i].AssetQuantity);
                                }
                                catch(Exception ex)
                                {
                                    userallotqty = 0;
                                }
                            }
                            else
                            {
                                userallotqty = Convert.ToInt32(assetAllotmentHistoryModels[i].AssetQuantity);
                            }

                            // check count is match or not
                            if (userallotqty != Convert.ToInt32(_AssetsApplyDetailsModel[j].UserAllottedQuantity))
                            {
                                changeFlow = false;
                            }


                            // save assetsapply data

                            AM_AssetsApplyDetailsBal _AssetsApplyDetailsBal = new AM_AssetsApplyDetailsBal();

                            string itemdetails = " 'UserAllottedtQuantity' : '" + userallotqty + "',";
                            itemdetails += "'UserBalanceQuantity': '" + assetAllotmentHistoryModels[i].PendingQuantity + "'";

                            _AssetsApplyDetailsBal.UpdateAssetsDetails(clientContext, itemdetails, _AssetsApplyDetailsModel[j].ID.ToString());

                            // save allotment history data
                            var actionType = "Cr";

                            var warrantydate = (assetAllotmentHistoryModels[i].WarrantyDate).ToString("MM/dd/yyyy");

                            string itemallotehistory = " 'LIDId' : '" + _AssetsApplyDetailsModel[j].ID + "',";
                            itemallotehistory += "'ActionType': '" + actionType + "',";
                            itemallotehistory += "'AssetQuantity': '" + assetAllotmentHistoryModels[i].AssetQuantity + "',";
                            itemallotehistory += "'PendingQuantity': '" + assetAllotmentHistoryModels[i].PendingQuantity + "',";
                            itemallotehistory += "'ProdSrNo': '" + assetAllotmentHistoryModels[i].ProdSrNo + "',";
                            itemallotehistory += "'ModelNo': '" + assetAllotmentHistoryModels[i].ModelNo + "',";
                            itemallotehistory += "'WarrantyDate': '" + warrantydate + "',";
                            itemallotehistory += "'Remark': '" + assetAllotmentHistoryModels[i].Remark + "'";

                            AM_AssetAllotmentHistoryBal _AssetAllotmentHistoryBal = new AM_AssetAllotmentHistoryBal();
                            _AssetAllotmentHistoryBal.SaveAllotmentHistory(clientContext, itemallotehistory);
                        }
                    }
                }


                //// call workflow data
                //List<AM_WorkFlowModel> workFlowModelPrevious = new List<AM_WorkFlowModel>();
                //List<AM_WorkFlowModel> workFlowModel = new List<AM_WorkFlowModel>();
                //AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
                //List<AM_BasicInfoModel> basicInfoModels = new List<AM_BasicInfoModel>();

                //// change flow code
                //if (changeFlow == true)
                //{
                //    AM_WorkFlowBal workFlowBal = new AM_WorkFlowBal();

                //    string actiontype = "Forward";
                //    string fromstatus = _AssetsApplyModel.StatusId;
                //    string title = "";

                //    workFlowModelPrevious = workFlowBal.getPreviousWorkflow(clientContext, fromstatus);
                //    workFlowModel = workFlowBal.getWorkFlowData(clientContext, actiontype, fromstatus, title);

                //    AM_AssetsApproverModel _AssetsApproverModelPrevious = new AM_AssetsApproverModel();
                //    AM_AssetsApproverModel _AssetsApproverModel = new AM_AssetsApproverModel();
                //    AM_AssetsApproverBal assetsApproverBal = new AM_AssetsApproverBal();

                //    _AssetsApproverModelPrevious = assetsApproverBal.GetAssetsApprover(clientContext, _AssetsApplyModel.ID, workFlowModelPrevious[0].ApproverRoleInternalName);
                //    _AssetsApproverModel = assetsApproverBal.GetAssetsApprover(clientContext, _AssetsApplyModel.ID, workFlowModel[0].ApproverRoleInternalName);

                //    // update asset data
                //    AM_AssetsApplyBal updateassets = new AM_AssetsApplyBal();

                //    string itemdata = " 'CurrentApprover' : '" + _AssetsApproverModel.ApproverCode + "',";
                //    itemdata += "'StatusId': '" + Convert.ToInt32(workFlowModel[0].ToStatusId) + "',";
                //    itemdata += "'InternalStatus': '" + workFlowModel[0].InternalStatus + "'";

                //    returnID = updateassets.UpdateAssets(clientContext, itemdata, (_AssetsApplyModel.ID).ToString());

                //    // update approver data
                //    string itemapprover = "'Status' : 'Approve'";

                //    assetsApproverBal.UpdateApprover(clientContext, itemapprover, (_AssetsApproverModelPrevious.ID).ToString());
                //}

                ////get current user empcode
                //string UserId = Session["UserID"].ToString();

                //basicInfoModels = basicInfoBal.GetCurrentLoginUser(clientContext, UserId);

                //// save history data
                //AM_AssetsHistoryModel historyModel = new AM_AssetsHistoryModel();

                //var todaydate = (assetAllotmentMainModel.todaydate).ToString("MM/dd/yyyy");

                //var statusid = "";

                //if(workFlowModel[0].ToStatusId!=null)
                //{
                //    statusid = workFlowModel[0].ToStatusId;
                //}
                //else
                //{
                //    statusid = "";
                //}

                //var itemhistory = " 'LIDId' : '" + _AssetsApplyModel.ID + "',";
                //itemhistory += "'ActionTakenId': '" + basicInfoModels[0].ID + "',";
                //itemhistory += "'Date': '" + todaydate + "',";
                //itemhistory += "'StatusId': '" + statusid + "',";
                //itemhistory += "'Comments': '" + assetAllotmentMainModel.comments + "'";

                //AM_AssetsHistoryBal assetsHistoryBal = new AM_AssetsHistoryBal();
                //assetsHistoryBal.SaveAssetsHistoryData(clientContext, itemhistory);

            }

            return Json(returnID, JsonRequestBehavior.AllowGet);
        }
    }
}
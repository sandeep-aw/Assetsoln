using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;
using AssetslnWeb.Models.EmployeeManagement;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class AssetViewController : Controller
    {

        [SharePointContextFilter]
        [HttpGet]
        // GET: AssetView
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
    }
}
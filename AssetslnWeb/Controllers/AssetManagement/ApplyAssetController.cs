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
        public ActionResult SaveAssetData()
        {
            string EmpName = Request["EmployeeName"];
            return View();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class ApplyAssetController : Controller
    {
        // GET: ApplyAsset
        public ActionResult Index()
        {
            List<AM_AssetsModel> AssetModelValue = new List<AM_AssetsModel>();
            List<AM_AssetsTypeModel> AssetModelTypeValue = new List<AM_AssetsTypeModel>();

            // get Asset Data
            AssetModelValue = GetAssetsModels();

            // assign asset data to viewbag
            ViewBag.AssetArr = AssetModelValue;

            return View();
        }

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

        private List<AM_AssetsModel> GetAssetsModels1()
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

    }
}
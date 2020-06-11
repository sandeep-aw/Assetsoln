using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;
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

            //List<AM_AssetCountHistoryModel> assetCountHistoryModel = new List<AM_AssetCountHistoryModel>();
            //AM_AssetCountHistoryBal assetCountHistoryBal = new AM_AssetCountHistoryBal();

            // previously assign quantity

            int prevAssignQty = 0;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                assetsApplyModel = assetsApplyBal.GetDataByID(clientContext, id);
                assetsHistoryModel = assetsHistoryBal.GetHistoryById(clientContext, id);

                ViewBag.assetsView = assetsApplyModel;
                ViewBag.assetsHistory = assetsHistoryModel;               

                //ViewBag.ReqQuantity = Convert.ToInt32(assetsApplyModel.AssetCount);

                //assetCountHistoryModel = assetCountHistoryBal.GetAssetCountHistory(clientContext, (assetsApplyModel.ID).ToString());

                //if(assetCountHistoryModel.Count>0)
                //{
                //    foreach(var j in assetCountHistoryModel)
                //    {
                //        int cnt = Convert.ToInt32((j.AssetCount).ToString());
                //        prevAssignQty += cnt;
                //    }
                //}

                //if(prevAssignQty!=0)
                //{
                //    ViewBag.prevQty = prevAssignQty;
                //}
                //else
                //{
                //    ViewBag.prevQty = "";
                //}

                ViewBag.prevQty = "";

                Session["assetsData"] = assetsApplyModel;
            }

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
        public ActionResult AllocateFunc(string Quantity, string CurrentDate)
        {
            string returnID = "0";

            AM_AssetsApplyModel assetsApplyModel = new AM_AssetsApplyModel();

            assetsApplyModel = (AM_AssetsApplyModel)Session["assetsData"];

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    if (assetsApplyModel.AssetCount == Quantity)
            //    {

            //    }
            //    else
            //    {
            //        AM_AssetsTypeModel assetsTypeModel = new AM_AssetsTypeModel();
            //        AM_AssetsTypeBal assetsTypeBal = new AM_AssetsTypeBal();

            //        int count = Convert.ToInt32(assetsApplyModel.AssetTypeStock) - Convert.ToInt32(Quantity);

            //        string itemdata = " 'Stock' : '" + count + "'";

            //        returnID = assetsTypeBal.UpdateAssetType(clientContext, itemdata, assetsApplyModel.AssetTypeId);

            //        AM_AssetCountHistoryBal assetCountHistoryBal = new AM_AssetCountHistoryBal();

            //        string actionType = "Allocate";

            //        string itemcount = " 'ActionType' : '" + actionType + "',";
            //        itemcount += "'AssetCount': '" + Quantity + "',";
            //        itemcount += "'LIDId': '" + assetsApplyModel.ID + "'";

            //        assetCountHistoryBal.SaveAssetCountHistory(clientContext, itemcount);
            //    }
            //}

            return Json(returnID, JsonRequestBehavior.AllowGet);
        }
    }
}
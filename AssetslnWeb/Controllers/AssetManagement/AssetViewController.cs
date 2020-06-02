using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;

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

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                assetsApplyModel = assetsApplyBal.GetDataByID(clientContext, id);
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
    }
}
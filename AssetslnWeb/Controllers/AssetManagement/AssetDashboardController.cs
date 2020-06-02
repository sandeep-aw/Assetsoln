using AssetslnWeb.BAL.AssetManagement;
using AssetslnWeb.Models.AssetManagement;
using AssetslnWeb.Models.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetslnWeb.Controllers.AssetManagement
{
    public class AssetDashboardController : Controller
    {
        // GET: AssetDashboard
        public ActionResult Index()
        {
            // get current use login id

            string UserId = Session["UserID"].ToString();

            // declare object to get current login user data
            List<AM_BasicInfoModels> EmpModel = new List<AM_BasicInfoModels>();

            // declare object to get pending request data
            List<AM_AssetsApplyModel> assetsApplyModels = new List<AM_AssetsApplyModel>();

            AM_AssetsApplyBal assetsApplyBals = new AM_AssetsApplyBal();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                AM_BasicInfoBal basicInfoBal = new AM_BasicInfoBal();
                EmpModel = basicInfoBal.GetCurrentLoginUser(clientContext, UserId);
                
                assetsApplyModels = assetsApplyBals.GetDataByEmpcode(clientContext,EmpModel[0].EmpCode);

                ViewBag.assetsDataArr = assetsApplyModels;
            }

            return View();
        }
    }
}
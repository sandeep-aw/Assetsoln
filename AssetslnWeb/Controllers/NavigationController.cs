using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using AssetslnWeb.DAL;
using AssetslnWeb.BAL;
using AssetslnWeb.Models;

namespace AssetslnWeb.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public ActionResult Index()
        {
            if (Session["Navigation"].ToString() == "" || Session["Navigation"].ToString() == null)
            {

                string ht = "";

                List<NavigationModel> Duplicatenavigation = new List<NavigationModel>();
                List<NavigationModel> navigation = new List<NavigationModel>();
                List<NavigationModel> Parrentnavigation = new List<NavigationModel>();
                List<NavigationModel> Subnavigation = new List<NavigationModel>();

                var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
                using (var clientContext = spContext.CreateUserClientContextForSPHost())
                {

                    UsersGroup usersGroup = new UsersGroup();
                    List<UserGroupModel> userGroupModels = new List<UserGroupModel>();
                    userGroupModels = usersGroup.GetGroup(clientContext, Session["UserID"].ToString());
                    NavigationBal navigationBal = new NavigationBal();


                    Duplicatenavigation = navigationBal.GetAllApproverType(clientContext, userGroupModels, Session["UserID"].ToString());

                    navigation = Duplicatenavigation.GroupBy(i => i.ID)
                                    .Select(g => g.First()).ToList();

                    for (int i = 0; i < navigation.Count; i++)
                    {
                        if (navigation[i].ParentManu == "0")
                        {
                            Parrentnavigation.Add(navigation[i]);
                        }
                        else
                        {
                            Subnavigation.Add(navigation[i]);
                        }
                    }


                    ht += "<nav class='horizontalMenu clearfix'>";
                
                    ht += "<ul class='horizontalMenu-list'>";
                    for (int i = 0; i < Parrentnavigation.Count; i++)
                    {
                        List<NavigationModel> Temp = new List<NavigationModel>();

                        for (int x = 0; x < Subnavigation.Count; x++)
                        {
                            if (Convert.ToString(Parrentnavigation[i].ID) == Subnavigation[x].ParentMenuNameID)
                            {
                                Temp.Add(Subnavigation[x]);
                            }
                        }

                        if (Temp.Count > 0)
                        {
                            ht += " <li aria-haspopup='true'>";
                            ht += "<a href='#' class='sub-icon'>";
                     
                            ht +=  Parrentnavigation[i].ManuName ;
                            ht += "<i class='fe fe-chevron-down horizontal-icon'></i></a>";

                            
                            ht += "<ul class='sub-menu'>";
                            for (int y = 0; y < Temp.Count; y++)
                            {  
                                ht += "<li aria-haspopup='true'><a href='" + Temp[y].URL + "' class='slide-item'> " + Temp[y].ManuName + "</a></li>";
                            }
                            ht += "</ul>";
                            
                            ht += "</li>";
                        }
                        else
                        {

                            ht += "<li aria-haspopup='true'>";
                            ht += "<a aria-haspopup='true' href='" + Parrentnavigation[i].URL + "'>";
                            ht += Parrentnavigation[i].ManuName ;
                            ht += "</a></li>";

                        }



                    }
                    ht += "</ul>";
                  
                    ht += "</nav>";

                    Session["Navigation"] = ht;
                }

            }




            return Json(Session["Navigation"].ToString(), JsonRequestBehavior.AllowGet);

        }
    }
}
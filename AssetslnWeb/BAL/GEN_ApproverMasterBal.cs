using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using AssetslnWeb.Models.AssetManagement;
using AssetslnWeb.Models.EmployeeManagement;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL
{
    public class GEN_ApproverMasterBal
    {
        public GEN_ApproverRoleNameModel getApproverData(ClientContext clientContext,string empcode,string module,string approvertype)
        {
            GEN_ApproverRoleNameModel approverRoleNameModel = new GEN_ApproverRoleNameModel();

            GEN_ApproverMasterModel approverMasterModel = new GEN_ApproverMasterModel();

            string filter = "Module eq '" + module + "' and Approver_Type eq '" + approvertype +"'";

            JArray jArray = RestGetApproverLevels(clientContext, filter);

            //List<GEN_ApproverRoleListModel> approverRoleListModels = new List<GEN_ApproverRoleListModel>();

            //GEN_ApproverRoleListBal approverRoleListBal = new GEN_ApproverRoleListBal();

            //approverRoleListModels = approverRoleListBal.GetApproverRoleListBals(clientContext);

            Emp_BasicInfoModels managerBasicInfo = new Emp_BasicInfoModels();            

            approverMasterModel = new GEN_ApproverMasterModel
            {
                ID = Convert.ToInt32(jArray[0]["ID"]),
                Module = jArray[0]["Module"] == null ? "" : Convert.ToString(jArray[0]["Module"]),
                Approver_Type = jArray[0]["Approver_Type"] == null ? "" : Convert.ToString(jArray[0]["Approver_Type"]),
                Rule_For_Filter_Type = jArray[0]["Rule_For_Filter_Type"] == null ? "" : Convert.ToString(jArray[0]["Rule_For_Filter_Type"]),
                Rule_For_Filter_Data = jArray[0]["Rule_For_Filter_Data"] == null ? "" : Convert.ToString(jArray[0]["Rule_For_Filter_Data"]),
                ApproverRoleName = jArray[0]["ApproverRoleName"] == null ? "" : Convert.ToString(jArray[0]["ApproverRoleName"]),
                ApproverRoleInternalName = jArray[0]["ApproverRoleInternalName"] == null ? "" : Convert.ToString(jArray[0]["ApproverRoleInternalName"])
            };

            List<string> rolenamearr = new List<string>();

            rolenamearr = approverMasterModel.ApproverRoleInternalName.Split(',').ToList();

            for (int i=0;i<rolenamearr.Count;i++)
            {
                if(rolenamearr[i] == "Manager")
                {

                }
                else if (rolenamearr[i] == "ManagersManager")
                {

                }
                else if (rolenamearr[i] == "DepartmentHead")
                {

                }
            }


            return approverRoleNameModel;
        }

        public JArray RestGetApproverLevels(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "Module,Approver_Type,Rule_For_Filter_Type,Rule_For_Filter_Data,ApproverRoleName,ApproverRoleInternalName";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "GEN_ApproverMaster", rESTOption);

            return jArray;
        }
    }
}
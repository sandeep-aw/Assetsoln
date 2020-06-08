using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL
{
    public class Emp_DepartmentBal
    {
        public Emp_DepartmentModel GetDepartmentHead(ClientContext clientContext, string department)
        {
            Emp_DepartmentModel departmentModel = new Emp_DepartmentModel();

            string filter = "DepartmentName eq '" + department + "'";

            JArray jArray = RESTGet(clientContext, filter);

            departmentModel = new Emp_DepartmentModel
            {
                ID = Convert.ToInt32(jArray[0]["ID"]),
                DepartmentName = jArray[0]["DepartmentName"] == null ? "" : Convert.ToString(jArray[0]["DepartmentName"]),
                Description = jArray[0]["Description"] == null ? "" : Convert.ToString(jArray[0]["Description"]),
                HeadOfDepartment = jArray[0]["HeadOfDepartment"]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["HeadOfDepartment"]["EmpCode"])              
            };

            return departmentModel;
        }

        private JArray RESTGet(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "DepartmentName,Description,HeadOfDepartment/EmpCode";
            rESTOption.expand = "HeadOfDepartment";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "Emp_Department", rESTOption);

            return jArray;
        }
    }
}
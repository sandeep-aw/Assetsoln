using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using AssetslnWeb.Models.EmployeeManagement;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL.AssetManagement
{
    public class Emp_BasicInfoBal
    {
        public Emp_BasicInfoModels GetEmpManager(ClientContext clientContext,string Empcode)
        {
            Emp_BasicInfoModels EmpBal = new Emp_BasicInfoModels();

            string filter = "EmpCode eq '" + Empcode + "'";

            JArray jArray = RestGetEmp(clientContext,filter);

            EmpBal = new Emp_BasicInfoModels
            {
                Id = Convert.ToInt32(jArray[0]["ID"]),
                EmpCode = jArray[0]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["EmpCode"]),
                UserNameId = jArray[0]["User_Name"]["Id"] == null ? "" : Convert.ToString(jArray[0]["User_Name"]["Id"]),
                User_Name = jArray[0]["User_Name"]["Title"] == null ? "" : Convert.ToString(jArray[0]["User_Name"]["Title"]).Trim(),
                Manger = jArray[0]["Manager"]["FirstName"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["FirstName"]),
                ManagerCode = jArray[0]["Manager"]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["EmpCode"]),
                Manager_Code = jArray[0]["Manager"]["ManagerCode"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["ManagerCode"]),
                Department = jArray[0]["Department"]["DepartmentName"] == null ? "" : Convert.ToString(jArray[0]["Department"]["DepartmentName"]),
                ManagerId = jArray[0]["Manager"]["Id"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["Id"]),
            };
           
            return EmpBal;
        }

        private JArray RestGetEmp(ClientContext clientContext,string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            string selectqry = "ID,FirstName,MiddleName,LastName,EmpCode,Gender,MaritalStatus,DOB,JoiningDate,OnProbationTill,ProbationStatus,OfficeEmail,";
            selectqry += "ContactNumber,EmpStatus,Designation/Designation,Department/DepartmentName,Division/Division,Region/Region,Branch/Branch,";
            selectqry += "Company/CompanyName,ManagerCode,User_Name/Id,User_Name/Title,Manager/FirstName,Manager/EmpCode,Manager/ManagerCode,Manager/Id";

            rESTOption.select = selectqry;
            rESTOption.expand = "Designation,Department,Division,Region,Branch,Company,User_Name,Manager";

            if(filter!=null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "Emp_BasicInfo", rESTOption);

            return jArray;
        }
    }
}
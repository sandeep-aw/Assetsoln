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
    public class AM_BasicInfoBal
    {
        public List<AM_BasicInfoModels> GetCreatedUserData(ClientContext clientContext,string uid)
        {
            List<AM_BasicInfoModels> EmpBal = new List<AM_BasicInfoModels>();

            string filter = "User_Name/Id eq '" + uid + "'";

            JArray jArray = RestGetEmp(clientContext,filter);

            foreach (JObject j in jArray)
            {
                EmpBal.Add(new AM_BasicInfoModels
                {
                    Id = Convert.ToInt32(j["ID"]),
                    EmpCode = j["EmpCode"] == null ? "" : Convert.ToString(j["EmpCode"]),
                    UserNameId = j["User_Name"]["Id"] == null ? "" : Convert.ToString(j["User_Name"]["Id"]),
                    User_Name = j["User_Name"]["Title"] == null ? "" : Convert.ToString(j["User_Name"]["Title"]).Trim(),
                });
            }

            return EmpBal;
        }

        public List<AM_BasicInfoModels> GetEmpData(ClientContext clientContext)
        {
            List<AM_BasicInfoModels> EmpBal = new List<AM_BasicInfoModels>();

            string filter = null;

            JArray jArray = RestGetEmp(clientContext,filter);

            foreach (JObject j in jArray)
            {
                EmpBal.Add(new AM_BasicInfoModels
                {
                    Id = Convert.ToInt32(j["ID"]),
                    FirstName = j["FirstName"] == null ? "" : Convert.ToString(j["FirstName"]),
                    MiddleName = j["MiddleName"] == null ? "" : Convert.ToString(j["MiddleName"]),
                    LastName = j["LastName"] == null ? "" : Convert.ToString(j["LastName"]),
                    EmpCode = j["EmpCode"] == null ? "" : Convert.ToString(j["EmpCode"]),
                    Gender = j["Gender"] == null ? "" : Convert.ToString(j["Gender"]),
                    MartialStatus = j["MaritalStatus"] == null ? "" : Convert.ToString(j["MaritalStatus"]),
                    DOB = j["DOB"] == null ? "" : Convert.ToString(j["DOB"]),
                    JoiningDate = j["JoiningDate"] == null ? "" : Convert.ToString(j["JoiningDate"]),
                    OnProbationTill = j["OnProbationTill"] == null ? "" : Convert.ToString(j["OnProbationTill"]),
                    ProbationStatus = j["ProbationStatus"] == null ? "" : Convert.ToString(j["ProbationStatus"]),
                    OfficeEmail = j["OfficeEmail"] == null ? "" : Convert.ToString(j["OfficeEmail"]),
                    ContactNumber = j["ContactNumber"] == null ? "" : Convert.ToString(j["ContactNumber"]),
                    EmpStatus = j["EmpStatus"] == null ? "" : Convert.ToString(j["EmpStatus"]),
                    Designation = j["Designation"]["Designation"] == null ? "" : Convert.ToString(j["Designation"]["Designation"]),
                    Department = j["Department"]["DepartmentName"] == null ? "" : Convert.ToString(j["Department"]["DepartmentName"]),
                    Division = j["Division"]["Division"] == null ? "" : Convert.ToString(j["Division"]["Division"]),
                    Region = j["Region"]["Region"] == null ? "" : Convert.ToString(j["Region"]["Region"]),
                    Branch = j["Branch"]["Branch"] == null ? "" : Convert.ToString(j["Branch"]["Branch"]),
                    Company = j["Company"]["CompanyName"] == null ? "" : Convert.ToString(j["Company"]["CompanyName"]),
                    Manager_Code = j["Manager_Code"]== null ? "" : Convert.ToString(j["Manager_Code"]),
                    UserNameId = j["User_Name"]["Id"] == null ? "" : Convert.ToString(j["User_Name"]["Id"]),
                    User_Name = j["User_Name"]["Title"] == null ? "" : Convert.ToString(j["User_Name"]["Title"]).Trim(),
                    Manger = j["Manager"]["FirstName"] == null ? "" : Convert.ToString(j["Manager"]["FirstName"])
                });
            }

            return EmpBal;
        }

        private JArray RestGetEmp(ClientContext clientContext,string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            string selectqry = "ID,FirstName,MiddleName,LastName,EmpCode,Gender,MaritalStatus,DOB,JoiningDate,OnProbationTill,ProbationStatus,OfficeEmail,";
            selectqry += "ContactNumber,EmpStatus,Designation/Designation,Department/DepartmentName,Division/Division,Region/Region,Branch/Branch,";
            selectqry += "Company/CompanyName,Manager_Code,User_Name/Id,User_Name/Title,Manager/FirstName";

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
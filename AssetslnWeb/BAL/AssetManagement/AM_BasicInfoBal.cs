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
        public AM_BasicInfoModel GetDataByEmpcode(ClientContext clientContext, string EmpCode)
        {
            AM_BasicInfoModel EmpBal = new AM_BasicInfoModel();

            string filter = "EmpCode eq '" + EmpCode + "'";

            JArray jArray = RESTGet(clientContext, filter);

            if (jArray.Count > 0)
            {
                EmpBal = new AM_BasicInfoModel
                {
                    ID = Convert.ToInt32(jArray[0]["ID"]),
                    EmpCode = jArray[0]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["EmpCode"]),
                    UserNameId = jArray[0]["User_Name"]["Id"] == null ? "" : Convert.ToString(jArray[0]["User_Name"]["Id"]),
                    User_Name = jArray[0]["User_Name"]["Title"] == null ? "" : Convert.ToString(jArray[0]["User_Name"]["Title"]).Trim(),
                };
            }

            return EmpBal;
        }

        public List<AM_BasicInfoModel> GetCurrentLoginUser(ClientContext clientContext,string uid)
        {
            List<AM_BasicInfoModel> EmpBal = new List<AM_BasicInfoModel>();

            string filter = "User_Name/Id eq '" + uid + "'";

            JArray jArray = RESTGet(clientContext,filter);

            if (jArray.Count > 0)
            {
                foreach (JObject j in jArray)
                {
                    EmpBal.Add(new AM_BasicInfoModel
                    {
                        ID = Convert.ToInt32(j["ID"]),
                        EmpCode = j["EmpCode"] == null ? "" : Convert.ToString(j["EmpCode"]),
                        UserNameId = j["User_Name"]["Id"] == null ? "" : Convert.ToString(j["User_Name"]["Id"]),
                        User_Name = j["User_Name"]["Title"] == null ? "" : Convert.ToString(j["User_Name"]["Title"]).Trim(),
                    });
                }
            }

            return EmpBal;
        }

        public List<AM_BasicInfoModel> GetEmpData(ClientContext clientContext)
        {
            List<AM_BasicInfoModel> EmpBal = new List<AM_BasicInfoModel>();

            string filter = null;

            JArray jArray = RESTGet(clientContext,filter);

            if (jArray.Count > 0)
            {
                foreach (JObject j in jArray)
                {
                    EmpBal.Add(new AM_BasicInfoModel
                    {
                        ID = Convert.ToInt32(j["ID"]),
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
                        Manager_Code = j["ManagerCode"] == null ? "" : Convert.ToString(j["ManagerCode"]),
                        UserNameId = j["User_Name"]["Id"] == null ? "" : Convert.ToString(j["User_Name"]["Id"]),
                        User_Name = j["User_Name"]["Title"] == null ? "" : Convert.ToString(j["User_Name"]["Title"]).Trim(),
                        Manger = j["Manager"]["FirstName"] == null ? "" : Convert.ToString(j["Manager"]["FirstName"])
                    });
                }
            }

            return EmpBal;
        }

        public List<AM_BasicInfoModel> GetEmpByManager(ClientContext clientContext,string mid)
        {
            List<AM_BasicInfoModel> EmpBal = new List<AM_BasicInfoModel>();

            string filter = "ManagerId eq '" + mid + "'";

            JArray jArray = RESTGet(clientContext, filter);

            if (jArray.Count > 0)
            {
                foreach (JObject j in jArray)
                {
                    EmpBal.Add(new AM_BasicInfoModel
                    {
                        ID = Convert.ToInt32(j["ID"]),
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
                        Manager_Code = j["ManagerCode"] == null ? "" : Convert.ToString(j["ManagerCode"]),
                        UserNameId = j["User_Name"]["Id"] == null ? "" : Convert.ToString(j["User_Name"]["Id"]),
                        User_Name = j["User_Name"]["Title"] == null ? "" : Convert.ToString(j["User_Name"]["Title"]).Trim(),
                        Manger = j["Manager"]["FirstName"] == null ? "" : Convert.ToString(j["Manager"]["FirstName"]),
                        ManagerId = jArray[0]["Manager"]["Id"] == null ? 0 : Convert.ToInt32(jArray[0]["Manager"]["Id"].ToString())
                    });
                }
            }

            return EmpBal;
        }

        public AM_BasicInfoModel GetEmpManager(ClientContext clientContext, string Empcode)
        {
            AM_BasicInfoModel EmpBal = new AM_BasicInfoModel();

            string filter = "EmpCode eq '" + Empcode + "'";

            JArray jArray = RESTGet(clientContext, filter);

            if (jArray.Count > 0)
            {
                EmpBal = new AM_BasicInfoModel
                {
                    ID = Convert.ToInt32(jArray[0]["ID"]),
                    EmpCode = jArray[0]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["EmpCode"]),
                    UserNameId = jArray[0]["User_Name"]["Id"] == null ? "" : Convert.ToString(jArray[0]["User_Name"]["Id"]),
                    User_Name = jArray[0]["User_Name"]["Title"] == null ? "" : Convert.ToString(jArray[0]["User_Name"]["Title"]).Trim(),
                    Manger = jArray[0]["Manager"]["FirstName"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["FirstName"]),
                    ManagerCode = jArray[0]["Manager"]["EmpCode"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["EmpCode"]),
                    Manager_Code = jArray[0]["Manager"]["ManagerCode"] == null ? "" : Convert.ToString(jArray[0]["Manager"]["ManagerCode"]),
                    Department = jArray[0]["Department"]["DepartmentName"] == null ? "" : Convert.ToString(jArray[0]["Department"]["DepartmentName"]),
                    ManagerId = jArray[0]["Manager"]["Id"] == null ? 0 : Convert.ToInt32(jArray[0]["Manager"]["Id"].ToString())
                };
            }

            return EmpBal;
        }

        private JArray RESTGet(ClientContext clientContext, string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            string selectqry = "ID,FirstName,MiddleName,LastName,EmpCode,Gender,MaritalStatus,DOB,JoiningDate,OnProbationTill,ProbationStatus,OfficeEmail,";
            selectqry += "ContactNumber,EmpStatus,Designation/Designation,Department/DepartmentName,Division/Division,Region/Region,Branch/Branch,";
            selectqry += "Company/CompanyName,ManagerCode,User_Name/Id,User_Name/Title,Manager/FirstName,Manager/EmpCode,Manager/ManagerCode,Manager/Id";

            rESTOption.select = selectqry;
            rESTOption.expand = "Designation,Department,Division,Region,Branch,Company,User_Name,Manager";

            if (filter != null)
            {
                rESTOption.filter = filter;
            }

            jArray = restService.GetAllItemFromList(clientContext, "Emp_BasicInfo", rESTOption);

            return jArray;
        }
    }
}
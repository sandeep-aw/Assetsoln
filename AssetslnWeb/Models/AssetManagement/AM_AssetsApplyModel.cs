using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetsApplyModel
    {
        public int ID { get; set; }

        public string RequestNo { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public string CreatedName { get; set; }

        public string CreatedCode { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Status { get; set; }

        public string InternalStatus { get; set; }

        public string CurrentDate { get; set; }

        public string CurrentApprover { get; set; }
        
        public string StatusId { get; set; }

        public string EmployeeNameId { get; set; }

        public string CreatedNameId { get; set; }

        public string fname { get; set; }

        public string lname { get; set; }

        public string AssetTypeStock { get; set; }

        public List<AM_AssetsApplyDetailsModel> applyDetailsModel { get; set; }
    }
}
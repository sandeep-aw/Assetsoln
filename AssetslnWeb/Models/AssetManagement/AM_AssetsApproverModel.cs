using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetsApproverModel
    {
        public int ID { get; set; }

        public string LID { get; set; }

        public string ApproverID { get; set; }

        public string ApproverCode { get; set; }

        public string ApproverRoleInternalName { get; set; }

        public string Status { get; set; }
    }
}
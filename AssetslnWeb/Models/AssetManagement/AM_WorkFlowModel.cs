using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_WorkFlowModel
    {
        public int ID { get; set; }

        public string ActionType { get; set; }

        public string FromStatus { get; set; }

        public string ToStatus { get; set; }

        public string InternalStatus { get; set; }

        public string ApproverRoleName { get; set; }

        public string StatusId { get; set; }
    }
}
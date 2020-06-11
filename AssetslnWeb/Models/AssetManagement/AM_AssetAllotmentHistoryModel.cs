using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetAllotmentHistoryModel
    {
        public int ID { get; set; }

        public string LID { get; set; }

        public string ActionType { get; set; }

        public string AssetQuantity { get; set; }

        public string PendingQuantity { get; set; }
    }
}
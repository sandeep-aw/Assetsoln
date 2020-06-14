using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetsHistoryModel
    {
        public int ID { get; set; }

        public string LID { get; set; }

        public string ActionTaken { get; set; }

        public string Status { get; set; }

        public DateTime CurrentDate { get; set; }

        public string Comments { get; set; }

        public string ActionTakenId { get; set; }

        public string StatusId { get; set; }
    }
}
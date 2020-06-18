using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetAllotmentMainModel
    {
        public DateTime todaydate { get; set; }

        public string comments { get; set; }

        public List<AM_AssetAllotmentHistoryModel> assetAllotmentHistoryModels { get; set; }
    }
}
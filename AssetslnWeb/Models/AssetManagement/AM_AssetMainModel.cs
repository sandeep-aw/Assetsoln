using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetMainModel
    {
        public IEnumerable<AM_AssetsApplyModel> PendingRequest { get; set; }

        public IEnumerable<AM_AssetsApplyModel> MyRequest { get; set; }

    }
}
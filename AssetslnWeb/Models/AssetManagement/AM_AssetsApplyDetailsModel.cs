using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetsApplyDetailsModel
    {
        public int ID { get; set; }

        public string Asset { get; set; }

        public string AssetType { get; set; }

        public string UserApplyQuantity { get; set; }

        public string AssetDetails { get; set; }

        public string ReasonToApply { get; set; }

        public string Replacement { get; set; }

        public string AssetId { get; set; }

        public string AssetTypeId { get; set; }

        public string UserAllottedtQuantity { get; set; }

        public string UserReturntQuantity { get; set; }

        public string UserBalancetQuantity { get; set; }
    }
}
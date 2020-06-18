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

        public string ProdSrNo { get; set; }

        public string ModelNo { get; set; }

        public DateTime WarrantyDate { get; set; }

        public string Remark { get; set; }

        public string PrevAssignQty { get; set; }

        public string UserQty { get; set; }

        public string UserApplyQuantity { get; set; }

        public string UserAllottedtQuantity { get; set; }

        public string UserReturnQuantity { get; set; }

        public string UserBalanceQuantity { get; set; }

        public string asset { get; set; }

        public string assetType { get; set; }

    }
}
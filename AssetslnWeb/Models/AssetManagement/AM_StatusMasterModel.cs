using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_StatusMasterModel
    {
        public int ID { get; set; }

        public string StatusName { get; set; }

        public string InternalStatus { get; set; }
    }
}
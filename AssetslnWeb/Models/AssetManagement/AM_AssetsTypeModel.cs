using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.Models.AssetManagement
{
    public class AM_AssetsTypeModel
    {
        public int ID { get; set; }

        public string AssetType { get; set; }

        public string Assets { get; set; }

        public string Stock { get; set; }

        public string MinStock { get; set; }
    }
}
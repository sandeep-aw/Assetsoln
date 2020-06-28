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

        public IEnumerable<AM_BasicInfoModel> BasicInfoModels { get; set; }

        public IEnumerable<AM_AssetsModel> assetsModels { get; set; }

        public AM_AssetsApplyModel assetsApplyModel { get; set; }

        public IEnumerable<AM_AssetsApplyDetailsModel> assetsApplyDetailsModels { get; set; }

        public IEnumerable<AM_AssetsHistoryModel> assetsHistoryModels { get; set; }

        public IEnumerable<string> ApproverNames;
    }
}
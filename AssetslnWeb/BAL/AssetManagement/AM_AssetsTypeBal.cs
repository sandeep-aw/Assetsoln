using AssetslnWeb.DAL;
using AssetslnWeb.Models;
using AssetslnWeb.Models.AssetManagement;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetslnWeb.BAL.AssetManagement
{
    public class AM_AssetsTypeBal
    {
        public List<AM_AssetsTypeModel> GetAssetsType(ClientContext clientContext,string assetType)
        {
            List<AM_AssetsTypeModel> assetsModelsTypeBal = new List<AM_AssetsTypeModel>();
            string filter = "Assets/ID eq '" + assetType + "'";

            JArray jArray = RestGetAssetsType(clientContext, filter);

            foreach (JObject j in jArray)
            {
                assetsModelsTypeBal.Add(new AM_AssetsTypeModel
                {
                    ID = Convert.ToInt32(j["ID"]),
                    Assets = j["Assets"]["Assets"] == null ? "" : Convert.ToString(j["Assets"]["Assets"]),
                    AssetType = j["AssetType"] == null ? "" : Convert.ToString(j["AssetType"])
                });
            }
            
            return assetsModelsTypeBal;
        }

        private JArray RestGetAssetsType(ClientContext clientContext,string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,AssetType,Assets/ID,Assets/Assets";
            rESTOption.filter = filter;
            rESTOption.expand = "Assets";

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetsType", rESTOption);

            return jArray;
        }
    }
}
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
        public AM_AssetsTypeModel GetAssetsStock(ClientContext clientContext, string id)
        {
            AM_AssetsTypeModel assetsModelsTypeBal = new AM_AssetsTypeModel();
            string filter = "ID eq '" + id + "'";

            JArray jArray = RestGetAssetsType(clientContext, filter);

            assetsModelsTypeBal = new AM_AssetsTypeModel()
            {
                ID = Convert.ToInt32(jArray[0]["ID"]),
                Assets = jArray[0]["Assets"]["Assets"] == null ? "" : Convert.ToString(jArray[0]["Assets"]["Assets"]),
                AssetType = jArray[0]["AssetType"] == null ? "" : Convert.ToString(jArray[0]["AssetType"]),
                Stock = jArray[0]["Stock"] == null ? "" : Convert.ToString(jArray[0]["Stock"]),
                MinStock = jArray[0]["MinStock"] == null ? "" : Convert.ToString(jArray[0]["MinStock"])
            };

            return assetsModelsTypeBal;
        }

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
                    AssetType = j["AssetType"] == null ? "" : Convert.ToString(j["AssetType"]),
                    Stock = j["Stock"] == null ? "" : Convert.ToString(j["Stock"]),
                    MinStock = j["MinStock"] == null ? "" : Convert.ToString(j["MinStock"])
                });
            }
            
            return assetsModelsTypeBal;
        }

        //RESTUpdate
        public string UpdateAssetType(ClientContext clientContext, string ItemData, string ID)
        {

            string response = RESTUpdate(clientContext, ItemData, ID);

            return response;
        }

        private JArray RestGetAssetsType(ClientContext clientContext,string filter)
        {
            RestService restService = new RestService();
            JArray jArray = new JArray();
            RESTOption rESTOption = new RESTOption();

            rESTOption.select = "ID,AssetType,Assets/ID,Assets/Assets,Stock,MinStock";
            rESTOption.filter = filter;
            rESTOption.expand = "Assets";

            jArray = restService.GetAllItemFromList(clientContext, "AM_AssetsType", rESTOption);

            return jArray;
        }

        private string RESTUpdate(ClientContext clientContext, string ItemData, string ID)
        {
            RestService restService = new RestService();

            return restService.UpdateItem(clientContext, "AM_AssetsType", ItemData, ID);
        }
    }
}
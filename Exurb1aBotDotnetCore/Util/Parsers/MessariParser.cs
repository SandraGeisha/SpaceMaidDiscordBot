using Exurb1aBot.Model.ViewModel.MessariModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Exurb1aBot.Util.Parsers {
    public class MessariParser {
        private const string apikey = "955b25d5-5a72-4754-aedd-f159742d7f54";

        public static async Task<Asset> GetAsset(string symbol) {
            HttpClient client = ApiHelper.APIClient;
            string iconurl = $"https://cryptoicons.org/api/icon/{symbol}/400";

            HttpResponseMessage result = await client.GetAsync(iconurl);
            if (!result.IsSuccessStatusCode) {
                iconurl = "https://pbs.twimg.com/profile_images/1094823834451103744/BCHmufYd_400x400.jpg";
            }

            string url = $"https://data.messari.io/api/v1/assets/{symbol}/metrics/market-data";
            client.DefaultRequestHeaders.Add("x-messari-api-key", apikey);

            result = await client.GetAsync(url);
            if (result.IsSuccessStatusCode) { 
                Asset asset = await result.Content.ReadAsAsync<Asset>();
                asset.Data.IconUrl = iconurl;
                return asset;
            }

            return null;
        }
    }
}

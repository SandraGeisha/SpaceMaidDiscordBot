using Exurb1aBot.Util.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exurb1aBot.Model.ViewModel.MessariModels {
    public class Asset {
        public AssetData Data { get; set; }
    }

    public class AssetData {
        public string Symbol { get; set; }
        public string IconUrl { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "market_data")]
        public MarketData MarketData { get; set; }
    }

    public class MarketData {
        private static readonly Dictionary<decimal, string> positiveStatus = new Dictionary<decimal, string>() {
            { 50M,"WSB!!"}, { 30M, "Pump"}, { 25M, "Big increase"}, { 15M, "Moderate increase"}, { 5M, "Slight increase"}
        };       
        
        private static readonly Dictionary<decimal, string> negativeStatus = new Dictionary<decimal, string>() {
            { -50M,"CRASH!!"}, { -30M, "Dump"}, { -25M, "Big decrease"}, { -15M, "Moderate decrease"}, { -5M, "Slight decrease"}
        };
        
        [JsonProperty(PropertyName = "price_usd")]
        public decimal PriceUSD { get; set; }
        [JsonProperty(PropertyName = "price_btc")]
        public decimal PriceBTC { get; set; }
        [JsonProperty(PropertyName = "price_eth")]
        public decimal PriceETH { get; set; }
        [JsonProperty(PropertyName = "percent_change_usd_last_1_hour")]
        public decimal PercentUSD1h { get; set; }
        [JsonProperty(PropertyName = "percent_change_usd_last_24_hours")]
        public decimal PercentUSD24h { get; set; }

        public string Status1h { get => ExtensionMethods.GetClosestValue(positiveStatus, negativeStatus, PercentUSD1h); }
        public string Status24h { get => ExtensionMethods.GetClosestValue(positiveStatus, negativeStatus, PercentUSD24h); }
    }
}

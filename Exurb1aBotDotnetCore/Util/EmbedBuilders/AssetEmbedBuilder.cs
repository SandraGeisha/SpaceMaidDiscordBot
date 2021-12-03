using Discord;
using Discord.Commands;
using Exurb1aBot.Model.ViewModel.MessariModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    public static class AssetEmbedBuilder {

        public static async Task<EmbedBuilder> MakeAssetEmbed(Asset model, ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = (model.Data.MarketData.PercentUSD24h >= 0) ? Color.Green : Color.Red,
                Title = $"{model.Data.Name}",
                ThumbnailUrl = model.Data.IconUrl
            };

            emb.Description = $"Stonk data for {model.Data.Name}";

            emb.AddField("Current price USD", model.Data.MarketData.PriceUSD.ToString("C8", CultureInfo.CreateSpecificCulture("en-US")));

            emb.AddField("24 hours status", model.Data.MarketData.Status24h, true);
            emb.AddField("1 hours status", model.Data.MarketData.Status1h, true);


            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.PercentDecimalDigits = 8;

            emb.AddField("24 hours percentage", (model.Data.MarketData.PercentUSD24h / 100).ToString("P",nfi), true);
            emb.AddField("1 hours percentage", (model.Data.MarketData.PercentUSD1h / 100).ToString("P", nfi), true);

            emb.AddField("BTC price",$"{model.Data.MarketData.PriceBTC.ToString("F8", CultureInfo.InvariantCulture)} ₿", true);
            emb.AddField("ETH price", $"{model.Data.MarketData.PriceETH.ToString("F8", CultureInfo.InvariantCulture)} Ξ", true);

            EmbedFooterBuilder footer = await EmbedBuilderFunctions.AddFooter(context);
            footer.Text += " - Powered by Messari";
            emb.WithFooter(footer);

            return emb;
        }
    }
}

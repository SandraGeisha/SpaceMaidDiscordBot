using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    public class CurrencyEmbedBuilder {
        public static async Task<string> MakeStringRanking(IEnumerable<EntityUser> _users,int page, ICommandContext context) {
            string res = ":heavy_dollar_sign: Currency Ranks For exurb1a Discord :heavy_dollar_sign:"
                + "\n```css\n-------------------------------------\n";
            
            int rank = (10 * (page - 1))+1;


            foreach (EntityUser user in _users) {
                double complexAmount = user.GetCurrencyAmount(CurrencyEnum.Complexity).RoundDownTwoDecimals();

                double entropyAmount = user.GetCurrencyAmount(CurrencyEnum.Entropy).RoundDownTwoDecimals();
                IGuildUser usr = await context.Guild.GetUserAsync(user.Id);

                res += $"[{rank}]\t" + "{" + $" {usr?.Nickname ?? user.Username}:\t '{complexAmount}🚀(C)\t" +
                    $"{entropyAmount} 🌕(E)'" + "}\n";

                rank += 1;
            }

            res += "-------------------------------------\n```";
            
            return res;
        }

        public static async Task<EmbedBuilder> MakeRedeemEmbed(ICommandContext context,EntityUser eu,double amount) {
            var user = context.User as IGuildUser;
            amount = Math.Round(amount, 2);

            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = $"{user.Nickname ?? user.Username} has redeemed {amount} Entropy",
                Footer = await EmbedBuilderFunctions.AddFooter(context),
                ThumbnailUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl(),
                Description = $"Redeemed {amount} Entropy \n" +
                $"Current balance:\t{eu.GetCurrencyAmount(CurrencyEnum.Complexity).RoundDownTwoDecimals()}🚀(C)\t "
                +$"{eu.GetCurrencyAmount(CurrencyEnum.Entropy).RoundDownTwoDecimals()}🌕(E)"
            };

            return emb;
        }

        public static async Task<EmbedBuilder> MakeBalanceEmbed(ICommandContext context, EntityUser eu) {
            var user = await context.Guild.GetUserAsync(eu.Id);

            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = $"Balance for {user.Nickname ?? user.Username}",
                Footer = await EmbedBuilderFunctions.AddFooter(context),
                ThumbnailUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl(),
                Description =  $"Current balance:\t{eu.GetCurrencyAmount(CurrencyEnum.Complexity).RoundDownTwoDecimals()}🚀(C)\t "
                + $"{eu.GetCurrencyAmount(CurrencyEnum.Entropy).RoundDownTwoDecimals()}🌕(E)"
            };

            emb.WithCurrentTimestamp();
            return emb;
        }

        public static async Task<EmbedBuilder> BuildCurrencyHelpEmbed(ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = "Currency Command help",
                Description = "Commands that have to do with hording Sheqels for all your octomoist purchases.",
                ThumbnailUrl = "https://cdn.apk4free.net/wp-content/uploads/2018/04/bitcoin.png",
                Footer = await EmbedBuilderFunctions.AddFooter(context)
            };

            emb.AddField("Command name", "Currency",true);
            emb.AddField("Parameters", "top,balance,redeem,exchange,help,currencies,transfer",true);

            emb.AddField("Examples", "currency top\ncurrency top 69\ncurrency help"
                + "\ncurrency redeem\ncurrency exchange 1000 Entropy Complexity\n" +
                "currency exchange 1000 E C\ncurrency exchange 1 C E\ncurrency exchange 1,25 C E\n" +
                "currency currencies\ncurrency transfer 100 Entropy @Margret@0027"+
                "\ncurrency transfer 0,25 C @Margret@0027", false);

            emb.AddField("Remarks", "You can only redeem every 24 hours.\n" +
                "Everyone is entitled to 300 🌕(E) starting credits type currency redeem to get them.\n" +
                "The first argument of currency exchange is the amount you'll be exchanging, the second argument is "
                + "the currency you'll be exchanging and the third argument is the currency you'll be converting to\n"
                + "You can only use a , to represent a comma number for an exchange and transfer E.G. 1,5 and not 1.5 ",false);

            emb.WithCurrentTimestamp();

            return emb;
        }

        public static async Task<EmbedBuilder> MakeTradeEmbed(double addAmount, double removeAmount
            , ICommandContext context,CurrencyEnum addCurrency, CurrencyEnum rmCurrency) {
            var user = context.User as IGuildUser;

            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = $"Exchange by {user.Nickname ?? user.Username} successful",
                Description = $"{user.Nickname ?? user.Username} exchanged {removeAmount.RoundDownTwoDecimals()} {rmCurrency}"
                + $" to {addAmount.RoundDownTwoDecimals()} {addCurrency}",
                ThumbnailUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl(),
                Footer = await EmbedBuilderFunctions.AddFooter(context)
            };

            return emb;
        }

        public static async Task<EmbedBuilder> MakeCurrencyError(string[] currencies,ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Red,
                Description = "One of the specified currencies was not found",
                Title = "Error with the currencies",
                Footer = await EmbedBuilderFunctions.AddFooter(context),
                ThumbnailUrl = "https://cache.desktopnexus.com/thumbseg/541/541559-bigthumbnail.jpg"
            };

            emb.AddField("Possible currencies :heavy_dollar_sign:", string.Join(",", currencies),true);
            emb.AddField("Remark", "You don't have to type out the full name of the " +
                "currency just the first letter already suffices. (case insensitive)");
            emb.AddField("See more", $"For a better list of the currencies you can run {Program.prefix}" +
                "currency currencies");

            emb.WithCurrentTimestamp();

            return emb;
        }

        public static async Task<EmbedBuilder> ShowExchangeRates(string[] currencies ,ICommandContext context) {
            var emb = new EmbedBuilder() {
                Color = Color.Teal,
                Footer = await EmbedBuilderFunctions.AddFooter(context),
                Title = "Currencies and exchange rates",
                Description = "A listing of available currencies and their exchange rates.",
                ThumbnailUrl = "https://dogwatchcafe.com/wp-content/uploads/2017/04/currency_exchange1600.png"
            };

            emb.WithCurrentTimestamp();
            emb.AddField("Currencies", string.Join(",", currencies),true);

            string s = "";

            for(int i= 0; i < currencies.Length; i += 2) {
                var cur = currencies[0];
                var cur2 = currencies[1];
                int A = (int)Enum.Parse(typeof(ConversionRate), $"{cur}To{cur2}");
                int B = (int)Enum.Parse(typeof(ConversionRate), $"{cur2}To{cur}");
                s += $"{A} {cur} = {B} {cur2}\n";
            }

            emb.AddField("Exchange Rates", s, false);

            return emb;
        }

        public static async Task<EmbedBuilder> GetTransferEmbed(ICommandContext context,EntityUser eu,EntityUser own,CurrencyEnum cu,double amount) {

            IGuildUser owner = context.User as IGuildUser;
            IGuildUser transferee = await context.Guild.GetUserAsync(eu.Id);

            string ownerName = owner.Nickname ?? owner.Username;
            string transferName = transferee.Nickname ?? transferee.Username;

            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = $"Transferred {amount.RoundDownTwoDecimals()} {cu} succesfully",
                Footer = await EmbedBuilderFunctions.AddFooter(context),
                ThumbnailUrl = "https://static.thenounproject.com/png/175569-200.png"
            };

            var desc = $"{ownerName} succesfully transferred {amount} {cu} to {transferName}";

            emb.Description = desc;
            emb.WithCurrentTimestamp();

            emb.AddField($"{ownerName}s Balance", $"\t{own.GetCurrencyAmount(CurrencyEnum.Complexity).RoundDownTwoDecimals()}🚀(C)\t" 
                + $"{own.GetCurrencyAmount(CurrencyEnum.Entropy).RoundDownTwoDecimals()}🌕(E)", false);

            emb.AddField($"{transferName}s Balance", $"\t{eu.GetCurrencyAmount(CurrencyEnum.Complexity).RoundDownTwoDecimals()}🚀(C)\t"
                + $"{eu.GetCurrencyAmount(CurrencyEnum.Entropy).RoundDownTwoDecimals()}🌕(E)", false);

            return emb;
        }
    }
}

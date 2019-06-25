using Discord;
using Discord.Commands;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Util.EmbedBuilders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exurb1aBot.Modules {
    [Name("Currency Commands")]
    [Group("Currency"), Summary("The Currency command, accepted methods top")]
    [Alias("cu")]
    public class CurrencyModule : ModuleBase<SocketCommandContext> {
        private readonly IUserRepository _userRepo;
        private static readonly double RedeemAmount = 5.27;
        private static readonly double StartingAmount = 300;

        public CurrencyModule(IUserRepository userRepo) {
            _userRepo = userRepo;
        }

        #region Top
        [Command("top")]
        [Alias("t")]
        public async Task Top() {
            await Top(1);
        }

        [Command("top")]
        [Alias("t")]
        public async Task Top(int page) {
            if (page > 1 && page * 10 > _userRepo.GetCountUsers())
                page = _userRepo.GetCountUsers() / 10;

            if (page < 1)
                page = 1;

            var users = _userRepo.GetRanking(page);
            string s = await CurrencyEmbedBuilder.MakeStringRanking(users, page, Context);
            await Context.Channel.SendMessageAsync(text: s);
        }

        [Command("top")]
        public async Task Top([Remainder] string s) {
            await Top(1);
        }
        #endregion

        #region Redeem
            [Command("redeem")]
            [Alias("r")]
            public async Task RedeemDaily() {
                var user = _userRepo.GetByID(Context.User.Id);
                bool firstTime = false;

                if (user == null) {
                    user = CheckIfExistAndCreate();
                    firstTime = true;
                }

                if (IsElligable(user) || firstTime) {
                    user.AddAmount(CurrencyEnum.Entropy, RedeemAmount);
                    user.RedeemedTime = DateTime.Now;

                    _userRepo.SaveChanges();

                    var emb = await CurrencyEmbedBuilder.MakeRedeemEmbed(Context, user, RedeemAmount);
                    await Context.Channel.SendMessageAsync(embed: emb.Build());
                }
                else {
                    var diff = user.RedeemedTime.AddDays(1).Subtract(DateTime.Now);
                    await Context.Channel.SendMessageAsync("You can't do that yet please wait another" +
                        $": {diff.ToString(@"hh\hmm\mss\s")}");
                }
            } 

            [Command("redeem")]
            [Alias("r")]
            public async Task RedeemDaily([Remainder]string s) {
                    await RedeemDaily();
            }
        #endregion

        #region Balance
        [Command("balance")]
        [Alias("b")]
        public async Task ShowBalance() {
            var user = _userRepo.GetByID(Context.User.Id);
            if (user == null)
                await ErrorUserDoesntExistInSystem();
            else
                await Context.Channel.SendMessageAsync(embed: CurrencyEmbedBuilder.MakeBalanceEmbed(Context, user)
                        .Result.Build());
        }

        [Command("balance")]
        [Alias("b")]
        public async Task ShowBalance(IGuildUser user) {
            var usr = _userRepo.GetByID(user.Id);
            if (usr == null)
                await ErrorOtherUserDoesntExistInSystem();
            else
                await Context.Channel.SendMessageAsync(embed: CurrencyEmbedBuilder.MakeBalanceEmbed(Context, usr)
                        .Result.Build());
        }

        [Command("balance")]
        [Alias("b")]
        public async Task ShowBalance(IGuildUser user,[Remainder] string s) {
            await ShowBalance(user);
        }

        [Command("balance")]
        [Alias("b")]
        public async Task ShowBalance([Remainder]string s) {
            await ShowBalance();
        }
        #endregion

        #region Help
            [Command("help")]
            [Alias("h")]
            public async Task DisplayHelp() {
                var embed = await CurrencyEmbedBuilder.BuildCurrencyHelpEmbed(Context);
                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }

            [Command("help")]
            [Alias("h")]
            public async Task DisplayHelp([Remainder] string s) {
                await DisplayHelp();
            }
        #endregion

        #region exchange
            [Command("exchange")]
            [Alias("e")]
            public async Task TradeCurrency(double amount, string cur1, string cur2,[Remainder] string s) {
            await TradeCurrency(amount, cur1, cur2);
            }

            [Command("exchange")]
            [Alias("e")]
            public async Task TradeCurrency(double amount, string cur1, string cur2) {

                if (amount <= 0) {
                    await Context.Channel.SendMessageAsync("You can't exchange  an invalid amount good try :^)");
                    return;
                }

                EntityUser eu = _userRepo.GetByID(Context.User.Id);

                if (eu == null) {
                    await ErrorUserDoesntExistInSystem();
                    return;
                }

                string[] names = Enum.GetNames(typeof(CurrencyEnum));
                var curToTrade = names.SingleOrDefault(n => n.ToLower().StartsWith(cur1.ToLower()));
                var curConvert = names.SingleOrDefault(n => n.ToLower().StartsWith(cur2.ToLower()));

                if (curToTrade == null || curConvert == null) {
                    var em = await CurrencyEmbedBuilder.MakeCurrencyError(names, Context);
                    await Context.Channel.SendMessageAsync(embed: em.Build());
                    return;
                }

                CurrencyEnum curr1 = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), curToTrade);

                if (eu.GetCurrencyAmount(curr1) < amount) {
                    await Context.Channel.SendMessageAsync($"You're {amount - Math.Round(eu.GetCurrencyAmount(curr1), 2)}"
                        + $" {curr1} short for that exchange.");
                    return;
                }

                CurrencyEnum curr2 = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), curConvert);

                ConversionRate cr1 = (ConversionRate)Enum.Parse(typeof(ConversionRate), $"{curToTrade}To{curConvert}");
                ConversionRate cr2 = (ConversionRate)Enum.Parse(typeof(ConversionRate), $"{curConvert}To{curToTrade}");

                var rate = (((double)(int)cr2) / ((double)(int)cr1));
                var addAmount = rate * amount;

                eu.RemoveAmount(curr1, amount);
                eu.AddAmount(curr2, addAmount);
                _userRepo.SaveChanges();

                var embed = await CurrencyEmbedBuilder.MakeTradeEmbed(addAmount, amount, Context, curr2, curr1);
                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }

            [Command("exchange")]
            [Alias("e")]
            public async Task TradeCurrency() {
                await EmbedBuilderFunctions.GiveErrorSyntax("currency exchange",
                    new string[] { "**amount**(required use a , for comma number) ",
                        "**currency**(required Currency you want to exchange (use -cu c for a list of available currencies))",
                        "**currency**(required Currency you want to convert to)"},
                    new string[] {  $"{Program.prefix}currency exchange 0,02 Complexity Entropy",
                         $"{Program.prefix}cu e 0,02 C E",
                         $"{Program.prefix}cu e 200 E C",
                         $"{Program.prefix}currency exchange 200 Entropy Complexity"
                    },
                    Context
                );
            }

            [Command("exchange")]
            [Alias("e")]
            public async Task TradeCurrency([Remainder]string s) {
                await TradeCurrency();
            }
        #endregion

        #region Currencies
            [Command("currencies")]
            [Alias("c")]
            public async Task ShowCurrencies() {
                string[] currencies = Enum.GetNames(typeof(CurrencyEnum));
                var emb = await CurrencyEmbedBuilder.ShowExchangeRates(currencies, Context);
                await Context.Channel.SendMessageAsync(embed: emb.Build());
            }

            [Command("currencies")]
            [Alias("c")]
            public async Task ShowCurrencies([Remainder]string s) {
                await ShowCurrencies();
            }
        #endregion

        #region Transfer
            [Command("transfer")]
            [Alias("tr")]
            public async Task TransferCurrency(double amount, string c, IGuildUser user) {

                if (amount == 0) {
                    await Context.Channel.SendMessageAsync($"Wow you gave nothing to {user.Nickname ?? user.Username},what are you Dutch, you greedy bastard");
                    return;
                }

                if (amount < 0) {
                    await Context.Channel.SendMessageAsync($"You can't transfer a negative amount to {user.Nickname ?? user.Username}, greedy stealing bastard.");
                    return;
                }

                EntityUser euO = _userRepo.GetByID(Context.User.Id);       
                
                if(euO == null) {
                    await ErrorUserDoesntExistInSystem();
                    return;
                }

                if (user.IsBot) {
                    await Context.Channel.SendMessageAsync("Why would you transfer money to a bot?");
                    return;
                }

                EntityUser euT = _userRepo.GetByID(user.Id);
                if (euT == null) {
                await ErrorOtherUserDoesntExistInSystem();
                    return;
                }

                string[] names = Enum.GetNames(typeof(CurrencyEnum));
                var curToTransfer = names.SingleOrDefault(n => n.ToLower().StartsWith(c.ToLower()));

                if (curToTransfer == null) {
                    var em = await CurrencyEmbedBuilder.MakeCurrencyError(names, Context);
                    await Context.Channel.SendMessageAsync(embed: em.Build());
                    return;
                }

                CurrencyEnum cu = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), curToTransfer);

                if (euO.GetCurrencyAmount(cu) < amount) {
                    await Context.Channel.SendMessageAsync($"You're {Math.Round((amount - euO.GetCurrencyAmount(cu)), 2)} " +
                        $"{cu} short to transfer this amount");
                    return;
                }

                euO.RemoveAmount(cu, amount);
                euT.AddAmount(cu, amount);
                _userRepo.SaveChanges();

                var emb = await CurrencyEmbedBuilder.GetTransferEmbed(Context, euT, euO, cu, amount);
                await Context.Channel.SendMessageAsync(embed: emb.Build());
            }

            [Command("transfer")]
            [Alias("tr")]
            public async Task TransferCurrency() {
                await EmbedBuilderFunctions.GiveErrorSyntax("currency transfer",
                    new string[] { "**amount**(required, in case of decimal use a ,)\n",
                          "**currency**(required Currency you want to exchange (use -cu c for a list of available currencies))",
                          "\n**user**(required, must be an @ mention)"}, 
                    new string[] { "currency transfer 100 entropy @Margret#0027",
                        "cu tr 100 e @Margret#0027","currency transfer 10,2 complexity @Margret#0027",
                        "cu tr 10,2 c @Margret#0027"
                    }, 
                    Context);
            }

            [Command("transfer")]
            [Alias("tr")]
            public async Task TransferCurrency([Remainder] string s) {
                await TransferCurrency();
            }

            [Command("transfer")]
            [Alias("tr")]
            public async Task TransferCurrency(double amount, string c, IGuildUser user,[Remainder] string s) {
                await TransferCurrency(amount, c, user);
            }
        #endregion

        #region HelperFunctions
            private bool IsElligable(EntityUser eu) {
                    return DateTime.Now.Subtract(eu.RedeemedTime).TotalHours >= 24;
            }

             private EntityUser CheckIfExistAndCreate() {
                EntityUser eu = new EntityUser(Context.User as IGuildUser);
                Context.Channel.SendMessageAsync(text: $"You've redeemed your {StartingAmount} starter Entropy");
                eu.AddAmount(CurrencyEnum.Entropy, StartingAmount);
                _userRepo.AddUser(eu);
                return eu;
             }

            private async Task ErrorUserDoesntExistInSystem() {
                await Context.Channel.SendMessageAsync($"You don't have a balance yet type `{Program.prefix}" +
                    "currency redeem` to get started.");
            }

            private async Task ErrorOtherUserDoesntExistInSystem() {
                await Context.Channel.SendMessageAsync($"This user doesn't have a balance yet they can type `{Program.prefix}" +
                "currency redeem` to get started.");
            }
        #endregion
    }
}

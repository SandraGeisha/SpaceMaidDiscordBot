using Discord;
using Discord.Commands;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Model.ViewModel.MessariModels;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Util.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Modules {
    [Name("Stock Commands")]
    [Group("stock")]
    public class StockModule : ModuleBase {
        private IStonkRepository _stonkRepo;
        private IUserRepository _userRepo;

        public StockModule(IStonkRepository repo, IUserRepository userRepository) {
            _stonkRepo = repo;
            _userRepo = userRepository;
        }

        [Command("add")]
        public async Task ErrorStockAdd([Remainder] string _ = "") {
            await EmbedBuilderFunctions.GiveErrorSyntax("stock add",
                new string[] { "number of stocks (required)", "buy in price USD (required)" ,"symbol (required)" },
                new string[] { $"{Program.prefix}stock add 15 0.052485 doge" }, Context) ;
        }

        [Command("add")]
        public async Task StockAdd(int quantity, decimal price, [Remainder] string symbol) {
            Asset asset = MessariParser.GetAsset(symbol).Result;
            
            if (asset == null) {
                await Context.Channel.SendMessageAsync("Stock not found");
            }

            EntityUser user = MakeUserIfNotExist(Context);
            Portfolio portfolio = _stonkRepo.GetPortfolio(user);

            if (portfolio == null) {
                portfolio = _stonkRepo.PortfolioAdd(user);
            }

            Stock stock = AddStockIfNotExist(symbol);

            portfolio.AddStock(stock, (uint)quantity, price);
            _stonkRepo.SaveChanges();

            await Context.Channel.SendMessageAsync("Portfolio added");
        }

        [Command("data")]
        public async Task GetPriceData([Remainder] string symbol="doge") {
            Asset asset = MessariParser.GetAsset(symbol).Result;
            
            if (asset == null) {
                await Context.Channel.SendMessageAsync("Couldn't find the asset you're looking for.");
                return;
            }

            await Context.Channel.SendMessageAsync(embed: AssetEmbedBuilder.MakeAssetEmbed(asset,Context).Result.Build());
        }

        private EntityUser MakeUserIfNotExist(ICommandContext context) {
            EntityUser user = _userRepo.GetUserById(Context.User.Id);

            if (user == null) {
                user = new EntityUser((IGuildUser)Context.User);
                _userRepo.AddUser(user);
                _userRepo.SaveChanges();
            }

            return user;
        }

        private Stock AddStockIfNotExist(string symbol) {
            if (!_stonkRepo.StockExist(symbol)) {
                return _stonkRepo.AddStock(symbol);
            }

            return _stonkRepo.GetStock(symbol);
        }
    }
}

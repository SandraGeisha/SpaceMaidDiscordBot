using Exurb1aBot.Model.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exurb1aBot.Model.Domain {
    public class Portfolio {
        public EntityUser User { get; set; }
        public ulong UserId { get; set; }
        public List<StockPortfolio> StockPortfolios { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Portfolio() {
            StockPortfolios = new List<StockPortfolio>();
        }

        public void AddStock(Stock stock, uint amount, decimal buyinamount) {
            StockPortfolios.Add(new StockPortfolio(stock, this, buyinamount, amount));
        }
    }
}

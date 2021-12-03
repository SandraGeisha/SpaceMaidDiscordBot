using System.ComponentModel.DataAnnotations.Schema;

namespace Exurb1aBot.Model.Domain {
    public class StockPortfolio {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Stock Stock { get; set; }
        public Portfolio Portfolio { get; set; }
        public int PortfolioID { get; set; }
        public string Symbol { get; set; }
        public decimal BuyInPrice { get; set; }
        public uint Amount { get; set; }

        //EF constructor
        protected StockPortfolio() {}

        public StockPortfolio(Stock stock, Portfolio portfolio, decimal buyinprice, uint amount) {
            Stock = stock;
            Portfolio = portfolio;
            BuyInPrice = buyinprice;
            Amount = amount;
            PortfolioID = portfolio.ID;
            Symbol = stock.Symbol;
        }
    }
}

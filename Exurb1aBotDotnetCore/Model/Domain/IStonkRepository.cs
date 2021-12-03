using Exurb1aBot.Model.ViewModel;

namespace Exurb1aBot.Model.Domain {
    public interface IStonkRepository {
        Portfolio PortfolioAdd(EntityUser user);
        bool PortfolioExist(EntityUser user);
        Portfolio GetPortfolio(EntityUser user);
        Stock AddStock(string symbol);
        bool StockExist(string symbol);
        Stock GetStock(string symbol);
        void SaveChanges();
    }
}

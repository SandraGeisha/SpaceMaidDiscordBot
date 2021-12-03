using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace Exurb1aBot.Data.Repository {
    class StonkRepository : IStonkRepository {
        private DbSet<Portfolio> _portfolios;
        private DbSet<Stock> _stocks;
        private ApplicationDbContext _context;

        public StonkRepository(ApplicationDbContext context) {
            _context = context;
            _portfolios = context.Portfolios;
            _stocks = context.Stocks;
        }

        public Stock AddStock(string symbol) {
            if (!StockExist(symbol)) {
                Stock stock = new Stock() {
                    Symbol = symbol,
                    Type = StockType.Crypto
                };

                _stocks.Add(stock);
                _context.SaveChanges();

                return stock;               
            }

            return GetStock(symbol);
        }

        public Portfolio GetPortfolio(EntityUser user) {
            return _portfolios.FirstOrDefaultAsync(p => p.UserId == user.Id).Result;
        }

        public Stock GetStock(string symbol) {
            return _stocks.FirstOrDefaultAsync(s => s.Symbol.ToLower().Equals(symbol.ToLower())).Result;
        }

        public Portfolio PortfolioAdd(EntityUser user) {
            var portfolio = _portfolios.Add(new Portfolio() {
                UserId = user.Id
            });

            _context.SaveChanges();
            return portfolio.Entity;
        }

        public bool PortfolioExist(EntityUser user) {
            return GetPortfolio(user) != null;
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public bool StockExist(string symbol) {
            return GetStock(symbol) != null;
        }
    }
}

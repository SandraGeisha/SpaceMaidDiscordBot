using System.Collections.Generic;
using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Exurb1aBot.Model.Exceptions.QuoteExceptions;
using Discord;
using Exurb1aBot.Util.Extensions;

namespace Exurb1aBot.Data.Repository {
    class QouteRepository : IQouteRepository {
        public static Random rand = new Random();
        private DbSet<Quote> Quotes { get; set; }
        private ApplicationDbContext _context;

        public QouteRepository(ApplicationDbContext context) {
            Quotes = context.Quote;
            _context = context;
        }

        public void AddQuote(Quote quote) {
            Quotes.Add(quote);
        }

        public IEnumerable<Quote> GetAllByUser(ulong id) {
            return Quotes.Where(q => q.Qoutee.Id == id).OrderBy(q => q.Time).Reverse();
        }

        private int GetCount() {
            return Quotes.Count();
        }

        private int GetCountUser(ulong id) {
            return Quotes.Where(q => q.Qoutee.Id == id).Count();
        }

        public Quote GetQuoteById(int id) {
            Quote q = Quotes.FirstOrDefault(qu => qu.Id == id);

            if (q == null)
                throw new QouteNotFound();

            return q;
        }

        public Quote GetRandom() {

            if (GetCount() == 0)
                throw new NoQuotesException();

            int r = rand.Next(GetCount());
            return Quotes.ToList()[r];
        }
        public Quote GetRandomByUser(ulong id) {
            if (GetCountUser(id) == 0)
                throw new NoQuoteFoundException();

            int r = rand.Next(GetCountUser(id));
            return Quotes.Where(q => q.Qoutee.Id == id).ToList()[r];
        }


        public void RemoveQuote(int id) {
            Quotes.Remove(GetQuoteById(id));
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public int GetId(Quote q) {
            Quote quote = Quotes.AsNoTracking().FirstOrDefault(qu => qu.Equals(q));
            int id = quote.Id;
            return id;
        }

        public bool MessageExists(string text, IGuildUser user, DateTime time) {

            return Quotes.Where(q =>
                     q.QuoteText == text && q.Qoutee.Id == user.Id
                     && q.Time == time
                    ).Count() >= 1;

        }
    }
}

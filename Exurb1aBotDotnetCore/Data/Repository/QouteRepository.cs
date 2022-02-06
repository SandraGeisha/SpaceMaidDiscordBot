using System.Collections.Generic;
using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Exurb1aBot.Model.Exceptions.QuoteExceptions;
using Discord;
using Exurb1aBot.Util.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

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

        private int GetCountUser(ulong id, ulong serverId) {
            return Quotes.Where(q => 
              q.Qoutee.Id == id && q.Guild.ID == serverId
            ).Count();
        }

        public Quote GetQuoteById(int id, ulong serverID) {
            Quote q = Quotes.FirstOrDefault(qu => qu.GuildQuoteID == id && qu.Guild.ID == serverID);

            if (q == null)
                throw new QouteNotFound();

            return q;
        }

        public Quote GetRandom(ulong ServerID) {
            var quotes = GetAllByServer(ServerID);
            if (!quotes.Any())
                throw new NoQuotesException();

            int r = rand.Next(quotes.Count());
            return quotes.ToList()[r];
        }
        public Quote GetRandomByUser(ulong id, ulong serverID) {
          int count = GetCountUser(id, serverID);

          if (count > 0) {
            int r = rand.Next(GetCountUser(id, serverID));
            return Quotes.Where(q => q.Qoutee.Id == id && q.Guild.ID == serverID).ToList()[r];
          }

          throw new NoQuoteFoundException();

        }


        public void RemoveQuote(int id, ulong serverId) {
            Quotes.Remove(GetQuoteById(id, serverId));
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public long GetId(Quote q) {
            Quote quote = Quotes.AsNoTracking().FirstOrDefault(qu => qu.Equals(q));
            long id = quote.GuildQuoteID;
            return id;
        }

        public bool MessageExists(string text, IGuildUser user, DateTime time, ulong serverId) {

            return Quotes.Where(q =>
                     q.QuoteText == text && q.Qoutee.Id == user.Id
                     && q.Time == time && q.Guild.ID == serverId
                    ).Count() >= 1;

        }


    public IEnumerable<Quote> GetAllByServer(ulong serverId) {
      return Quotes.ToList().Where(q => q.Guild != null && q.Guild.ID == serverId);
    }

    public Int64 GetMissingQuoteID(ulong serverId) {
      var output = new SqlParameter
      {
        ParameterName = "outputBit",
        SqlDbType = SqlDbType.BigInt,
        Direction = ParameterDirection.Output
      };

      SqlParameter[] parameters ={
        new SqlParameter{
          ParameterName = "serverId", 
          Value = serverId ,
          SqlDbType = SqlDbType.Decimal
        },
        output
      };

      _context.Database.ExecuteSqlRaw("Exec GetMissingID @serverId, @outputBit OUTPUT", parameters);
      _context.Database.ExecuteSqlRaw("EXEC sp_recompile N'GetMissingID';");
      return (Int64)output.Value;
    }
  }
}

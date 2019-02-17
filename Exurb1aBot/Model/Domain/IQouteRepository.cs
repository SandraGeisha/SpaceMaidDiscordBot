using Discord;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IQouteRepository {
        #region Methods
            Quote GetQuoteById(int id);
            IEnumerable<Quote> GetAllByUser(ulong id);
            int GetId(Quote q);
            Quote GetRandom();
            Quote GetRandomByUser(ulong id);
            void AddQuote(Quote quote);
            void RemoveQuote(int id);
            void SaveChanges();
            bool MessageExists(ulong id);
        #endregion
    }
}

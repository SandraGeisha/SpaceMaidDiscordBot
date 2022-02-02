using Discord;
using System;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IQouteRepository {
        #region Methods
    IEnumerable<Quote> GetAllByServer(ulong serverId);
            Quote GetQuoteById(int id, ulong serverID);
            long GetId(Quote q);
            Quote GetRandom(ulong serverID);
            Quote GetRandomByUser(ulong id, ulong serverID);
            void AddQuote(Quote quote);
            void RemoveQuote(int id, ulong serverID);
            Int64 GetMissingQuoteID(ulong serverId);
            void SaveChanges();
            bool MessageExists(string text, IGuildUser user, DateTime time, ulong serverID);
        #endregion
    }
}

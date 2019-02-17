using System;

namespace Exurb1aBot.Model.Exceptions.QuoteExceptions {
    class NoQuotesException:Exception{
        public NoQuotesException():base("There are no quotes in the server, Wow quote more losers") {}
    }
}

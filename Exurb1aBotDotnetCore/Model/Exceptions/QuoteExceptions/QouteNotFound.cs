using System;

namespace Exurb1aBot.Model.Exceptions.QuoteExceptions {
    class QouteNotFound:Exception {
        public QouteNotFound():base("we couldn't find the quote"){

        }
    }
}

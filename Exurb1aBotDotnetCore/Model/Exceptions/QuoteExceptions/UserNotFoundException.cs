using System;

namespace Exurb1aBot.Model.Exceptions.QuoteExceptions {
    class UserNotFoundException:Exception {
        public UserNotFoundException():base("We couldn't find the user") {}
    }
}

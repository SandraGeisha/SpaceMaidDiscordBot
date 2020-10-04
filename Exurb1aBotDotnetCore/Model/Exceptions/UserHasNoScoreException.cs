using System;

namespace Exurb1aBotDotnetCore.Model.Exceptions {
    class UserHasNoScoreException : Exception {
        public UserHasNoScoreException() : base("Wow you've done nothing to place you on the scores, be significant.") { }
    }
}

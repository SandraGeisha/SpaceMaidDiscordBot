using System;

namespace Exurb1aBot.Model.Exceptions.BannedWordExceptions {
    class WordAlreadyBannedException : Exception {
        public WordAlreadyBannedException() :base("word already banned"){}
    }
}

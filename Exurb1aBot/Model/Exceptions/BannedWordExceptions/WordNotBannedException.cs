using System;

namespace Exurb1aBot.Model.Exceptions.BannedWordExceptions {
    class WordNotBannedException:Exception {
        public WordNotBannedException() :base("word not banned"){}
    }
}

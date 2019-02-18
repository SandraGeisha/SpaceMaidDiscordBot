using System;

namespace Exurb1aBot.Model.Exceptions.LocationExceptions {
    class NoLocationFoundException:Exception {
        public NoLocationFoundException():base("We couldn't find a location") {}
    }
}

using System;

namespace Exurb1aBot.Model.Exceptions.LocationExceptions {
    class NoLocationAssociatedException:Exception{
        public NoLocationAssociatedException():base("There is no location set"){}
    }
}

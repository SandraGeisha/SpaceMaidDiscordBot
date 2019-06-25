using System;

namespace Exurb1aBot.Model.Exceptions {
     class WouldMakeCurrencyNegative :Exception{
        public WouldMakeCurrencyNegative():base("Retracting this amount would make your balance negative") {}
    }
}

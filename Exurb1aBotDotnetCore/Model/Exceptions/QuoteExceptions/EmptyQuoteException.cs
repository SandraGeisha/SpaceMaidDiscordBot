using System;

namespace Exurb1aBot.Model.Exceptions.QuoteExceptions {
    class EmptyQuoteException:Exception{
        #region Constructors
        public EmptyQuoteException() : base("Empty Quote") { }

        public EmptyQuoteException(string msg) : base(msg) { }

        public EmptyQuoteException(string msg, Exception ex) : base(msg, ex) { } 
        #endregion
    }
}

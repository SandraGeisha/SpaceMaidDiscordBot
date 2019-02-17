using System;
namespace Exurb1aBot.Model.Exceptions.QuoteExceptions {
    class NoQuoteFoundException:Exception{
        public NoQuoteFoundException() :base("No quotes found"){}
        public NoQuoteFoundException(string msg):base(msg) {}
        public NoQuoteFoundException(string msg,Exception ex) :base(msg,ex){}
    }
}

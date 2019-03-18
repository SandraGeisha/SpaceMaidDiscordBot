using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Model.Exceptions.QuoteExceptions {
    class QuotingYourselfException:Exception {
        public QuotingYourselfException():base("You're trying to quote yourself"){}
        public QuotingYourselfException(string msg):base(msg) {}
        public QuotingYourselfException(string msg, Exception e):base(msg,e) {}
    }
}

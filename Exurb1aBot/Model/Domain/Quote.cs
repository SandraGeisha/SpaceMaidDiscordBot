using Exurb1aBot.Model.Exceptions.QuoteExceptions;
using Exurb1aBot.Model.ViewModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exurb1aBot.Model.Domain {
    public class Quote {
        public string QuoteText { get; protected set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public ulong msgId { get; set; }
        public EntityUser Creator { get; private set; }
        public EntityUser Qoutee { get; private set; }
        public DateTime Time { get; private set; }

        //used by entity framework
        protected Quote() {}

        public Quote(string text,EntityUser Creator,EntityUser Quotee,DateTime dateTime) {

            if (text == null || text.Trim().Length == 0)
                throw new EmptyQuoteException();

            QuoteText = text;
            this.Qoutee = Quotee;
            this.Creator = Creator;
            this.Time = dateTime;
        }
    }
}

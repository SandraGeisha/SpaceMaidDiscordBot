using Exurb1aBot.Model.Exceptions.QuoteExceptions;
using Exurb1aBot.Model.ViewModel;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public class Quote {
        public string QuoteText { get; protected set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public ulong msgId { get; set; }
        public EntityUser Creator { get; private set; }
        public EntityUser Qoutee { get; private set; }
        public DateTime Time { get; private set; }

        public IGuild _context { get; private set; }

        //used by entity framework
        protected Quote() {}

        public Quote(string text,EntityUser Creator,EntityUser Quotee,DateTime dateTime, IGuild context) {
            _context = context;

            if (text == null || text.Trim().Length == 0)
                throw new EmptyQuoteException();

            if (Creator.Id == Quotee.Id)
                throw new QuotingYourselfException();

            QuoteText = text;

            if (QuoteText.Contains("@")) {
                string[] s = QuoteText.Split(' ').Where(x => {
                    return x.Contains("@");
                }).ToArray();

                string[] replaceText = getTextRoleOrUsername(s).Result;

                for(int i=0;i<s.Count();i++) {
                    QuoteText = QuoteText.Replace(s[i], '@'+replaceText[i].Replace(" ","-"));
                }
            }

            this.Qoutee = Quotee;
            this.Creator = Creator;
            this.Time = dateTime;
        }

        private async Task<string[]> getTextRoleOrUsername(string[] mentions) {

            List<string> list = new List<string>();

            foreach (string ment in mentions) {
                // ! for user with nickname , & for role
                string type = ment.Substring(2, 1);

                var x=ment;
                //user doesn't have a nickname (discord api is retarded)
                if (ment.StartsWith("<@") && type != "!" && type != "&") {
                    x = ment.Remove(0, 2);
                    type = "!";
                }
                else
                    x = ment.Remove(0, 3);

                x = x.Remove(x.Length - 1, 1);



                switch (type) {
                    case "!":
                        var user = await _context.GetUserAsync(ulong.Parse(x));
                        list.Add(user==null? ment:(user.Nickname ?? user.Username));
                        break;
                    case "&":
                        var group = _context.GetRole(ulong.Parse(x));
                        list.Add(group == null ? ment : group.Name);
                        break;
                    default:
                        list.Add(ment.Replace('@',' '));
                        break;
                }

            }

            return list.ToArray();
        }

    }
}

using Discord;
using Exurb1aBot.Model.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Exurb1aBot.Model.Exceptions;

namespace Exurb1aBot.Model.ViewModel {
    public class EntityUser {
        public string Username { get; set; }
        public ulong Id { get; set; }
        public ICollection<CurrencyUser> Currencies { get; set; }     
        public DateTime RedeemedTime { get; set; }

        [NotMapped]
        public double ValueCurrencies {
            get {
                return GetCurrencyAmount(CurrencyEnum.Entropy) +
                    ( GetCurrencyAmount(CurrencyEnum.Complexity) * 1000);
            }
        }

        //Used by entity framework
        protected EntityUser() {}

        public EntityUser(IGuildUser user) {
            Username = user.Username;
            Id = user.Id;

            if (RedeemedTime == DateTime.MinValue)
                RedeemedTime = DateTime.Now.AddDays(-1).AddHours(-1);
        }

        public void AddAmount(CurrencyEnum cur, double amount) {
            var curr = Currencies?.FirstOrDefault(c => c.Currency == cur);

            if (curr == null) {
                if (Currencies == null)
                    Currencies = new List<CurrencyUser>();

                curr = new CurrencyUser(this, amount, cur);
                Currencies.Add(curr);
            } else
                curr.Amount = curr.Amount += amount;
        }

        public void RemoveAmount(CurrencyEnum cur, double amount) {
            var curr = Currencies?.FirstOrDefault(c => c.Currency == cur);

            if (curr == null) {
                if (Currencies == null)
                    Currencies = new List<CurrencyUser>();

                curr = new CurrencyUser(this, 0, cur);
                RemoveAmount(cur, amount);
            }

            if (curr.Amount < amount)
                throw new WouldMakeCurrencyNegative();
            
            curr.Amount = curr.Amount -= amount;
        }

        public double GetCurrencyAmount(CurrencyEnum cur){
            if (Currencies == null || Currencies.Where(c => c.Currency == cur).Count() == 0)
                return 0;

            return Currencies.Where(c => c.Currency == cur).Sum(c => c.Amount);            
        }
    }
}

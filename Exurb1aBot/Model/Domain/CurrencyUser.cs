using Exurb1aBot.Model.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exurb1aBot.Model.Domain {
    public class CurrencyUser {
        public EntityUser Owner { get; set; }

        public ulong OwnerID { get; set; }
        public double Amount { get;  set; }
        public CurrencyEnum Currency { get; set;  }
        //Fucking entityFramework bullshit
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CUID { get; set; }

        protected CurrencyUser() {}

        public CurrencyUser(EntityUser owner, double amount, CurrencyEnum cur) {
            Currency = cur;
            Amount = amount;
            OwnerID = owner.Id;
            Owner = owner;
        }
    }
}

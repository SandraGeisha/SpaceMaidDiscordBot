using Exurb1aBot.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Model.Domain {
    public class Location {
        public EntityUser User { get; set; }
        public string LocationName { get; set; }
        public ulong Id { get; set; }

        //For entity framework
        protected Location() { }

        public Location(EntityUser user, string location) {
            User = user;
            LocationName = location;
        }
    }
}

using Exurb1aBot.Model.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exurb1aBot.Model.Domain {
    public class Scores {
        public ulong Id { get; set; }
        public Server Server { get; set; }
        public ulong ServerID { get; set; }
        public int Quoted { get; set; }
        public int Quotes_Created { get; set; }
    }
}

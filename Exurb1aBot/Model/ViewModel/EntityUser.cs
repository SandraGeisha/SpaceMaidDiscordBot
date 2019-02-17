using Discord;

namespace Exurb1aBot.Model.ViewModel {
    public class EntityUser {
        public string Username { get; set; }
        public ulong Id { get; set; }
        //Used by entity framework
        protected EntityUser() {}

        public EntityUser(IGuildUser user) {
            Username = user.Username;
            Id = user.Id;
        }
    }
}

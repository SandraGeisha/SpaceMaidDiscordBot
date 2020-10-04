namespace Exurb1aBot.Model.Domain {
    public class Role {
        public long ID { get; set; }
        public string Name { get; set; }
        public Enums.RoleType RoleType { get; set; }
        public string ReactionEmote { get; set; }
    }
}

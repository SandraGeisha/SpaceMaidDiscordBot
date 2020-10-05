namespace Exurb1aBot.Model.Domain {
    public class Role {
        public ulong ID { get; set; }
        public string Name { get; set; }
        public Enums.RoleType RoleType { get; set; }
        public string ReactionEmote { get; set; }
        public Enums.EmoteType? EmoteType { get; set; }
    }
}

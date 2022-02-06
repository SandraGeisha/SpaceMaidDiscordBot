namespace Exurb1aBot.Model.Domain {
    public static class Enums {
        public static readonly ulong SandraID = 401452008957280257;
        public static readonly ulong MudaID = 373513157227839499;

        public static readonly string EmojiRegex = "^(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])$";
        public static readonly string DataFileName = "DataFile.json";

        public enum ScoreType {
            Qouted,
            Qouter
        }
    }
}

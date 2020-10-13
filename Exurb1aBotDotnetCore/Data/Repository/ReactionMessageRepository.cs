using Exurb1aBot.Model.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Exurb1aBot.Data.Repository {
    public class ReactionMessageRepository : IReactionMessageRepository {
        private List<ulong> _messages;
        private readonly string _path = $"{Directory.GetCurrentDirectory()}\\{Enums.DataFileName}";

        public ReactionMessageRepository() {
            _messages = new List<ulong>();
            Initialize();
        }

        public void AddReactionMessage(ulong messageId) {
            _messages.Add(messageId);
            WriteToFile();
        }

        public IEnumerable<ulong> GetReactionMessages() {
            return _messages;
        }

        public void Initialize() {
            if (!File.Exists(_path)) {
                File.Create(_path);
            } else {
                string s = File.ReadAllText(_path);
                var result = JsonConvert.DeserializeObject<List<ulong>>(s);

                if (result != null && result.Count > 0) {
                    _messages = result;
                }
            }
        }

        public bool IsReactionMessage(ulong messageId) {
            return _messages.Contains(messageId);
        }

        public void RemoveReactionMessage(ulong messageId) {
            _messages.Remove(messageId);
            WriteToFile();
        }

        private void WriteToFile() {
            string s = JsonConvert.SerializeObject(_messages);
            File.WriteAllText(_path, s);
        }
    }
}

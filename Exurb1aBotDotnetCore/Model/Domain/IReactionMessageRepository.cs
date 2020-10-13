using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IReactionMessageRepository {
        IEnumerable<ulong> GetReactionMessages();
        void Initialize();
        void AddReactionMessage(ulong messageId);
        void RemoveReactionMessage(ulong messageId);
        bool IsReactionMessage(ulong messageId);
    }
}

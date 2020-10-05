using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IRoleRepository {
        bool Exist(string name);
        bool Exist(ulong id);
        bool EmojiExist(string emoji);
        void Add(Role role);
        void Remove(Role role);
        IEnumerable<Role> GetAll();
        Role GetByName(string name);
    }
}

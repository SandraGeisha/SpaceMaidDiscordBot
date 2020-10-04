using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IRoleRepository {
        bool Exist(string name);
        bool Exist(long id);
        void Add(Role role);
        void Remove(Role role);
        IEnumerable<Role> GetAll();
        Role GetByName(string name);
    }
}

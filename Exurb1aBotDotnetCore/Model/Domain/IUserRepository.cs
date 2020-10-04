using Exurb1aBot.Model.ViewModel;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IUserRepository {
        #region Methods
        IEnumerable<EntityUser> GetAllUsers();
        void SaveChanges();
        EntityUser GetUserById(ulong id);
        void AddUser(EntityUser user);
        #endregion
    }
}

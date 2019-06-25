using Exurb1aBot.Model.ViewModel;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IUserRepository {
        #region Methods
        IEnumerable<EntityUser> GetAllUsers();
        IEnumerable<EntityUser> GetRanking(int page);
        int GetCountUsers();
        EntityUser GetByID(ulong id);
        void AddUser(EntityUser eu);
        void SaveChanges();
        #endregion
    }
}

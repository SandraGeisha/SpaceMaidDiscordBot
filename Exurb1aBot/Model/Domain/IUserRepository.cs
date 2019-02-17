using Exurb1aBot.Model.ViewModel;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IUserRepository {
        #region Methods
        IEnumerable<EntityUser> GetAllUsers();
        void SaveChanges();
        #endregion
    }
}

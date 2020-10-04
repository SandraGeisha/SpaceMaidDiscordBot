using Exurb1aBot.Model.ViewModel;

namespace Exurb1aBot.Model.Domain {
    public interface ILocationRepository {
        bool UserHasLocation(EntityUser user);
        Location GetLocation(EntityUser user);
        void ChangeLocation(EntityUser user,string location);
        void AddLocation(EntityUser user, string location);
        void SaveChanges();
    }
}

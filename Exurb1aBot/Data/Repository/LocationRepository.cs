using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Exurb1aBot.Data.Repository {
    public class LocationRepository : ILocationRepository {
        private  DbSet<Location> _locations;
        private ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context) {
            _context = context;
            _locations = context.Location;
        }

        public void AddLocation(EntityUser user, string location) {
            Location l = new Location(user,location);
            _locations.Add(l);
        }

        public void ChangeLocation(EntityUser user, string location) {
            Location l = GetLocation(user);
            if(l!=null)
                l.LocationName = location;
        }

        public Location GetLocation(EntityUser user) {
            return _locations.FirstOrDefault(l => l.Id == user.Id);
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public bool UserHasLocation(EntityUser user) {
            return GetLocation(user) != null;
        }
    }
}

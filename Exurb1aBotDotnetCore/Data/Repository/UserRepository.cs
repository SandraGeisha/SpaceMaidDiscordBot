using System.Collections.Generic;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Exurb1aBot.Model.Exceptions.QuoteExceptions;

namespace Exurb1aBot.Data.Repository {
    class UserRepository : IUserRepository {
        private readonly ApplicationDbContext _context;
        private DbSet<EntityUser> _users;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
            _users = context.Users;
        }

        public void AddUser(EntityUser user) {
            _users.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<EntityUser> GetAllUsers() {
            return _users.ToList();
        }

        public EntityUser GetUserById(ulong id) {
            return _users.ToList().FirstOrDefault(u => u.Id == id);
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

    }
}

using System.Collections.Generic;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Exurb1aBot.Model.Exceptions.QuoteExceptions;
using System;

namespace Exurb1aBot.Data.Repository {
    class UserRepository : IUserRepository {
        private readonly ApplicationDbContext _context;
        private DbSet<EntityUser> _users;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
            _users = context.Users;
        }

        public IEnumerable<EntityUser> GetAllUsers() {
            return _users.ToList();
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public int GetCountUsers() {
            return _users.Count();
        }

        public void AddUser(EntityUser eu) {
            _users.Add(eu);
            SaveChanges();
        }

        public EntityUser GetByID(ulong id) {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<EntityUser> GetRanking(int page) {
            return _users.OrderByDescending(u=>u.ValueCurrencies).Skip((page - 1 )* 10).Take(10);
        }
    }
}

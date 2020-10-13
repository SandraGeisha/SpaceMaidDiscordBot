using Discord;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Exurb1aBot.Data.Repository {
    public class RoleRepository : IRoleRepository {
        private ApplicationDbContext _context;
        private DbSet<Role> _roles;

        public RoleRepository(ApplicationDbContext context) {
            _context = context;
            _roles = context.Roles;
        }

        public void Add(Role role) {
            _roles.Add(role);
            _context.SaveChanges();
        }

        public bool EmojiExist(string emoji) {
            return _roles.ToList().FirstOrDefault(
                 r => r.RoleType == Enums.RoleType.Reaction && r.ReactionEmote.Equals(emoji)
            ) != null;
        }

        public bool Exist(string name) {
            return _roles.ToList().FirstOrDefault(
                r => r.Name.ToLower().Trim().Equals(name.ToLower().Trim())
            ) != null;
        }

        public bool Exist(ulong id) {
            return _roles.ToList().FirstOrDefault(r => r.ID == id) != null;
        }

        public IEnumerable<Role> GetAll() {
            return _roles.ToList();
        }

        public IEnumerable<Role> GetAllByType(Enums.RoleType type) {
            return GetAll().Where(r => r.RoleType.Equals(type));
        }

        public Role GetByName(string name) {
            if (!Exist(name))
                throw new RoleNotFoundException(string.Format("The specified role with name {0} doesn't exist.",name));

            return GetAll().First(r => r.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
        }

        public Role GetRoleByEmoji(string emoji) {
            return _roles.FirstOrDefault(r => r.ReactionEmote == emoji);
        }

        public void Remove(Role role) {
            if (!Exist(role.Name))
                throw new RoleNotFoundException(string.Format("The specified role with name {0} doesn't exist.", role.Name));

            _roles.Remove(role);
            _context.SaveChanges();
        }
    }
}

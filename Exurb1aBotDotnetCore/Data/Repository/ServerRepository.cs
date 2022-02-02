using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exurb1aBot.Data.Repository {
  public class ServerRepository : IServerRepository {
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Server> _servers;

    public ServerRepository(ApplicationDbContext context) {
      _context = context;
      _servers = context.Servers;
    }


    public void Add(Server guild) {
      if (Exist(guild.ID))
        throw new Exception("This server already exists");

      _servers.Add(guild);
      _context.SaveChanges();
    }

    public bool Exist(ulong id) {
      return GetById(id) != null;
    }

    public IEnumerable<Server> GetAll() {
      return _servers.ToList();
    }

    public Server GetById(ulong id) {
      return _servers.FirstOrDefault(s => s.ID.Equals(id));
    }
  }
}

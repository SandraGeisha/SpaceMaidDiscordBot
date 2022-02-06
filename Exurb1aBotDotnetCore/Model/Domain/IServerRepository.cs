using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Model.Domain {
  public interface IServerRepository {
    IEnumerable<Server> GetAll();
    void Add(Server guild);
    bool Exist(ulong id);
    Server GetById(ulong id);
  }
}

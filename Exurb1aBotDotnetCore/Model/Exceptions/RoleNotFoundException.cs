using System;

namespace Exurb1aBot.Model.Exceptions {
    public class RoleNotFoundException : Exception {
        public RoleNotFoundException():base() {}
        public RoleNotFoundException(string msg): base(msg) {}
    }
}

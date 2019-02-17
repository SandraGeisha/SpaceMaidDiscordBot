using System;
using System.Collections.Generic;
using System.Text;

namespace Exurb1aBot.Model.Exceptions {
    class NoVideoFoundException:Exception {
        public NoVideoFoundException():base("We couldn't find the video you're looking for") {}
    }
}

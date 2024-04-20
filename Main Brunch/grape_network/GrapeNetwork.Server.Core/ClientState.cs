using GrapeNetwork.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core
{
    public class ClientState
    {
        public Connection connection { get; }

        public ClientState(Connection connection)
        {
            this.connection = connection;
        }
    }
}

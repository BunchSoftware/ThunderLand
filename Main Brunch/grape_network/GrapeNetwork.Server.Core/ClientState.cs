using GrapeNetwork.Core.Server;

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

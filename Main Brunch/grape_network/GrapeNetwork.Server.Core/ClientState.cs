using GrapeNetwork.Core.Server;

namespace GrapeNetwork.Server.Core
{
    public class ClientState
    {
        public Connection connection { get; }

        public bool isAuth;
        public bool isLobby = false;
        public bool isGame = false;

        public ClientState(Connection connection)
        {
            this.connection = connection;
        }
    }
}

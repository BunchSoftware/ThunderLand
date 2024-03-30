using System.Net;

namespace GrapeNetwork.Client
{
    public interface ICommandClient
    {
        public void ConnectToServer(int portServer, IPAddress iPAddressServer);
        public void DisconnectFromServer();
    }
}

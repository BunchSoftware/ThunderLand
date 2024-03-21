using System.Net;

namespace GrapNetwork.Client
{
    public interface ICommandClient
    {
        public void ConnectToServer(int portServer, IPAddress iPAddressServer);
        public void DisconnectFromServer();
    }
}

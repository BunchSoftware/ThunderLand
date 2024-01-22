using System.Net;

namespace GrapNetwork.Client
{
    public interface ICommandClient
    {
        public void ConnectToServer(IPAddress iPAddressServer, int portServer, string passwordServer);
        public void DisconnectFromServer();
    }
}

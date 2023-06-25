using System.Net;

namespace ClientNet
{
    public interface ICommandClient
    {
        public void ConnectToServer(IPAddress iPAddressServer, int portServer, string passwordServer);
        public void DisconnectFromServer();
    }
}

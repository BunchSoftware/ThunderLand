using UnityEngine;
using System.Net;
using GrapNetwork;
using GrapNetwork.Client;

public class ClientHandler : MonoBehaviour
{
    public Package package;
    NetworkClient client;

    public void Start()
    {
        client = new NetworkClient();
        package = new Package();

        client.OnConnectEvent += () =>
        {
            print(client.ConnectionState);
        };
        client.OnDisconnectEvent += () =>
        {
            print(client.ConnectionState);
        };
        client.OnRecieveEvent += (package) =>
        {
            
        };
        client.OnSendEvent += () =>
        {
            print(1);
        };

        OnConnectToServer();
    }

    public void OnConnectToServer()
    {
        client.ConnectToServer(IPAddress.Parse("192.168.1.100"), 7777, "125");       
    }
    public void OnDisconnectFromServer()
    {
        client.DisconnectFromServer();
    }
    public void SendPackage()
    {
        
    }
}

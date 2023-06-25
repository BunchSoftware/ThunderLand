using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using ClientNet;
using Newtonsoft.Json;
using System.Net.Http;

public class ClientHandler : MonoBehaviour
{
    public Package package;
    Client client;

    public void Start()
    {
        client = new Client();
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

        

        print(JsonConvert.SerializeObject(package));
    }

    public void OnConnectToServer()
    {
        client.ConnectToServer(IPAddress.Parse("192.168.1.100"), 7777, "125");       
    }
    public void OnDisconnectFromServer()
    {
        client.DisconnectFromServer();
    }
    private void SendPackage(Package package)
    {
        
    }

    private IEnumerator SendPackageWhile(Package package, float timeOut)
    {
        while (true)
        {
            if (client.ConnectionState == ConnectionState.Connected)
            {
                yield return new WaitForSeconds(timeOut);
                SendPackage(package);
            }
        }
    }
}

using GrapeNetwork.Packages;
using GrapNetwork.Client;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class ClientHandler : MonoBehaviour
{
    NetworkClient networkClient;

    [SerializeField] private Player player;

    private void Start()
    {
        networkClient = new NetworkClient();
        networkClient.ConnectToServer(2200, IPAddress.Parse("192.168.1.100"));

        player.OnChangePositionPlayer += (position) =>
        {
            Vector3Package vector3Package = new Vector3Package(MathF.Round(position.x, 2), MathF.Round(position.y, 2), MathF.Round(position.z, 2));

            Package package = new Package();
            package.Content = JsonConvert.SerializeObject(vector3Package);
            PackageHeaders packageHeaders = new PackageHeaders();
            packageHeaders.Server = "GameServer";
            package.Headers = packageHeaders;
            package.ContentType = "gameInfo/text";
            package.StatusCode = StatusCode.OK;


            networkClient.SendPackage(package, true);
        };

        networkClient.OnRecieveDataEvent += NetworkClient_OnRecieveDataEvent;
    }

    private void NetworkClient_OnRecieveDataEvent(Package package)
    {
        print(package.Content);
    }

    private void OnApplicationQuit()
    {
        networkClient.DisconnectFromServer();
    }
}

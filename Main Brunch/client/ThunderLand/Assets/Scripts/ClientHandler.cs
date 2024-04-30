using GrapeNetwork.Client.Core;
using GrapeNetwork.Packages;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class ClientHandler : MonoBehaviour
{
    GameClient gameClient = new GameClient();

    [SerializeField] private Player player;

    private void Start()
    {
        gameClient.ReadConfig();
        gameClient.ConnectToServer();

        player.OnChangePositionPlayer += (position) =>
        {
            
        };

        gameClient.OnDebugInfo += (message) =>
        {
            Debug.Log(message);
        };
        gameClient.OnExceptionInfo += (exception) =>
        {
            Debug.LogError(exception);
        };
        gameClient.OnErrorInfo += (message) =>
        {
            Debug.LogError(message);
        };
    }

    private void OnApplicationQuit()
    {
        gameClient.DisconnectFromServer();
    }
}


using GrapeNetwork.Client.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandler : MonoBehaviour
{

    private void Start()
    {
        GameClient gameClient = new GameClient(2200, "192.168.56.1");

        gameClient.ReadConfig();

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

        gameClient.ConnectToServer();
        //player.OnChangePositionPlayer += (position) =>
        //{

        //}
    }
}

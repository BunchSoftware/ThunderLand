using GrapeNetwork.Client.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABC
{
    private GameClient gameClient = new GameClient();

    public void Awake()
    {
        gameClient.ReadConfig();
        gameClient.ConnectToServer();

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

        gameClient.Authetication("Den4o", "win");
    }

    private void OnApplicationQuit()
    {
        gameClient.DisconnectFromServer();
    }
}

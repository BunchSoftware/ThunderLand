
using GrapeNetwork.Client.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientHandler : MonoBehaviour
{
    private GameClient gameClient = new GameClient();

    private void Start()
    {
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
        gameClient.OnConnectEvent += () =>
        {
            SceneManager.LoadScene(1);
        };

        gameClient.ConnectToLoginServer();
        //player.OnChangePositionPlayer += (position) =>
        //{

        //}
    }

    public void Registration(string login, string password)
    {
        gameClient.Registration(login, password);
    }
    public void Authetication(string login, string password)
    {
        gameClient.Authetication(login, password);
    }
    public void ConnectToGameServer()
    {
        gameClient.ConnectToGameServer();
    }
}

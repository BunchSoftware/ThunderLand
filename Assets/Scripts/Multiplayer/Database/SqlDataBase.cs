using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SqlDataBase : MonoBehaviour
{
    private SqlConnection sqlConnection;
    private TcpClient clientSocket;
    private NetworkStream networkStream;
    [SerializeField] private Text text;
    int ConnectionTimeOut = 2;

    private void Start()
    {
        //sqlConnection = new SqlConnection("Data Source=DESKTOP-GAIGRAL\\THUNDERSERVER;Initial Catalog=InfoPlayer;Integrated Security=True;" + $"Connection Timeout={ConnectionTimeOut}");
        //sqlConnection.Open();

        //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        //SqlCommand sqlCommand = new SqlCommand("SELECT Name FROM LoginPlayerInfo ", sqlConnection);
        //sqlDataAdapter.SelectCommand = sqlCommand;
        //print(sqlConnection.State);

        clientSocket = new TcpClient("192.168.1.104", 7000);
        networkStream = clientSocket.GetStream();

        byte[] bytes = new byte[256];
        networkStream.Read(bytes, 0, bytes.Length);
        string request = Encoding.ASCII.GetString(bytes);
        text.text = request;
        StartCoroutine(WriteServer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print(1);
            StartCoroutine(WriteServer());
        }
    }

    IEnumerator WriteServer()
    {
        string message = transform.position.x.ToString();
        byte[] bytes = new byte[256];
        bytes = Encoding.ASCII.GetBytes(message);
        networkStream.Write(bytes, 0, bytes.Length);
        networkStream.Flush();
        yield return new WaitForSeconds(2);
    }
}

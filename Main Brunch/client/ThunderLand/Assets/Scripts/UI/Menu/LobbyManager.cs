using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button enterToGameWorldButton;
    [Header("Options")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button supportButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button exitButton;
    [Header("Personage")]
    [SerializeField] private Button createPersonageButton;
    [SerializeField] private Button deletePersonageButton;

    private ClientHandler clientHandler;

    private void Start()
    {
        clientHandler = FindObjectOfType<ClientHandler>();

        enterToGameWorldButton?.onClick.AddListener(() => { EnterToGameWorld(); });
        optionsButton?.onClick.AddListener(() => { Options(); });
        supportButton?.onClick.AddListener(() => { Application.OpenURL("https://github.com/BunchSoftware/ThunderLand"); });
        returnButton?.onClick.AddListener(() => { Return(); });
        exitButton?.onClick.AddListener(() => { Exit(); });
        createPersonageButton?.onClick.AddListener(() => { CreatePersonage(); });
        deletePersonageButton?.onClick.AddListener(() => { DeletePersonage(); });
    }
    private void CreatePersonage()
    {

    }
    private void DeletePersonage()
    {

    }
    private void EnterToGameWorld()
    {
        clientHandler.ConnectToGameServer();
        SceneManager.LoadScene(3);
    }
    private void Options()
    {
        optionsPanel?.SetActive(true);
    }
    private void Return()
    {
        SceneManager.LoadScene(1);
    }

    private void Exit()
    {
        Application.Quit(0);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeetingMenuManager : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private Text loginText;
    [SerializeField] private Text passwordText;
    [SerializeField] private Button loginButton;
    [Header("Options")]
    [SerializeField] private Button createAccountButton;
    [SerializeField] private Button myAccountButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button productionCommandButton;
    [SerializeField] private GameObject productionCommandPanel;
    [Header("Error")]
    [SerializeField] private GameObject errorPanel;
    [Header("Site")]
    [SerializeField] private Button supportButton;
    [SerializeField] private Button donationButton;
    [SerializeField] private Button forumButton;
    [SerializeField] private Button siteServerButton;
    [Header("Exit")]
    [SerializeField] private Button exitButton;

    private void Start()
    {
        loginButton?.onClick.AddListener(() => { Login();});
        createAccountButton?.onClick.AddListener(() =>{Application.OpenURL("https://github.com/BunchSoftware/ThunderLand");});
        myAccountButton?.onClick.AddListener(() =>{Application.OpenURL("https://github.com/BunchSoftware/ThunderLand");});
        optionsButton?.onClick.AddListener(() =>{Options();});
        productionCommandButton?.onClick.AddListener(() =>{ProductionCommand();});
        supportButton?.onClick.AddListener(() => { Application.OpenURL("https://github.com/BunchSoftware/ThunderLand"); });
        donationButton?.onClick.AddListener(() => { Application.OpenURL("https://github.com/BunchSoftware/ThunderLand"); });
        forumButton?.onClick.AddListener(() => { Application.OpenURL("https://github.com/BunchSoftware/ThunderLand"); });
        siteServerButton?.onClick.AddListener(() => { Application.OpenURL("https://github.com/BunchSoftware/ThunderLand"); });
        exitButton?.onClick.AddListener(() =>{Exit();});
    }

    private void Login()
    {
        if (loginText.text == "Den4o" && passwordText.text == "win")
            SceneManager.LoadScene(1);
        else
            errorPanel?.SetActive(true);
    }
    private void Options()
    {
        optionsPanel.SetActive(true);
    }
    private void ProductionCommand()
    {
        productionCommandPanel.SetActive(true);
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

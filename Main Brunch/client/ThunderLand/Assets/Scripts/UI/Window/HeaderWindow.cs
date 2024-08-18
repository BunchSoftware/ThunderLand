using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeaderWindow : MonoBehaviour
{
    private Button closeButton;
    private Button minimizeButton;
    private Button maximizeButton;
    private Text headerText;
    private Window window;

    public bool ActiveButtonMinimize = true;
    public bool ActiveButtonClosed = true;
    public bool ActiveButtonMaximized = true;
    public int WidthHeader = 10;

    private RectTransform rectTransformWindow;
    private RectTransform rectHeader;
    private WindowManager windowManager;
    private string titleWindow;

    public void Visible(bool visible)
    {

    }

    public void Initialize(Window window, WindowManager windowManager)
    {
        this.window = window;
        titleWindow = window.TitleWindow;
        this.windowManager = windowManager;
        rectTransformWindow = window.GetComponent<RectTransform>();
        rectHeader = GetComponent<RectTransform>();
        rectHeader.sizeDelta = new Vector2(rectHeader.sizeDelta.x, WidthHeader);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Buttons")
            {
                Transform buttons = transform.GetChild(i);
                for (int j = 0; j < buttons.childCount; j++)
                {
                    if (buttons.GetChild(j).name == "CloseButton")
                    {
                        closeButton = buttons.GetChild(j).GetComponent<Button>();
                        closeButton.gameObject.SetActive(ActiveButtonClosed);
                        closeButton.onClick.AddListener(Close);
                    }
                    if (buttons.GetChild(j).name == "MinimizeButton")
                    {
                        minimizeButton = buttons.GetChild(j).GetComponent<Button>();
                        minimizeButton.gameObject.SetActive(ActiveButtonMinimize);
                        minimizeButton.onClick.AddListener(Minimize);
                    }
                    if (buttons.GetChild(j).name == "MaximizeButton")
                    {
                        maximizeButton = buttons.GetChild(j).GetComponent<Button>();
                        maximizeButton.gameObject.SetActive(ActiveButtonMaximized);
                        maximizeButton.onClick.AddListener(Maximize);
                    }
                }
            }
            if (transform.GetChild(i).name == "HeaderText")
            {
                headerText = transform.GetChild(i).GetComponent<Text>();
                headerText.text = titleWindow;
            }
        }
    }

    private void Minimize()
    {
        window.Minimize();
    }
    private void Close()
    {
        window.Close();
    }
    private void Maximize()
    {
        window.Maximize();
    }

    public void DragHandler(BaseEventData data)
    {
        if (window.IsMaximize == false)
        {
            rectTransformWindow.position = new Vector3
                (
                  Mathf.Clamp(rectTransformWindow.position.x, 0, windowManager.Width - window.Width),
                  Mathf.Clamp(rectTransformWindow.position.y, window.Height, windowManager.Height),
                  rectTransformWindow.position.z
                );

            PointerEventData pointerData = (PointerEventData)data;
            rectTransformWindow.anchoredPosition += pointerData.delta;
        }
    }
}

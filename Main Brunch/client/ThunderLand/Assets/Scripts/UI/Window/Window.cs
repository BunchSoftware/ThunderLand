using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public bool IsMinimize { get { return isMinimize; } }
    public bool IsMaximize { get { return isMaximize; } }
    private bool isMinimize = false;
    private bool isMaximize = false;
    public bool EnableButtonMinimize = true;
    public bool EnableButtonClosed = true;
    public bool EnableButtonMaximized = true;
    public bool EnableHeader = true;
    public int Width = 400;
    public int Height = 200;
    public string TitleWindow;
    private HeaderWindow headerWindow;
    private WindowManager windowManager;
    private RectTransform rectTransform;
    private int lastMaximizeWidth;
    private int lastMaximizeHeight;
    private Vector2 lastAnchoredPosition;

    private void OnValidate()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Width, Height);
    }

    public void Initialization()
    {
        windowManager = GetComponentInParent<WindowManager>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Width, Height);
        lastAnchoredPosition = rectTransform.anchoredPosition;
        lastMaximizeWidth = Width;
        lastMaximizeHeight = Height;
        if (EnableHeader)
        {
            headerWindow = GetComponentInChildren<HeaderWindow>();
            if (headerWindow != null)
            {
                headerWindow.ActiveButtonClosed = EnableButtonClosed;
                headerWindow.ActiveButtonMinimize = EnableButtonMinimize;
                headerWindow.ActiveButtonMaximized = EnableButtonMaximized;
                headerWindow.Initialize(this, windowManager);
            }
            else
                print("Ошибка: Не найден HeaderWindow");
        }
    }
    public void Minimize()
    {
        if (isMinimize)
        {
            isMinimize = false;
        }
        else
        {
            isMinimize = true;
        }
    }
    public void Maximize()
    {
        if (isMaximize)
        {
            Height = lastMaximizeHeight;
            Width = lastMaximizeWidth;
            rectTransform.anchoredPosition = lastAnchoredPosition;
            rectTransform.sizeDelta = new Vector2(Width, Height);
            isMaximize = false;
        }
        else
        {
            lastMaximizeHeight = Height;
            lastMaximizeWidth = Width;
            lastAnchoredPosition = rectTransform.anchoredPosition;
            Height = windowManager.Height;
            Width = windowManager.Width;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(Width, Height);
            isMaximize = true;
        }
    }

    public void Close()
    {
        windowManager.CloseWindow(this);
        Destroy(gameObject);
    }
}

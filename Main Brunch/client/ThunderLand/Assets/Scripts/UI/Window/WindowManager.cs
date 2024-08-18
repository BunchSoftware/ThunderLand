using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [HideInInspector][SerializeField] private List<Window> windows;
    public int Width = 1920;
    public int Height = 1080;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            windows.Add(transform.GetComponentInChildren<Window>());
            windows[i].Initialization();
        }
    }

    public void CloseWindow(Window window)
    {
        windows.Remove(window);
    }
}

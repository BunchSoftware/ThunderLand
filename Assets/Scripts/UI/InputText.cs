using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputText : MonoBehaviour
{
    [HideInInspector] public string inputText;
    public void ReadStringInput(string text)
    {
        inputText = text;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct KeyCodeCombination 
{ 
    public KeyCode keyCode;
    public EventModifiers keyCombination;

    public KeyCodeCombination(KeyCode keyCode, EventModifiers keyCombination)
    {
        this.keyCode = keyCode;
        this.keyCombination = keyCombination;
    }

    public override string ToString()
    {
        string message = "";
        if (keyCode != KeyCode.None)
            message += $"{keyCode}";
        if (keyCombination != EventModifiers.None)
            message += $" + {keyCombination}";
        return message;
    }
}

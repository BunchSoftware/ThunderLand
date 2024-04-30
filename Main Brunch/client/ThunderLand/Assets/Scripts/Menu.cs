using GrapeNetwork.Client.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        ABC aBC = new ABC();
        aBC.Awake();
        print(1);
    }
}

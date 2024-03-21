using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBar : MonoBehaviour
{
    public Image Handle;
    public Text Amount;
    public int MaxValue = 100;
    public int MinValue = 0;
    [SerializeField] private int _value = 0;

    private void OnValidate()
    {
        Recount();
    }
    public int Value
    {
        get { return _value;  }
        set 
        {
            _value = value;
            Recount();
        }
    }

    private void Recount()
    {
        if (Handle != null)
            if (_value >= MinValue & _value <= MaxValue)
                Handle.fillAmount = (float)(_value) / MaxValue;
            else
                Handle.fillAmount = 0;
        if (Amount != null)
            if (_value >= MinValue & _value <= MaxValue)
                Amount.text = (_value).ToString() + "/" + MaxValue.ToString();
            else
                Amount.text = "0/" + MaxValue.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCounter : MonoBehaviour
{
    [SerializeField]
    private TextMesh _targetCounterText;

    private int _counter = 0;

    public void IncreaseCounter()
    {
        if (_targetCounterText != null)
        {
            _counter++;
            _targetCounterText.text = "Button Clicked: " + _counter;
        }
    }
}

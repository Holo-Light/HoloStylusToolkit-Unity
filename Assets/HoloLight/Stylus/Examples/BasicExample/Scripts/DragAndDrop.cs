using HoloLight.HoloStylus.Configuration;
using HoloLight.HoloStylus.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour, IStylusButtonHandler
{
    private bool _isHolding = false;
    private Vector3 _deltaPosition = Vector3.zero;

    public void OnStylusButtonDown(int sourceID)
    {
        if (Globals.ACTION_BUTTON == sourceID)
        {
            _deltaPosition = transform.position - InputManager.Instance.StylusTransform.Position;
            _isHolding = true;
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    public void OnStylusButtonUp(int sourceID)
    {
        if (Globals.ACTION_BUTTON == sourceID)
        {
            _isHolding = false;
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    public void OnStylusButton(int sourceID, float value)
    {
    }

    void Update()
    {
        if (_isHolding) { 
            transform.position = InputManager.Instance.StylusTransform.Position + _deltaPosition;
        }
    }
}

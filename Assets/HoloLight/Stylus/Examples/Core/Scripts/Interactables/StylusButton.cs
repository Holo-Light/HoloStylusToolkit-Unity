using HoloLight.HoloStylus.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StylusButton : MonoBehaviour, IStylusActionClick, IStylusBackClick, IStylusFocusHandler
{
    [Tooltip("Triggers click functions, even when the object is not focused")]
    [SerializeField]
    public bool IsGlobal = false;

    [Header("Events")]
    public UnityEvent OnStylusActionClick;
    public UnityEvent OnStylusBackClick;
    public UnityEvent OnStylusFocusEnter;
    public UnityEvent OnStylusFocusExit;

    void OnEnable()
    {
        if (IsGlobal)
        {
            InputManager.Instance.GlobalListeners.Add(gameObject);
        }
    }

    void OnDisable()
    {
        if (IsGlobal)
        {
            InputManager.Instance.GlobalListeners.Remove(gameObject);
        }
    }

    public void OnStylusEnter()
    {
        if (OnStylusFocusEnter != null)
        {
            OnStylusFocusEnter.Invoke();
        }
    }

    public void OnStylusExit()
    {
        if (OnStylusFocusExit != null)
        {
            OnStylusFocusExit.Invoke();
        }
    }

    void IStylusActionClick.OnStylusActionClick()
    {
        if (OnStylusActionClick != null)
        {
            OnStylusActionClick.Invoke();
        }
    }

    void IStylusBackClick.OnStylusBackClick()
    {
        if (OnStylusBackClick != null)
        {
            OnStylusBackClick.Invoke();
        }
    }
}
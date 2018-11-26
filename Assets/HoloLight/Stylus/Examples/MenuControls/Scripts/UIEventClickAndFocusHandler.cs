#region copyright
/*******************************************************
 * Copyright (C) 2017-2018 Holo-Light GmbH -> <info@holo-light.com>
 * 
 * This file is part of the Stylus SDK.
 * 
 * Stylus SDK can not be copied and/or distributed without the express
 * permission of the Holo-Light GmbH
 * 
 * Author of this file is Peter Roth
 *******************************************************/
#endregion
using HoloLight.HoloStylus.Configuration;
using HoloLight.HoloStylus.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.MenuControls
{
    /// <summary>
    /// Obsolete: Example for UI clicks with the Holo-Stylus®
    /// </summary>
    public class UIEventClickAndFocusHandler : MonoBehaviour, IStylusActionClick, IStylusFocusHandler, IStylusButtonHandler
    {
        private Camera _currentCamera;
        private Camera _mainCamera
        {
            get
            {
                if (_currentCamera == null)
                {
                    _currentCamera = Camera.main;
                    if (_currentCamera == null)
                    {
                        _currentCamera = FindObjectOfType<Camera>();
                        if (_currentCamera == null)
                        {
                            Debug.LogError("No camera in scene.");
                        }
                    }
                }

                return _currentCamera;

            }
        }

        public void OnStylusEnter()
        {
            this.EventHandler(ExecuteEvents.selectHandler);
        }

        public void OnStylusExit()
        {
            this.EventHandler(ExecuteEvents.deselectHandler);
        }

        void EventHandler<T>(ExecuteEvents.EventFunction<T> handler) where T : IEventSystemHandler
        {
            ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), handler);
        }

        public void OnStylusActionClick()
        {
            this.EventHandler(ExecuteEvents.submitHandler);
        }

        public void OnStylusButtonDown(int sourceID)
        {

        }

        public void OnStylusButtonUp(int sourceID)
        {

        }

        public void OnStylusButton(int sourceID, float value)
        {

        }

        void Update()
        {

        }
    }
}

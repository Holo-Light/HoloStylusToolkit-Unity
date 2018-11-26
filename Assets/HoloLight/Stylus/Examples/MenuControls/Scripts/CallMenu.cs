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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLight.HoloStylus.InputModule;
using HoloLight.HoloStylus.Configuration;
using UnityEngine.Events;

namespace HoloLight.HoloStylus.Examples.MenuControls
{
    /// <summary>
    /// Simple method for calling something on back click
    /// </summary>
    public class CallMenu : MonoBehaviour, IStylusBackClick
    {
        [SerializeField, Tooltip("Call method")]
        private UnityEvent _onCall = new UnityEvent();
        // Access for the input manager instance
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        // Registering on back click
        void Start()
        {
            if (_input != null)
            {
                _input.GlobalListeners.Add(gameObject);
            }
        }

        // Deregister back click
        void OnDestroy()
        {
            if (_input != null)
            {
                _input.GlobalListeners.Remove(gameObject);
            }
        }

        /// <summary>
        /// Invokes the on call unity event 
        /// </summary>
        public void OnStylusBackClick()
        {
            _onCall.Invoke();
        }

        /// <summary>
        /// Manual invoke of the unity event "OnCall"
        /// </summary>
        public void ManualCall()
        {
            _onCall.Invoke();
        }
    }
}

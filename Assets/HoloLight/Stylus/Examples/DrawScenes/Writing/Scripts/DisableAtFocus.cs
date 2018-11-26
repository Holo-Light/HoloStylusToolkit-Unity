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
using HoloLight.HoloStylus.InputModule;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.Tests
{
    /// <summary>
    /// Disables mesh renderer at focusing
    /// </summary>
    public class DisableAtFocus : MonoBehaviour, IStylusFocusHandler
    {
        /// <summary>
        /// Called if stylus focus this gameobject
        /// </summary>
        public void OnStylusEnter()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        /// <summary>
        /// Called if stylus stops focusing this gameobject
        /// </summary>
        public void OnStylusExit() { }
    }
}

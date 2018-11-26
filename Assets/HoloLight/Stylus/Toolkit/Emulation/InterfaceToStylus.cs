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

namespace HoloLight.HoloStylus.Emulation
{
    /// <summary>
    /// Motherclass of all stylus interfaces to the InputManager.
    /// </summary>
    public class InterfaceToStylus : MonoBehaviour
    {
        /// <summary>
        /// Shortcut and readonly variable for Inputmanager.Instance
        /// </summary>
        protected InputManager InputInstance
        {
            get
            {
                return InputManager.Instance;
            }
        }
    }
}
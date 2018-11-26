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

namespace HoloLight.HoloStylus.Examples.SceneCollection
{
    /// <summary>
    /// Class for following stylus transform data.
    /// </summary>
    public class StylusCursor : MonoBehaviour
    {
        /// <summary>
        /// Shortcut and readonly variable for Inputmanager.Instance
        /// </summary>
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// Updates the cursor position
        /// </summary>
        private void Update()
        {
            transform.position = _input.StylusTransformRaw.Position;
        }
    }
}

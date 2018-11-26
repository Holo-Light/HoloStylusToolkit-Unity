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
using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Stores position and timestamp delta of the last position.
    /// </summary>
    internal class PositionInfo
    {
        /// <summary>
        /// The position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Delta of the position timestamps.
        /// </summary>
        public float DeltaTime;
    }
}
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
using System;

namespace HoloLight.HoloStylus.StatusModule
{
    /// <summary>
    /// The battery data, inclusive critical and low level.
    /// </summary>
    [Serializable]
    public struct BatteryStatus
    {
        /// <summary>
        /// The charged battery level
        /// </summary>
        public float Charge;
        /// <summary>
        /// Warning constant for low battery
        /// </summary>
        public const float LOW_LEVEL = 0.2f;
        /// <summary>
        /// Critical constant for extrem low battery
        /// </summary>
        public const float CRITICAL_LEVEL = 0.05f;

        /// <summary>
        /// Sets the current charge level
        /// </summary>
        /// <param name="charge"></param>
        public BatteryStatus(float charge)
        {
            Charge = charge;
        }
    }
}
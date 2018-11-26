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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HoloLight.HoloStylus.Examples.Calibration
{
    /// <summary>
    /// The three axis, of the stylus
    /// </summary>
    public enum StylusAxis
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Helper class for saving the calibration file
    /// </summary>
    public static class CalibrationManager
    {
        /// <summary>
        /// Add axis value to offset of the calibration file.
        /// </summary>
        /// <param name="axis">Axis type (x,y,z)</param>
        /// <param name="value">Additive offset value</param>
        public static void AddValueToAxis(StylusAxis axis, float value)
        {
            var data = new CalibrationPreferences.Data(CalibrationPreferences.CalibrationData);
            var offset = data.Offset;
            offset += Vector3.right * ((axis == StylusAxis.X) ? value : 0);
            offset += Vector3.up * ((axis == StylusAxis.Y) ? value : 0);
            offset += Vector3.forward * ((axis == StylusAxis.Z) ? value : 0);
            data.Offset = offset;
            CalibrationPreferences.CalibrationData = data;
            CalibrationPreferences.Save();
        }

        /// <summary>
        /// Set and saves the offset
        /// </summary>
        /// <param name="offset">Position offset of the stylus</param>
        public static void SetOffset(Vector3 offset)
        {
            var data = new CalibrationPreferences.Data(CalibrationPreferences.CalibrationData)
            {
                Offset = offset
            };
            CalibrationPreferences.CalibrationData = data;
            CalibrationPreferences.Save();
        }
    }
}

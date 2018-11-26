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

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Preference class for calibration values
    /// </summary>
    public static class CalibrationPreferences
    {
        /// <summary>
        /// Tags of the axis
        /// </summary>
        internal static class PrefsTags
        {
            public const string OFFSET_X = "StylusOffsetX";
            public const string OFFSET_Y = "StylusOffsetY";
            public const string OFFSET_Z = "StylusOffsetZ";
        }

        /// <summary>
        /// Calibration data class
        /// </summary>
        public class Data
        {
            public Vector3 Offset = Vector3.zero;

            public Data()
            {

            }

            public Data(Data data)
            {
                this.Offset = data.Offset;
            }
        }

        // Loaded flag
        private static bool _isLoaded = false;
        // hidden calibration data
        private static Data _data;
        /// <summary>
        /// Calibration data, like offset
        /// Loads data, if it's not loaded
        /// </summary>
        public static Data CalibrationData
        {
            get
            {
                if (!_isLoaded)
                {
                    _data = LoadCalibrationData();
                }
                return _data;
            }
            set
            {
                SaveData(value);
            }
        }

        // Load and returns calibration data from player prefs file
        private static Data LoadCalibrationData()
        {
            var data = new Data();
            _isLoaded = true;

            float offsetx = GetFloat(PrefsTags.OFFSET_X);
            float offsety = GetFloat(PrefsTags.OFFSET_Y);
            float offsetz = GetFloat(PrefsTags.OFFSET_Z);

            data.Offset = new Vector3(offsetx, offsety, offsetz);

            if(data.Offset == Vector3.zero)
            {
                data.Offset = Vector3.zero;
            }
            return data;
        }

        // Save calibration data
        private static void SaveData(Data data)
        {
            _data = data;

            PlayerPrefs.SetFloat(PrefsTags.OFFSET_X, data.Offset.x);
            PlayerPrefs.SetFloat(PrefsTags.OFFSET_Y, data.Offset.y);
            PlayerPrefs.SetFloat(PrefsTags.OFFSET_Z, data.Offset.z);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Save calibration data
        /// </summary>
        public static void Save()
        {
            SaveData(_data);
            PlayerPrefs.Save();
        }

        // Returns player prefs float if exists, else it returns 0
        private static float GetFloat(string tag)
        {
            if (PlayerPrefs.HasKey(tag))
            {
                return PlayerPrefs.GetFloat(tag);
            }
            else
            {
                return 0;
            }
        }

    }
}
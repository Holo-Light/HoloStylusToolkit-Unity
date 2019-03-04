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

            public const string HMU_POSITION_X = "HMUPositionX";
            public const string HMU_POSITION_Y = "HMUPositionY";
            public const string HMU_POSITION_Z = "HMUPositionZ";
            public const string HMU_ROTATION_X = "HMURotationX";
            public const string HMU_ROTATION_Y = "HMURotationY";
            public const string HMU_ROTATION_Z = "HMURotationZ";
            public const string HMU_ROTATION_W = "HMURotationW";

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

        public class HMUData
        {
            public Vector3 Position = Vector3.zero;
            public Quaternion Rotation = Quaternion.identity;

            public HMUData()
            {

            }

            public HMUData(HMUData data)
            {
                this.Position = data.Position;
                this.Rotation = data.Rotation;
            }
        }

        // Loaded flag
        private static bool _isLoaded = false;
        // HMU loaded flag
        private static bool _isHMULoaded = false;

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

        // hidden calibration data
        private static HMUData _hmuData;
        /// <summary>
        /// Calibration hmu data, like position and rotation
        /// Loads data, if it's not loaded
        /// </summary>
        public static HMUData HMUCalibrationData
        {
            get
            {
                if (!_isHMULoaded)
                {
                    _hmuData = LoadHMUCalibration();
                }
                return _hmuData;
            }
            set
            {
                SaveHMU(value);
            }
        }

        // Load and returns calibration data from player prefs file
        private static HMUData LoadHMUCalibration()
        {
            var data = new HMUData();
            _isHMULoaded = true;

            float posx = GetFloat(PrefsTags.HMU_POSITION_X);
            float posy = GetFloat(PrefsTags.HMU_POSITION_Y);
            float posz = GetFloat(PrefsTags.HMU_POSITION_Z);

            float rotx = GetFloat(PrefsTags.HMU_ROTATION_X);
            float roty = GetFloat(PrefsTags.HMU_ROTATION_Y);
            float rotz = GetFloat(PrefsTags.HMU_ROTATION_Z);
            float rotw = GetFloat(PrefsTags.HMU_ROTATION_W);

            data.Position = new Vector3(posx, posy, posz);
            data.Rotation = new Quaternion(rotx, roty, rotz, rotw);

            return data;
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

        public static void SaveHMU(HMUData data)
        {
            PlayerPrefs.SetFloat(PrefsTags.HMU_POSITION_X, data.Position.x);
            PlayerPrefs.SetFloat(PrefsTags.HMU_POSITION_Y, data.Position.y);
            PlayerPrefs.SetFloat(PrefsTags.HMU_POSITION_Z, data.Position.z);
            PlayerPrefs.SetFloat(PrefsTags.HMU_ROTATION_X, data.Rotation.x);
            PlayerPrefs.SetFloat(PrefsTags.HMU_ROTATION_Y, data.Rotation.y);
            PlayerPrefs.SetFloat(PrefsTags.HMU_ROTATION_Z, data.Rotation.z);
            PlayerPrefs.SetFloat(PrefsTags.HMU_ROTATION_W, data.Rotation.w);
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
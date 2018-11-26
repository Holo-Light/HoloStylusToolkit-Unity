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
using UnityEngine;

namespace HoloLight.HoloStylus.Configuration
{
    [Serializable]
    public class Thresholds
    {
        /// <summary>
        /// Movement threshold.
        /// </summary>
        [Tooltip("Movement threshold.")]
        public float Movement = 0.2f;

        /// <summary>
        /// Rotation threshold.
        /// </summary>
        [Tooltip("Rotation threshold")]
        public float Rotation = 0.2f;

        /// <summary>
        /// Acceleration threshold
        /// </summary>
        [Tooltip("Acceleration threshold")]
        public float Acceleration = 0.2f;

        /// <summary>
        /// Button pressure threshold
        /// </summary>
        [Tooltip("Button pressure threshold")]
        public float ButtonPressureThreshold = 0.02f;

        /// <summary>
        /// Sensivity, multiplies the pressure
        /// </summary>
        [Tooltip("Sensivity, multiplies the pressure")]
        public float ButtonSensitivity = 1.0f;

        /// <summary>
        /// Sets all button values below this value to zero
        /// </summary>
        [Tooltip("Sets all button values below this value to zero")]
        public float ButtonDeadzone = 0.05f;

        /// <summary>
        /// Snaps all button values higher this value to one
        /// </summary>
        [Tooltip("Snaps all button values higher this value to one")]
        public float ButtonGravity = 0.95f;

        /// <summary>
        /// Calculate the threshold
        /// </summary>
        /// <returns></returns>
        public Thresholds CreateCalculatedThresholds()
        {
            var newThreshold = new Thresholds
            {
                Movement = Mathf.Pow(this.Movement, 2.0f),
                Rotation = Mathf.Pow(this.Rotation, 2.0f),
                Acceleration = Mathf.Pow(this.Acceleration, 2.0f),

                ButtonGravity = this.ButtonGravity,
                ButtonDeadzone = this.ButtonDeadzone,
                ButtonPressureThreshold = this.ButtonPressureThreshold,
                ButtonSensitivity = this.ButtonSensitivity
            };

            return newThreshold;
        }
    }

    /// <summary>
    /// Stylus configuration asset, e.g. for threshold.
    /// </summary>
    [CreateAssetMenu(fileName = "StylusConfig", menuName = "Stylus/Configuration", order = 0)]
    public class ConfigurationObject : ScriptableObject
    {
        [SerializeField, Tooltip("All thresholds of the threshold container.")]
        private Thresholds _thresholds;

        /// <summary>
        /// Threshold to avoid stylus flickering.
        /// </summary>
        public Thresholds Thresholds
        {
            get
            {
                if(_thresholds == null)
                {
                    _thresholds = new Thresholds();
                }
                return _thresholds;
            }
        }
    }
}
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

namespace HoloLight.HoloStylus.StatusModule
{
    /// <summary>
    /// Manage statuses of the Stylus and HMU, e.g. Battery, SourceState, etc.
    /// </summary>
    public class StatusManager : MonoBehaviour // todo: vielleicht kein monobehaviour benötigt
    {
        /// <summary>
        /// Singleton of the StatusManager.
        /// </summary>
        public static StatusManager Instance;

        /// <summary>
        /// Raised if the battery of the Stylus is below the battery low constant value.
        /// </summary>
        public event DeviceBatteryHandler OnStylusBatteryLow;

        /// <summary>
        /// Raised if the battery of the Stylus is below the battery critical constant value.
        /// </summary>
        public event DeviceBatteryHandler OnStylusBatteryCritical;

        /// <summary>
        /// Raised if the battery of the Stylus has changed.
        /// </summary>
        public event DeviceBatteryHandler OnStylusBatteryChanged;

        /// <summary>
        /// Raised if the battery of the HMU is below the battery low constant value.
        /// </summary>
        public event DeviceBatteryHandler OnHMUBatteryLow;

        /// <summary>
        /// Raised if the battery of the HMU is below the battery critical constant value.
        /// </summary>
        public event DeviceBatteryHandler OnHMUBatteryCritical;

        /// <summary>
        /// Raised if the battery of the HMU changed.
        /// </summary>
        public event DeviceBatteryHandler OnHMUBatteryChanged;


        private BatteryStatus _HMUBatteryLevel = new BatteryStatus(1);

        /// <summary>
        /// The current battery level of the HMU.
        /// </summary>
        public float HMUBatteryLevel
        {
            get
            {
                return _HMUBatteryLevel.Charge;
            }
            set
            {
                _HMUBatteryLevel.Charge = value;
                RaiseHMUBatteryLevelHandlers(value);
            }
        }

        private BatteryStatus _stylusBatteryLevel = new BatteryStatus(1);

        /// <summary>
        /// The current battery level of the Stylus.
        /// </summary>
        public float StylusBatteryLevel
        {
            get
            {
                return _stylusBatteryLevel.Charge;
            }
            set
            {
                _stylusBatteryLevel.Charge = value;
                RaiseStylusBatteryLevelHandlers(value);
            }
        }

        // Initialize the status manager singleton
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        // Raise events, if hmu battery status is critical oder low
        private void RaiseHMUBatteryLevelHandlers(float value)
        {
            if (value <= BatteryStatus.CRITICAL_LEVEL)
            {
                RaiseBatteryHandler(OnHMUBatteryCritical, value);
            }
            else if (value <= BatteryStatus.LOW_LEVEL)
            {
                RaiseBatteryHandler(OnHMUBatteryLow, value);
            }

            RaiseBatteryHandler(OnHMUBatteryChanged, value);
        }

        // Raise events, if stylus battery status is critical oder low
        private void RaiseStylusBatteryLevelHandlers(float value)
        {
            if (value <= BatteryStatus.CRITICAL_LEVEL)
            {
                RaiseBatteryHandler(OnStylusBatteryCritical, value);
            }
            else if (value <= BatteryStatus.LOW_LEVEL)
            {
                RaiseBatteryHandler(OnStylusBatteryLow, value);
            }

            RaiseBatteryHandler(OnStylusBatteryChanged, value);
        }

        // General raise handler
        private void RaiseBatteryHandler(DeviceBatteryHandler handler, float value)
        {
            if (handler != null)
            {
                handler(value);
            }
        }
    }
}
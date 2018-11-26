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
using HoloLight.HoloStylus.Connection;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using HoloLight.HoloStylus.Connection.Editor;
#endif
#if WINDOWS_UWP
using HoloLight.DriverLibrary.Events;
using Windows.Devices.Enumeration;
using HoloLight.DriverLibrary;
using HoloLight.DriverLibrary.DeviceDiscovery;
using HoloLight.DriverLibrary.Devices;
using HoloLight.DriverLibrary.Data;
#endif

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Switches between base input modules to platform based input module.
    /// </summary>
    [RequireComponent(typeof(BaseInputModule))]
    public class EventSystemSwitcher : MonoBehaviour
    {
        /// <summary>
        /// Platform name
        /// </summary>
        public const string HOLOGRAPHIC_PLATFORM_NAME = "Windows.Holographic";

        /// <summary>
        /// Input module for pc app
        /// </summary>
        public BaseInputModule StandaloneInputModule;

        /// <summary>
        /// Input module for windows holographic app
        /// </summary>
        public BaseInputModule GazeInputModule;

        /// <summary>
        /// Stylus input module
        /// </summary>
        public BaseInputModule StylusInputModule;

        /// <summary>
        /// Module type for different input modules and base inputs.
        /// </summary>
        internal enum InputModuleType
        {
            Standalone,
            Gaze,
            Stylus
        }

#if UNITY_EDITOR
        [SerializeField, Tooltip("Preferred module for editor.")]
        private InputModuleType EditorType = InputModuleType.Standalone;
#endif

        /// <summary>
        /// Initialize, set start input module.
        /// </summary>
        private void Start()
        {
            if(DeviceManager.Instance != null)
            {
                DeviceManager.Instance.OnConnect += OnConnect;
            }
#if UNITY_EDITOR
            EnableModule(EditorType);
#elif WINDOWS_UWP
            var vi = Windows.System.Profile.AnalyticsInfo.VersionInfo;

            if(vi.DeviceFamily == HOLOGRAPHIC_PLATFORM_NAME)
            {
                EnableModule(InputModuleType.Gaze);
            }
            else
            {
                EnableModule(InputModuleType.Standalone);
            }
#endif
        }

        /// <summary>
        /// Activate new input module
        /// </summary>
        /// <param name="type">Input module type</param>
        private void EnableModule(InputModuleType type)
        {
            StandaloneInputModule.enabled = type == InputModuleType.Standalone;
            GazeInputModule.enabled = type == InputModuleType.Gaze;
            StylusInputModule.enabled = type == InputModuleType.Stylus;
        }

        /// <summary>
        /// Starts stylus input module on connect
        /// </summary>
        /// <param name="deviceInfo">Stylus device</param>
        private void OnConnect(DeviceInformation deviceInfo)
        {
            EnableModule(InputModuleType.Stylus);
        }
    }
}

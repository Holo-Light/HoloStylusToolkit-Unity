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

#if WINDOWS_UWP
namespace HoloLight.HoloStylus.Connection.Editor
{
    /// <summary>
    /// Template class will be destroyed on build
    /// </summary>
    public class DeviceDiscoveryTest : MonoBehaviour
    {
        // Destroy this instance after loading
        void Awake()
        {
            DestroyImmediate(gameObject);
        }
    }
}
#elif UNITY_EDITOR
using UnityEditor;

namespace HoloLight.HoloStylus.Connection.Editor
{
    /// <summary>
    /// Template class for faking device discovery.
    /// In future updates, maybe device discovery also works in editor
    /// </summary>
    public class DeviceDiscoveryTest : MonoBehaviour
    {
        // Access to the device manager
        private DeviceManager _deviceManager
        {
            get
            {
                return DeviceManager.Instance;
            }
        }

        // Access to the device watcher of the device manager
        private DeviceWatcher _deviceWatcher
        {
            get
            {
                return _deviceManager.DeviceDiscovery.DeviceWatcher;
            }
        }

        // Stops the device search after destruction
        private void OnDestroy()
        {
            if(_deviceManager == null)
            {
                return;
            }
            _deviceManager.EndDeviceSearch();
        }

        // Sample device informations
        private DeviceInformation _deviceTestInformation1 = new DeviceInformation { Id = "abcd", Name = "First Test BLE Device" };
        private DeviceInformation _deviceTestInformation2 = new DeviceInformation { Id = "efgh", Name = "Second Test BLE Device" };

        // Create buttons with sample device informations, starts and stops searching
        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _deviceWatcher.Added(_deviceWatcher, _deviceTestInformation1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _deviceWatcher.Added(_deviceWatcher, _deviceTestInformation2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _deviceWatcher.EnumerationCompleted(_deviceWatcher, null);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _deviceWatcher.Stopped(_deviceWatcher, null);
            }
        }

    }

    /// <summary>
    /// This device discovery fake all neccessary UWP methods for UNITY
    /// </summary>
    public class DeviceDiscovery
    {
        public DeviceWatcher DeviceWatcher = new DeviceWatcher();
    }

    /// <summary>
    /// This device discovery fake all neccessary UWP methods for UNITY
    /// </summary>
    public class DeviceWatcher
    {
        public delegate void AddHandler(DeviceWatcher sender, DeviceInformation args);
        public delegate void EnumerationCompletedHandler(DeviceWatcher sender, object args);
        public delegate void StoppedHandler(DeviceWatcher sender, object args);
        public AddHandler Added;
        public EnumerationCompletedHandler EnumerationCompleted;
        public StoppedHandler Stopped;

        public void Start() { }
        public void Stop() { }

        public DeviceWatcherStatus Status = DeviceWatcherStatus.Stopped;

    }

    /// <summary>
    /// Faking enumeration for the status of the device watcher
    /// </summary>
    public enum DeviceWatcherStatus
    {
        Started,
        Stopped,
        Aborted,
        Created,
        Stopping,
        EnumerationCompleted
    }

    /// <summary>
    /// This device information fake all neccessary UWP methods for UNITY
    /// </summary>
    [Serializable]
    public class DeviceInformation : EventArgs
    {
        public string Id;
        public string Name;
    }

    /// <summary>
    /// Event arguments for faking status of the stylus connection
    /// </summary>
    public class  StylusEventArgs : EventArgs
    {
        public StylusData StylusData { get; private set; }
        public StylusEventArgs() { }

        public StylusEventArgs(StylusData data)
        {
            this.StylusData = data;
        }
    }

    /// <summary>
    /// Faking class for the stylus data (position, button pressure, raw data)
    /// </summary>
    public class StylusData
    {
        public static float ButtonThreshold = 0.000001f;

        public float ActionButton { get; set; }
        public float BackButton { get; set; }
        public Vector3HMU Position { get; set; }

        public byte[] RawData { get; set; }

        public StylusData()
        {

        }

        public bool IsFrontButtonPressed;
        public bool IsBackButtonPressed;
    }

    /// <summary>
    /// Vector3 with Pascal cased axis (like the System data type)
    /// </summary>
    public struct Vector3HMU
    {
        public float X, Y, Z;
    }

    /// <summary>
    /// Fake class of stylus control
    /// </summary>
    public class StylusControl
    {

    }

    /// <summary>
    /// Feedback of the device discovery test in the editor.
    /// </summary>
    [CustomEditor(typeof(DeviceDiscoveryTest))]
    public class DeviceDiscoveryTestEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("This discovery serves only for the UnityEditor.", MessageType.Warning);
        }
    }
}
#endif

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
using System.Collections;
using System.Collections.Generic;
using HoloLight.HoloStylus.Connection;
#if WINDOWS_UWP
using Windows.Devices.Enumeration;
using HoloLight.DriverLibrary;
using HoloLight.DriverLibrary.DeviceDiscovery;
using HoloLight.DriverLibrary.Devices;
#endif
#if UNITY_EDITOR
using HoloLight.HoloStylus.Connection.Editor;
#endif

namespace HoloLight.HoloStylus.Examples.Connection
{
    /// <summary>
    /// Example class for handling connecting to the device
    /// </summary>
    public class DeviceButtonManager : MonoBehaviour
    {
        // Access to the device manager singleton
        private DeviceManager _deviceManager
        {
            get
            {
                return DeviceManager.Instance;
            }
        }

        [SerializeField, Tooltip("Prefab for the device buttons")]
        private ConnectToDeviceButton _buttonPrefab;
        [SerializeField, Tooltip("Scene parent for the device buttons")]
        private Transform _buttonParent;

        /// <summary>
        /// Possible devices are listed in this queue
        /// </summary>
        public Queue<DeviceInformation> deviceQueue = new Queue<DeviceInformation>();

        // register the callback for new devices found
        private void Start()
        {
            if (_deviceManager != null)
            {
                _deviceManager.OnDeviceFound += OnDeviceFound;
            }
        }

        // lists all devices which was found after last enable
        private void OnEnable()
        {
            if(_deviceManager == null || _deviceManager.GetFoundDevices() == null)
            {
                return;
            }
            foreach(var device in _deviceManager.GetFoundDevices())
            {
                CreateButton(device);
            }

        }

        // deregister the callback for new devices found
        private void OnDestroy()
        {
            if (_deviceManager != null)
            {
                _deviceManager.OnDeviceFound -= OnDeviceFound;
            }
        }

        /// <summary>
        /// Event called if any BLE device is found.
        /// </summary>
        /// <param name="device"></param>
        public void OnDeviceFound(DeviceInformation device)
        {
            deviceQueue.Enqueue(device);
        }

        // Handles the filled queue
        private void Update()
        {
            if (deviceQueue.Count > 0)
            {
                var device = deviceQueue.Dequeue();
                CreateButton(device);
            }
        }

        /// <summary>
        /// Create the instance of the device button
        /// </summary>
        /// <param name="device"></param>
        void CreateButton(DeviceInformation device)
        {
            var buttonGO = Instantiate<GameObject>(_buttonPrefab.gameObject, _buttonParent);
            var button = buttonGO.GetComponent<ConnectToDeviceButton>();

            button.Device = device;
        }
    }
}
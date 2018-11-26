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
using UnityEngine.UI;
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
    /// Connection button (helper class, create your own one, if you need a more fancy one :)
    /// </summary>
    public class ConnectToDeviceButton : MonoBehaviour
    {
        // Current device information
        private DeviceInformation _device;

        /// <summary>
        /// Current device information
        /// </summary>
        public DeviceInformation Device
        {
            get
            {
                return _device;
            }
            set
            {
                _device = value;
                _deviceTextField.text = _device.Name + " - " + _device.Id;
            }
        }

        // Access to the device manager
        private DeviceManager _deviceManager
        {
            get
            {
                return DeviceManager.Instance;
            }
        }

        // Access to the text field of the connection button
        private Text _deviceTextField
        {
            get
            {
                return GetComponentInChildren<Text>();
            }
        }

        /// <summary>
        /// Connect to device method (connects with the current device information)
        /// </summary>
        public void ConnectToDevice()
        {
            if(Device != null && _deviceManager != null)
            {
                _deviceManager.ConnectToDevice(Device);
            }
        }        
    }
}
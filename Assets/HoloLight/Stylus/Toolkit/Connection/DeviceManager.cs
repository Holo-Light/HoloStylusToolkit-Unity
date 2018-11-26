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
using System.Collections.Generic;
using HoloLight.HoloStylus.InputModule;
using System.Linq;

#if WINDOWS_UWP
using HoloLight.DriverLibrary.Events;
using Windows.Devices.Enumeration;
using HoloLight.DriverLibrary;
using HoloLight.DriverLibrary.DeviceDiscovery;
using HoloLight.DriverLibrary.Devices;
using HoloLight.DriverLibrary.Data;
#endif
#if UNITY_EDITOR
using HoloLight.HoloStylus.Connection.Editor;
#endif

namespace HoloLight.HoloStylus.Connection
{
    /// <summary>
    /// Base class for all connection stuff
    /// </summary>
    public class DeviceManager : MonoBehaviour
    {
        // Access to the input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// Device found event handler.
        /// </summary>
        /// <param name="device"></param>
        public delegate void DeviceFoundEventHandler(DeviceInformation device);
        /// <summary>
        /// Event for device found, called after device is listed by the device watcher
        /// </summary>
        public event DeviceFoundEventHandler OnDeviceFound;
        /// <summary>
        /// Event for the connection event, called after connection finished and services are avaible
        /// </summary>
        public event DeviceFoundEventHandler OnConnect;
        // Flag for continous watching of devices
        private bool _continueSearch = true;
        /// <summary>
        /// Current device discovery, contains device watcher
        /// </summary>
        public DeviceDiscovery DeviceDiscovery = new DeviceDiscovery();
        // current hmu device
        private DeviceInformation _hmuDevice;
        // current hmus status
        private StylusEventArgs _HMUStatus;
        // flag for data recieved
        private bool _receivingdata = false;
        // current control of the HMU
        private StylusControl _headmountedUnit;
        /// <summary>
        /// singleton access
        /// </summary>
        public static DeviceManager Instance;
        // lists of all found devices
        private readonly List<DeviceInformation> _devices = new List<DeviceInformation>();
        // flag for connection allowed
        private bool _canConnect = true;

        // Initializing singleton and dont destroy on load, device manager is persistent
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        // resets singleton
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Sets the stylus data, after connection
        /// </summary>
        /// <param name="e"></param>
        public void SetStylusData(StylusEventArgs e)
        {

            if (_input != null && (_receivingdata))
            {
                Vector3 Position = new Vector3(e.StylusData.Position.X, e.StylusData.Position.Y, e.StylusData.Position.Z);
                StylusButtonData Button1 = new StylusButtonData { SourceID = 1, Pressure = e.StylusData.ActionButton };
                StylusButtonData Button2 = new StylusButtonData { SourceID = 0, Pressure = e.StylusData.BackButton };
                _input.StylusTransformRaw = new StylusTransformData { Position = Position };
                _input.StylusActionButtonData = Button1;
                _input.StylusBackButtonData = Button2;

            }
            _receivingdata = false;
        }

        /// <summary>
        /// Called if event arguments changed
        /// </summary>
        /// <param name="e"></param>
        public void OnStatusChanged(StylusEventArgs e)
        {
            _HMUStatus = e;

            _receivingdata = true;

        }

        /// <summary>
        /// Starts the device search
        /// </summary>
        public void StartDeviceSearch()
        {
            if (DeviceDiscovery.DeviceWatcher == null)
            {
                Debug.LogError("DeviceDiscovery is null");
                return;
            }
            DeviceDiscovery.DeviceWatcher.Added += DeviceWatcherAdded;
            DeviceDiscovery.DeviceWatcher.EnumerationCompleted += DeviceWatcherEnumerationCompleted;
            DeviceDiscovery.DeviceWatcher.Stopped += DeviceWatcherStopped;
            DeviceDiscovery.DeviceWatcher.Start();
        }

        /// <summary>
        /// Show listed devices
        /// </summary>
        /// <returns></returns>
        public List<DeviceInformation> GetFoundDevices()
        {
            return _devices;
        }

        /// <summary>
        /// Stops device search
        /// </summary>
        public void EndDeviceSearch()
        {
            _continueSearch = false;
            if (DeviceDiscovery.DeviceWatcher.Status == DeviceWatcherStatus.Started)
            {
                DeviceDiscovery.DeviceWatcher.Stop();
            }
        }

#if WINDOWS_UWP
        /// <summary>
        /// Connects to the device
        /// </summary>
        /// <param name="deviceInfo"></param>
        public async void ConnectToDevice(DeviceInformation deviceInfo)
        {
            if(!_canConnect)
            {
                return;
            }

            _canConnect = false;
            
            var config = new StylusConfig(deviceInfo.Id);
            _headmountedUnit = new StylusControl(config);

            await _headmountedUnit.ConnectDevice(deviceInfo);


            EndDeviceSearch();
            StartTracking();
            if(OnConnect == null)
            {
                return;
            }
            OnConnect(deviceInfo);
        }

        /// <summary>
        /// Start access to tracking services
        /// </summary>
        private async void StartTracking()
        {
            if (_headmountedUnit == null)
            {
                return;
            }

            await _headmountedUnit.StartServices();

            _headmountedUnit.StatusChanged += OnStatusChanged;
        }

        /// <summary>
        /// Calls on device found if device is added to the device watcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void DeviceWatcherAdded(DeviceWatcher sender, DeviceInformation args)
        {
            if (OnDeviceFound == null)
                return;

            if(CheckDoubledDevices(args))
            {
                return;
            }

            OnDeviceFound(args);
        }

#elif UNITY_EDITOR
        /// <summary>
        /// Fake: Connects to the device
        /// </summary>
        /// <param name="deviceInfo"></param>
        public void ConnectToDevice(DeviceInformation deviceInfo)
        {
            if (!_canConnect)
            {
                return;
            }

            _hmuDevice = deviceInfo;
            if(OnConnect == null)
            {
                return;
            }
            OnConnect(_hmuDevice);
        }

        /// <summary>
        /// Fake: Start access to tracking services
        /// </summary>
        private void StartTracking() { }

        /// <summary>
        /// Fake: Calls on device found if device is added to the device watcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DeviceWatcherAdded(DeviceWatcher sender, DeviceInformation args)
        {
            if (CheckDoubledDevices(args))
            {
                return;
            }

            if (OnDeviceFound != null)
            {
                OnDeviceFound(args);
            }
        }
#endif
        /// <summary>
        /// Checks for double sources
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool CheckDoubledDevices(DeviceInformation args)
        {
            var doubleDevices = (_devices.Where(device => device.Id == args.Id)).ToArray();

            if (doubleDevices.Length > 0)
            {
                return true;
            }

            _devices.Add(args);

            return false;
        }


        // Called while device watcher is completed
        private void DeviceWatcherEnumerationCompleted(DeviceWatcher sender, object args)
        {
            if (DeviceDiscovery.DeviceWatcher.Status == DeviceWatcherStatus.Started)
            {
                DeviceDiscovery.DeviceWatcher.Stop();
            }
        }

        // Called if device watcher is stopped
        private void DeviceWatcherStopped(DeviceWatcher sender, object args)
        {
            if (_continueSearch)
            {
                DeviceDiscovery.DeviceWatcher.Start();
            }
            else
            {
                DeviceDiscovery.DeviceWatcher.Added -= DeviceWatcherAdded;
                DeviceDiscovery.DeviceWatcher.EnumerationCompleted -= DeviceWatcherEnumerationCompleted;
                DeviceDiscovery.DeviceWatcher.Stopped -= DeviceWatcherStopped;
            }
        }

        // Initiliaze the device search
        void Start()
        {
            StartDeviceSearch();
            DontDestroyOnLoad(transform.root.gameObject);

        }

        // Refreshs the stylus data
        private void Update()
        {
            SetStylusData(_HMUStatus);
        }
    }
}
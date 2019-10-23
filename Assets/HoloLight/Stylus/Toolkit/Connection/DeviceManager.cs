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
using System.Xml.Serialization;
using System.IO;
#if WINDOWS_UWP
using HoloLight.DriverLibrary.Events;
using Windows.Devices.Enumeration;
using HoloLight.DriverLibrary;
using HoloLight.DriverLibrary.DeviceDiscovery;
using HoloLight.DriverLibrary.Devices;
using HoloLight.DriverLibrary.Data;

using Tracker;
using Windows.Storage;
using System.Threading.Tasks;
using System;

#endif
#if UNITY_EDITOR
using HoloLight.HoloStylus.Connection.Editor;
#endif

namespace HoloLight.HoloStylus.Connection
{
    public enum StylusType
    {
        Version1,
        Version2
    }

    /// <summary>
    /// Base class for all connection stuff
    /// </summary>
    ///
    public class DeviceManager : MonoBehaviour
    {
        //   [SerializeField]
        //   public StylusVersion StylusVersion;

        // Access to the input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        private float _lastButtonEventAction = 0;
        private float _lastButtonEventBack = 0;

#if WINDOWS_UWP
        private IPositionCalculation _positionCalculator;
        private Tracker.Tracker _tracker;
        private float[] _cameraData;
        private float[] positionFloat;
#endif
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

        /// <summary>
        /// If active, Stylus will be auto-connected
        /// </summary>
        public bool AutoConnect = true;

        /// <summary>
        /// Base device name for identification
        /// </summary>
        private string _baseDeviceName = "UNITY_FAKE_RECEIVER";


        /// <summary>
        /// 
        /// </summary>
        public const string SETTINGS_FILE_NAME = "StylusSettings.xml";

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

#if WINDOWS_UWP
        private async void initHMUv2(DeviceInformation device) {
            string filePath = await GetFilePath();
            _positionCalculator = new PositionCalculationHMUv2(filePath);
            _baseDeviceName = _positionCalculator.BaseDeviceName;
            ConnectToDevice(device);
        }
#endif
        // resets singleton
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            if (AutoConnect)
            {
                OnDeviceFound -= OnAutoConnectDeviceFound;
                OnConnect -= OnAutoConnected;
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
                _receivingdata = false;
#if WINDOWS_UWP
                _positionCalculator.StylusData = e;
                StylusButtonData Button1 = new StylusButtonData { SourceID = 1, Pressure = _positionCalculator.Button1 };
                _input.StylusActionButtonData = Button1;
                StylusButtonData Button2 = new StylusButtonData { SourceID = 0, Pressure = _positionCalculator.Button2 };
                _input.StylusBackButtonData = Button2;                  
                _input.StylusTransformRaw = new StylusTransformData { Position = _positionCalculator.Position };
#endif
#if UNITY_EDITOR

                Vector3 Position = new Vector3(e.StylusData.Position.X, e.StylusData.Position.Y, e.StylusData.Position.Z);
                StylusButtonData Button1 = new StylusButtonData { SourceID = 1, Pressure = e.StylusData.ActionButton };
                StylusButtonData Button2 = new StylusButtonData { SourceID = 0, Pressure = e.StylusData.BackButton };
                _input.StylusTransformRaw = new StylusTransformData { Position = Position };
                _input.StylusActionButtonData = Button1;
                _input.StylusBackButtonData = Button2;
#endif

            }

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

        private void OnDeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
        }

        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
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
            DeviceDiscovery.DeviceWatcher.Removed += OnDeviceRemoved;
            DeviceDiscovery.DeviceWatcher.Updated += OnDeviceUpdated;

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

        private Queue<DeviceInformation> _onConnectToMainThread = new Queue<DeviceInformation>();

        private void OnConnectToMainThread(DeviceInformation deviceInfo)
        {
            if (OnConnect == null)
            {
                return;
            }
            OnConnect(deviceInfo);
        }

#if WINDOWS_UWP
        private async Task<string> GetFilePath() {
            StorageFolder _currentStorage = KnownFolders.DocumentsLibrary;
            IReadOnlyList<StorageFile> documentsFileList = await _currentStorage.GetFilesAsync();
        
            string filePath = "";

            for (int i = 0; i < documentsFileList.Count; i++)
            { 
                StorageFile file = documentsFileList.ElementAt(i);
                if (file.Name.ToLower() == "stylus.nnf") {
                    filePath = file.Path;
                    return filePath;
                }
            }
            
            return filePath;
        }

        /// <summary>
        /// Connects to the device
        /// </summary>
        /// <param name="deviceInfo"></param>
        public async void ConnectToDevice(DeviceInformation deviceInfo)
        {
            if (!_canConnect)
            {
                return;
            }

            _canConnect = false;
        // _positionCalculator.Version,    <--- i cant add this as parameter....stylusconfig has only two for the constructor...but can't find that...
            var config = new StylusConfig(deviceInfo.Id);
            _headmountedUnit = new StylusControl(config);

            await _headmountedUnit.ConnectDevice(deviceInfo);


            EndDeviceSearch();
            StartTracking();
            _onConnectToMainThread.Enqueue(deviceInfo);
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

            if (CheckDoubledDevices(args))
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

            _onConnectToMainThread.Enqueue(_hmuDevice);
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
            LoadStylusSettings();
            if (AutoConnect)
            {
                OnDeviceFound += OnAutoConnectDeviceFound;
                OnConnect += OnAutoConnected;
            }
            // because accessing the directory and finding the path of the .nnf file is async....and when it finds devices and checks the basename..it doesnt conenct because _baseName hasn't changed yet ..i call StartSearch on V2 when it has already finished finding the path (initHMUv2())

            StartDeviceSearch();
            DontDestroyOnLoad(transform.root.gameObject);
        }

        void OnAutoConnectDeviceFound(DeviceInformation device)
        {
            if (!device.Pairing.IsPaired)
            {
                return;
            }

            string stylusNameLower = device.Name.ToLower();

            if (stylusNameLower.Contains("holosense"))
            {
#if WINDOWS_UWP
                _positionCalculator = new PositionCalculationHMUv1();
                _baseDeviceName = _positionCalculator.BaseDeviceName;
#endif
                ConnectToDevice(device);
            }
            else if (stylusNameLower.Contains("hmu_v_2"))
            {
#if WINDOWS_UWP
                initHMUv2(device);
#endif
            }
        }

        void OnAutoConnected(DeviceInformation device)
        {
            Debug.Log("Connected to: " + device.Name);
            OnDeviceFound -= OnAutoConnectDeviceFound;
            OnConnect -= OnAutoConnected;
        }

        // Refreshs the stylus data
        private void Update()
        {
            if (_onConnectToMainThread.Count > 0)
            {
                var device = _onConnectToMainThread.Dequeue();
                OnConnectToMainThread(device);
            }
            SetStylusData(_HMUStatus);
        }

        public void SaveStylusSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

            SettingsFile settings = new SettingsFile();

            if (!Directory.Exists(Application.persistentDataPath))
            {
                Directory.CreateDirectory(Application.persistentDataPath);
            }

            using (FileStream file = File.Create(Application.persistentDataPath + "/" + SETTINGS_FILE_NAME))
            {
                serializer.Serialize(file, settings);
            }
        }

        public void LoadStylusSettings()
        {
            if (!File.Exists(Application.persistentDataPath + "/" + SETTINGS_FILE_NAME))
            {
                SaveStylusSettings();
                LoadStylusSettings();
            }
            else
            {

                XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

                SettingsFile settings;

                using (FileStream file = File.Open(Application.persistentDataPath + "/" + SETTINGS_FILE_NAME, FileMode.Open))
                {
                    settings = serializer.Deserialize(file) as SettingsFile;
                }

                if (settings == null)
                    return;

            }
        }
    }

    [XmlRoot("StylusSettings")]
    public class SettingsFile
    {
        [XmlAttribute("BaseDeviceName")]
        public string BaseDeviceName = "";
    }
}
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
using HoloLight.HoloStylus.Examples.SceneCollection;
#if UNITY_EDITOR
using HoloLight.HoloStylus.Connection.Editor;
#elif WINDOWS_UWP
using Windows.Devices.Enumeration;
using HoloLight.DriverLibrary;
using HoloLight.DriverLibrary.DeviceDiscovery;
using HoloLight.DriverLibrary.Devices;
#endif
using UnityEngine;

namespace HoloLight.HoloStylus.Connection
{
    /// <summary>
    /// Helper class for OnConnect callback.
    /// </summary>
    public class OnConnectHelper : MonoBehaviour
    {
        /// <summary>
        /// Initialize and register helper class, if Device manager is in scene
        /// </summary>
        private void Start()
        {
            if (DeviceManager.Instance == null)
            {
                DestroyImmediate(gameObject);
            }
            else
            { 
                DeviceManager.Instance.OnConnect += OnConnect;
            }
        }

        /// <summary>
        /// Deregistration of this class at the device manager
        /// </summary>
        private void OnDestroy()
        {
            if (DeviceManager.Instance != null)
            {
                DeviceManager.Instance.OnConnect -= OnConnect;
            }
        }

        /// <summary>
        /// Event of successul connection.
        /// Load the scene collection.
        /// </summary>
        /// <param name="deviceInformation">Current connected device</param>
        public void OnConnect(DeviceInformation deviceInformation)
        {
            var sceneCollection = FindObjectOfType<SceneCollectionManager>();
            if (sceneCollection != null)
            {
                sceneCollection.LoadSceneCollection();
            }

            DestroyImmediate(this);
        }
    }
}

using HoloLight.HoloStylus.Connection;
using HoloLight.HoloStylus.Connection.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if WINDOWS_UWP
using HoloLight.DriverLibrary.Events;
using Windows.Devices.Enumeration;
using HoloLight.DriverLibrary;
using HoloLight.DriverLibrary.DeviceDiscovery;
using HoloLight.DriverLibrary.Devices;
using HoloLight.DriverLibrary.Data;
#endif

namespace HoloLight.HoloStylus.Examples.Preloading
{
    public class LoadNextSceneAfterConnect : MonoBehaviour
    {
        private bool _autoConnect
        {
            get
            {
                if (DeviceManager.Instance == null) { return false; }
                return DeviceManager.Instance.AutoConnect;
            }
        }

        void Start()
        {
            if (_autoConnect)
            {
                DeviceManager.Instance.OnConnect += OnConnect;
            }
            else
            {
                StartCoroutine(LoadNextScene());
            }
        }

        private void OnDestroy()
        {
            if (_autoConnect)
            {
                DeviceManager.Instance.OnConnect -= OnConnect;
            }
        }

        void OnConnect(DeviceInformation info)
        {
            StartCoroutine(LoadNextScene());
        }

        IEnumerator LoadNextScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            var nextIndex = currentScene.buildIndex + 1;

            var result = SceneManager.LoadSceneAsync(nextIndex, LoadSceneMode.Single);

            while (!result.isDone)
            {
                yield return null;
            }
        }
    }
}

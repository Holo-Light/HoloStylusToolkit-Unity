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
#if WINDOWS_UWP
using Windows.Devices.Enumeration;
#endif
#if UNITY_EDITOR
using HoloLight.HoloStylus.Connection.Editor;
#endif
using UnityEngine;

namespace HoloLight.HoloStylus.Connection
{
    /// <summary>
    /// TODO: Still in progress, methods are included for the future
    /// Will be responsible for all stylus commands, like vibrate, lights etc.
    /// </summary>
    public static class StylusCommands
    {
        public static void ConnectToHmu(DeviceInformation deviceInfo)
        {
#if WINDOWS_UWP
            var result = ConnectToHmuUwp(deviceInfo);
#endif
        }
#if WINDOWS_UWP
        private static async System.Threading.Tasks.Task ConnectToHmuUwp(DeviceInformation deviceInfo)
        {
            Debug.Log("ConnectToHmu called");
        }
#endif

        private static void Vibrate(float duration, float intensity)
        {
            Debug.Log("ConnectToStylus called");
        }

        private static void TurnLightIntensity(float intensity)
        {
            Debug.Log("TurnLightIntensity called");
        }

        private static void TurnStylusOn(bool turnOn)
        {
            Debug.Log("TurnStylusOn called");
        }

        private static void TurnHmuOn(bool turnOn)
        {
            Debug.Log("TurnHmuOn called");
        }

        private static void TurnTrackingOn(bool turnOn)
        {
            Debug.Log("TurnTrackingOn called");
        }

        private static void TurnEverythingOn(bool turnOn)
        {
            TurnStylusOn(turnOn);
            TurnHmuOn(turnOn);
            TurnTrackingOn(turnOn);
        }

        private static void Standby()
        {
            Debug.Log("Standby called");
        }

        private static void SaveToHmu(byte[] data, int length)
        {
            Debug.Log("ConnectToHmu called");

        }
    }
}
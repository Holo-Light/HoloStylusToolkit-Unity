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
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace HoloLight.HoloStylus.Configuration
{
    /// <summary>
    /// Configuration class
    /// </summary>
    public static class Config
    {
        // Config informations
        private static Dictionary<string, string> _config = null;

        /// <summary>
        /// Get setting information
        /// </summary>
        /// <param name="setting">setting name</param>
        /// <returns>setting information</returns>
        public static string GetSetting(string setting)
        {
            //if no config file is loaded, load it
            if (_config == null)
            {
                _config = new Dictionary<string, string>();
                string[] lines;

                //try to load config file
                try
                {
                    lines = File.ReadAllLines(Application.persistentDataPath + "/" + "config");
                    Debug.Log("sucessfully loaded config");
                }
                //logg error if no config is there and return
                catch
                {
                    Debug.LogError("failed to load config file");
                    return "";
                }

                //write configs in Dictonary
                foreach (var line in lines)
                {
                    var splitString = line.Split(' ');
                    _config.Add(splitString[0], splitString[1]);
                }
            }

            //get key if key exists, otherwise log an error
            if (_config.ContainsKey(setting))
            {
                return _config[setting];
            }

            Debug.LogError("No config called '" + setting + "' exists");
            return "";
        }
    }
}
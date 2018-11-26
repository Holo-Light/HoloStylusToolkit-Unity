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
using HoloLight.Utilities;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples
{
    /// <summary>
    /// Helper class for changing description and headline text
    /// </summary>
    public class ChangeMenuText : MonoBehaviour
    {
        [SerializeField, Tooltip("Scene title"), Multiline]
        private string _headline = "";
        [SerializeField, Tooltip("Scene description"), Multiline]
        private string _description = "";

        // Initilialization
        private void Start()
        {
            ChangeText(_description);
        }

        /// <summary>
        /// Changes the text to the description
        /// </summary>
        /// <param name="description"></param>
        public void ChangeText(string description)
        {
            DescriptionTextHandler.ChangeText(_headline, description);
        }
    }
}

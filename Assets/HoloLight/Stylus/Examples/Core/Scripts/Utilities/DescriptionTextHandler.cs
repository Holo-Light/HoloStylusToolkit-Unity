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

namespace HoloLight.Utilities
{
    /// <summary>
    /// Handles descriptions of all scenes in examples
    /// </summary>
    public class DescriptionTextHandler : MonoBehaviour
    {
        // Headline text field
        private static Text _headline;
        // Description text field
        private static Text _description;

        /// <summary>
        /// Initialization
        /// </summary>
        void Awake()
        {
            var texts = GetComponentsInChildren<Text>();
            _headline = texts[0];
            _description = texts[1];
        }

        /// <summary>
        /// Change the text of the scene information
        /// </summary>
        /// <param name="headline">The bold headline of the scene</param>
        /// <param name="description">The description text of the scene</param>
        public static void ChangeText(string headline, string description)
        {
            if (_headline == null || _description == null) { return; }
            _headline.text = headline;
            _description.text = description;
        }
    }
}

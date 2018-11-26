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
namespace HoloLight.HoloStylus.Configuration
{
    /// <summary>
    /// Applciation wide constants.
    /// </summary>
    public class Globals
    {
        /// <summary>
        /// Source ID of the action button
        /// </summary>
        public const int ACTION_BUTTON = 0;
        /// <summary>
        /// Source ID of the back button
        /// </summary>
        public const int BACK_BUTTON = 1;

        /// <summary>
        /// Button pressure if button is released
        /// </summary>
        public const float MIN_BUTTON_PRESSURE = 0f;
        /// <summary>
        /// Button pressure if button is maximal pressed
        /// </summary>
        public const float MAX_BUTTON_PRESSURE = 1.0f;

        /// <summary>
        /// Framerate for calculation of the thresholds
        /// </summary>
        public const int DEFAULT_FRAME_RATE = 3;

        /// <summary>
        /// Tooltip message for the hmu transform.
        /// </summary>
        public const string HMU_FIELD_MSG =
            "The transform of the HMU. E.g. if the HMU is on HoloLens, then the Camera is the HMU transform.";

        /// <summary>
        /// Console message, if hmu isn't set, no camera is in the scene and auto set is active
        /// </summary>
        public const string HMU_AUTOSET_MSG =
            "Can't autoset HMU transform, there is no Camera in the Scene.";

        /// <summary>
        /// Tooltip message for the show values for debugging
        /// </summary>
        public const string DEBUG_FIELD_MSG = "Debug the stylus data in the console.";
        /// <summary>
        /// Warning if no hmu is set and auto set is inactive
        /// </summary>
        public const string NO_HMU_SET_MSG = "No HMU transform is set, caculation is done by origin (Vector3.zero)";
        /// <summary>
        /// Error message if configuration asset is not set in the input manager
        /// </summary>
        public const string NO_CONFIG_ASSET_MSG = "No configuration asset is set, Inputmanager can't work without settings.";
    }
}
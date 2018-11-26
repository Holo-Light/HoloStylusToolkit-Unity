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
namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Returns transform data event callbacks from the stylus input manager.
    /// </summary>
    /// <param name="data">Displays rotation and position of the stylus.</param>
    public delegate void StylusTransformDataUpdateHandler(StylusTransformData data);

    /// <summary>
    /// Handler for stylus button up and down events.
    /// </summary>
    /// <param name="sourceID">The button id. Usually 0 means Actionbutton
    /// and 1 means the Backbutton.</param>
    public delegate void StylusButtonPressedHandler(int sourceID);

    /// <summary>
    /// Returns click pressed event callback from the stylus input manager.
    /// </summary>
    /// <param name="sourceID">The button id, normally 0 means first button 
    /// and 1 means the second one.</param>
    public delegate void StylusButtonHandler(int sourceID, float value);

    /// <summary>
    /// Focus event handler for stylus input manager.
    /// </summary>
    public delegate void StylusFocusHandler();

    /// <summary>
    /// Detection event handler for stylus input manager.
    /// </summary>
    public delegate void DeviceDetectionHandler();

    // Templates for future updates.
    //public delegate void StylusLostHandler();
    //public delegate void StylusDetectedHandler();

    //public delegate void HmuLostHandler();
    //public delegate void HmuDetectedHandler();
}
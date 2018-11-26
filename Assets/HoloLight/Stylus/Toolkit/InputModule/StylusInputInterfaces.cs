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
using UnityEngine.EventSystems;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Base interface of all input events.
    /// </summary>
    public interface IStylusInputHandler { }

    /// <summary>
    /// The base interface for all click events.
    /// </summary>
    public interface IStylusButtonHandler : IStylusInputHandler
    {
        /// <summary>
        /// Called if button started to be pressed
        /// </summary>
        /// <param name="sourceID">Button id</param>
        void OnStylusButtonDown(int sourceID);
        /// <summary>
        /// Called if button is released
        /// </summary>
        /// <param name="sourceID">Button id</param>
        void OnStylusButtonUp(int sourceID);
        /// <summary>
        /// Called if button is pressed 
        /// </summary>
        /// <param name="sourceID">Button id</param>
        /// <param name="value">Pressure value</param>
        void OnStylusButton(int sourceID, float value);
    }

    /// <summary>
    /// Action button was clicked.
    /// </summary>
    public interface IStylusActionClick : IStylusInputHandler
    {
        /// <summary>
        /// Called if action button is clicked
        /// </summary>
        void OnStylusActionClick();
    }

    /// <summary>
    /// Action button was double clicked.
    /// </summary>
    public interface IStylusActionDoubleClick : IStylusInputHandler
    {
        /// <summary>
        /// Called if action button is double clicked
        /// </summary>
        void OnStylusActionDoubleClick();
    }

    /// <summary>
    /// Back button was clicked.
    /// </summary>
    public interface IStylusBackClick : IStylusInputHandler
    {
        /// <summary>
        /// Called if back button is clicked
        /// </summary>
        void OnStylusBackClick();
    }

    /// <summary>
    /// Back button was double clicked.
    /// </summary>
    public interface IStylusBackDoubleClick : IStylusInputHandler
    {
        /// <summary>
        /// Called if back button is double clicked
        /// </summary>
        void OnStylusBackDoubleClick();
    }

    /// <summary>
    /// Action button was held.
    /// </summary>
    public interface IStylusActionHold : IStylusInputHandler
    {
        /// <summary>
        /// Called on holding the action button
        /// </summary>
        void OnStylusActionHold();
        /// <summary>
        /// Called on start holding the action button
        /// </summary>
        void OnStylusActionHoldStart();
        /// <summary>
        /// Called on stop holding the action button
        /// </summary>
        void OnStylusActionHoldEnd();
    }

    /// <summary>
    /// Back button was held.
    /// </summary>
    public interface IStylusBackHold : IStylusInputHandler
    {
        /// <summary>
        /// Called on holding the back button
        /// </summary>
        void OnStylusBackHold();
        /// <summary>
        /// Called on start holding the back button
        /// </summary>
        void OnStylusBackHoldStart();
        /// <summary>
        /// Called on stop holding the back button
        /// </summary>
        void OnStylusBackHoldEnd();
    }

    /// <summary>
    /// Focus events.
    /// </summary>
    public interface IStylusFocusHandler /*: IStylusInputHandler*/
    {
        /// <summary>
        /// Called if stylus enters a focusable object
        /// </summary>
        void OnStylusEnter();
        /// <summary>
        /// Called if stylus leaves a focusable object
        /// </summary>
        void OnStylusExit();
    }

    /// <summary>
    /// Stylus transform data updates for raw data.
    /// </summary>
    public interface IStylusMoveRawHandler : IStylusInputHandler
    {
        void OnStylusTransformDataRawUpdate(StylusTransformData data);
    }

    /// <summary>
    /// Stylus transform data updates for calculated data.
    /// </summary>
    public interface IStylusMoveHandler : IStylusInputHandler
    {
        void OnStylusTransformDataUpdate(StylusTransformData data);
    }
}
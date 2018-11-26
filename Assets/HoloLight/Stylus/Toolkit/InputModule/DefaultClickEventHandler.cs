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
using HoloLight.HoloStylus.Configuration;
using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Handles all click gestures, like hold, double and single click.
    /// Handles also the action and back click fuss.
    /// </summary>
    public class DefaultClickEventHandler
    {
        // Delegate type for click events
        public delegate void ClickEventHandler();

        /// <summary>
        /// Called if action button is double clicked
        /// </summary>
        public event ClickEventHandler OnActionDoubleClickEvent;
        /// <summary>
        /// Called if back button is double clicked
        /// </summary>
        public event ClickEventHandler OnBackDoubleClickEvent;
        /// <summary>
        /// Called if action button is held
        /// </summary>
        public event ClickEventHandler OnActionHoldEvent;
        /// <summary>
        /// Called if action button hold started
        /// </summary>
        public event ClickEventHandler OnActionHoldStartEvent;
        /// <summary>
        /// Called if action button hold stopped
        /// </summary>
        public event ClickEventHandler OnActionHoldEndEvent;
        /// <summary>
        /// Called if back button is held
        /// </summary>
        public event ClickEventHandler OnBackHoldEvent;
        /// <summary>
        /// Called if back button hold started
        /// </summary>
        public event ClickEventHandler OnBackHoldStartEvent;
        /// <summary>
        /// Called if back button hold stopped
        /// </summary>
        public event ClickEventHandler OnBackHoldEndEvent;
        /// <summary>
        /// Called if back button is clicked
        /// </summary>
        public event ClickEventHandler OnBackClickEvent;
        /// <summary>
        /// Called if action button is clicked
        /// </summary>
        public event ClickEventHandler OnActionClickEvent;

        // minimum check time for double click
        private float _doubleClickTime = 0.5f;
        // minimum hold time to call on hold events
        private float _holdClickTime = 0.5f;

        // saved last button click times
        private readonly float[] _lastButtonUpTime = new float[2] { -100.0f, -100.0f };
        // saved last button up times
        private readonly float[] _lastButtonDownTime = new float[2] { -100.0f, -100.0f };
        // flag for hold events raised
        private readonly bool[] _holdEventsRaised = new bool[2] { false, false };

        /// <summary>
        /// Called if some stylus button is pressed
        /// </summary>
        /// <param name="sourceId">Stylus button id</param>
        public void StylusButtonDown(int sourceId)
        {
            _lastButtonDownTime[sourceId] = Time.timeSinceLevelLoad;
            RaiseSingleClick(sourceId);
        }

        /// <summary>
        /// Raises on hold events
        /// </summary>
        /// <param name="sourceId">Stylus button id</param>
        /// <param name="value">Stylus button pressure value</param>
        public void RaiseStylusButtonHold(int sourceId, float value)
        {
            float timeHold = Time.timeSinceLevelLoad - _lastButtonDownTime[sourceId];

            if (timeHold > _holdClickTime)
            {
                switch (sourceId)
                {
                    case Globals.ACTION_BUTTON:
                        if (!_holdEventsRaised[sourceId])
                        {
                            _holdEventsRaised[sourceId] = true;
                            RaiseHandler(OnActionHoldStartEvent);
                        }
                        RaiseHandler(OnActionHoldEvent);
                        break;
                    case Globals.BACK_BUTTON:
                        if (!_holdEventsRaised[sourceId])
                        {
                            _holdEventsRaised[sourceId] = true;
                            RaiseHandler(OnBackHoldStartEvent);
                        }
                        RaiseHandler(OnBackHoldEvent);
                        break;
                }
            }
        }

        /// <summary>
        /// Raise released / up events
        /// </summary>
        /// <param name="sourceId">Stylus button id</param>
        public void RaiseStylusButtonUp(int sourceId)
        {
            _holdEventsRaised[sourceId] = false;

            float timeSinceLastClick = Time.timeSinceLevelLoad - _lastButtonUpTime[sourceId];

            float timeHold = Time.timeSinceLevelLoad - _lastButtonDownTime[sourceId];

            if (timeHold > _holdClickTime)
            {
                switch (sourceId)
                {
                    case Globals.ACTION_BUTTON:
                        RaiseHandler(OnActionHoldEndEvent);
                        break;
                    case Globals.BACK_BUTTON:
                        RaiseHandler(OnBackHoldEndEvent);
                        break;
                }
            }

            if (timeSinceLastClick < _doubleClickTime)
            {
                RaiseDoubleClick(sourceId);
            }

            _lastButtonUpTime[sourceId] = Time.timeSinceLevelLoad;
        }

        // Raise single click events
        private void RaiseSingleClick(int sourceId)
        {
            switch (sourceId)
            {
                case Globals.ACTION_BUTTON:
                    RaiseHandler(OnActionClickEvent);
                    break;
                case Globals.BACK_BUTTON:
                    RaiseHandler(OnBackClickEvent);
                    break;
            }
        }

        // Raise double click events
        private void RaiseDoubleClick(int sourceId)
        {
            switch (sourceId)
            {
                case Globals.ACTION_BUTTON:
                    RaiseHandler(OnActionDoubleClickEvent);
                    break;
                case Globals.BACK_BUTTON:
                    RaiseHandler(OnBackDoubleClickEvent);
                    break;
            }
        }

        // Raise click event handler and checks if it is not null
        private void RaiseHandler(ClickEventHandler handler)
        {
            if (handler != null)
            {
                handler();
            }
        }
    }
}
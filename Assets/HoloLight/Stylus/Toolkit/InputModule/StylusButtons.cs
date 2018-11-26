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
using HoloLight.HoloStylus.Configuration;
using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Handles button press
    /// </summary>
    public class StylusButtons
    {
        // Access to the input manager
        protected InputManager InputInstance
        {
            get
            {
                return InputManager.Instance;
            }
        }

        // Access to the thresholds
        protected Thresholds InputThresholds
        {
            get
            {
                return InputManager.Instance.Configuration.Thresholds;
            }
        }

        private List<ButtonThresholdCalculator> _buttonThresholdCalculators;

        private StylusButtonData _stylusActionButtonData = new StylusButtonData { Pressure = 0, SourceID = Globals.ACTION_BUTTON };
        public StylusButtonData StylusActionButtonData
        {
            get { return _stylusActionButtonData; }
            set
            {
                ExecuteStylusButtonHandler(_stylusActionButtonData, value);
                _stylusActionButtonData = value;
            }
        }

        private StylusButtonData _stylusBackButtonData = new StylusButtonData { Pressure = 0, SourceID = Globals.BACK_BUTTON };

        public StylusButtonData StylusBackButtonData
        {
            get { return _stylusBackButtonData; }
            set
            {
                ExecuteStylusButtonHandler(_stylusBackButtonData, value);
                _stylusBackButtonData = value;
            }
        }

        /// <summary>
        /// Changes of button pressure are handled here
        /// </summary>
        /// <param name="stylusOutput">Output pressure</param>
        /// <param name="value"> Input preassure</param>
        private void ExecuteStylusButtonHandler(StylusButtonData stylusOutput, StylusButtonData value)
        {
            CheckButtonThresholds(value);
            ButtonThresholdCalculator calculator = _buttonThresholdCalculators[value.SourceID];
            value.Pressure = calculator.AddValue(value).Pressure;
            RaiseButtonHandlers(stylusOutput, value);
        }

        internal void RaiseButtonHandlers(StylusButtonData stylusOutput, StylusButtonData value)
        {
            //before 0 now bigger then 0
            if (stylusOutput.Pressure == 0 && value.Pressure > 0)
            {
                InputInstance.RaiseStylusButtonDown(value);
            }

            if (stylusOutput.Pressure > 0 && value.Pressure == 0)
            {
                InputInstance.RaiseStylusButtonUp(value);
            }

            if (value.Pressure != 0)
            {
                InputInstance.RaiseStylusButtonChanged(value);
            }
        }

        private void CheckButtonThresholds(StylusButtonData value)
        {
            //checking if preassure is lower then dead zone
            if (value.Pressure < InputThresholds.ButtonDeadzone)
            {
                value.Pressure = Globals.MIN_BUTTON_PRESSURE;
            }

            //checking if preassure is higher then gravity
            if (value.Pressure > InputThresholds.ButtonGravity)
            {
                value.Pressure = Globals.MAX_BUTTON_PRESSURE;
            }

            //smooth movement according to threshold
            if (_buttonThresholdCalculators == null)
            {
                _buttonThresholdCalculators = new List<ButtonThresholdCalculator>(2)
                {
                    new ButtonThresholdCalculator(Globals.DEFAULT_FRAME_RATE, InputThresholds),
                    new ButtonThresholdCalculator(Globals.DEFAULT_FRAME_RATE, InputThresholds)
                };
            }
        }
    }
}
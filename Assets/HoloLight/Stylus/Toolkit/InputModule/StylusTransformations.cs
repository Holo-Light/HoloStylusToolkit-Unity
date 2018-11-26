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

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Handles Transformation updates of InputManager.
    /// </summary>
    public class StylusTransformations
    {
        // Access to the input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }
        private readonly TransformThresholdCalculator _thresholdCalculator = new TransformThresholdCalculator(Globals.DEFAULT_FRAME_RATE, InputManager.Instance.Configuration.Thresholds);

        private StylusTransformData _stylusTransformRaw = new StylusTransformData();
        /// <summary>
        /// Position, rotation and acceleration of the stylus. (Calculated to world space in relation to the HMU).
        /// If no HMU transfrom is set, then no recalculation will be done.
        /// </summary>
        public StylusTransformData StylusTransformRaw
        {
            get
            {
                return _stylusTransformRaw;
            }
            set
            {
                
                if (_input.HMUTransform == null)
                {
                    _stylusTransformBeforeCalibrated = value;
                    value.Position += CalibrationPreferences.CalibrationData.Offset;
                    _stylusTransformRaw = value;
                    _stylusTransform = value;
                }
                else
                {
                    _stylusTransformBeforeCalibrated = RecalculateValues(value);
                    value.Position += CalibrationPreferences.CalibrationData.Offset;
                    var data = RecalculateValues(value);
                    _stylusTransformRaw = data;
                    this.StylusTransform = data;
                }

                _input.RaiseStylusTransformRawChanged(_stylusTransformRaw);
            }
        }

        private StylusTransformData _stylusTransformBeforeCalibrated;
        public StylusTransformData StylusTransformBeforeCalibrated
        {
            get
            {
                return _stylusTransformBeforeCalibrated;
            }
        }

        private StylusTransformData _stylusTransform = new StylusTransformData();
        /// <summary>
        /// Position, rotation and acceleration of the stylus. (Calculated to world space in relation to the HMU).
        /// If no HMU transfrom is set, then no recalculation will be done.
        /// Value will only be updated when the change exceeds the threshold
        /// </summary>
        public StylusTransformData StylusTransform
        {
            get
            {
                return _stylusTransform;
            }
            set
            {
                _stylusTransform = _thresholdCalculator.AddValue(value);
                _input.RaiseStylusTransformChanged(_stylusTransform);
            }
        }

        public StylusTransformations() { }

        private StylusTransformData RecalculateValues(StylusTransformData data)
        {
            var recalculatedData = new StylusTransformData
            {
                Position = _input.HMUTransform.TransformPoint(data.Position),
                Rotation = _input.HMUTransform.rotation * data.Rotation,
                Acceleration = data.Rotation * data.Acceleration
            };

            return recalculatedData;
        }
    }
}
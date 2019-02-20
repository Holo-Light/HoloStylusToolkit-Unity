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
    /// Handles Transformation updates of InputManager.
    /// </summary>
    ///
    public class StylusTransformations
    {
        private Queue<GameObject> _oldTransformsQueue= new Queue<GameObject>();

        private int _queueCounter = 0;
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

        public StylusTransformations()
        {
        }

        private StylusTransformData RecalculateValues(StylusTransformData data)
        {
            var temporaryTransform=new GameObject();
            MonoBehaviour.DontDestroyOnLoad(temporaryTransform);
            temporaryTransform.transform.position = _input.HMUTransform.position;
            temporaryTransform.transform.rotation = _input.HMUTransform.rotation;
            _oldTransformsQueue.Enqueue(temporaryTransform);
            if (_queueCounter < 5)
            {
                _queueCounter++;
                return new StylusTransformData
                {
                    Position = _input.HMUTransform.TransformPoint(data.Position),
                    Rotation = _input.HMUTransform.transform.rotation * data.Rotation,
                    Acceleration = data.Rotation * data.Acceleration
                };
            }

            var oldHMU = _oldTransformsQueue.Dequeue();

            var recalculatedData = new StylusTransformData
            {
                Position = oldHMU.transform.TransformPoint(data.Position),
                Rotation = oldHMU.transform.rotation * data.Rotation,
                Acceleration = data.Rotation * data.Acceleration
            };
            MonoBehaviour.Destroy(oldHMU);

            return recalculatedData;
        }
    }
}
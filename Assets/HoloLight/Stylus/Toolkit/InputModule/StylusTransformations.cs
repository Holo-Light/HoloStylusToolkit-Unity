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
        internal class TransformQueueElement
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        private Queue<TransformQueueElement> _oldTransformsQueue= new Queue<TransformQueueElement>();
        private Transform _oldTransform
        {
            get
            {
                if(_oldTransformInternal == null)
                {
                    var oldtransform = new GameObject("Last Stylus transform position helper");
                    _oldTransformInternal = oldtransform.transform;
                }

                return _oldTransformInternal;
            }
        }

        private Transform _oldTransformInternal;

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
            _oldTransform.position = _input.HMUTransform.position;
            _oldTransform.rotation = _input.HMUTransform.rotation;
            _oldTransformsQueue.Enqueue(new TransformQueueElement() { Position = _oldTransform.position, Rotation = _oldTransform.rotation });

            if (_queueCounter < InputManager.RECALCULATED_STYLUS_TRANSFORMATION_FRAME)
            {
                _queueCounter++;
                return new StylusTransformData
                {
                    Position = _input.HMUTransform.TransformPoint(data.Position),
                    Rotation = _input.HMUTransform.transform.rotation * data.Rotation,
                    Acceleration = data.Rotation * data.Acceleration
                };
            }

            var old = _oldTransformsQueue.Dequeue();

            _oldTransform.position = old.Position;
            _oldTransform.rotation = old.Rotation;

            var recalculatedData = new StylusTransformData
            {
                Position = _oldTransform.TransformPoint(data.Position),
                Rotation = _oldTransform.rotation * data.Rotation,
                Acceleration = data.Rotation * data.Acceleration
            };

            return recalculatedData;
        }
    }
}
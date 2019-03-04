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
using HoloLight.HoloStylus.InputModule;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.Calibration
{
    public class CalibrationHMU : MonoBehaviour, IStylusBackClick
    {
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        public void Reset()
        {
            _input.HMUTransform.localPosition = Vector3.zero;
            _input.HMUTransform.localRotation = Quaternion.identity;
        }

        public void Rotate(StylusAxis axis, float value)
        {
            switch (axis)
            {
                case StylusAxis.X:
                    _input.HMUTransform.Rotate(Vector3.right, value);
                    break;
                case StylusAxis.Y:
                    _input.HMUTransform.Rotate(Vector3.up, value);
                    break;
                case StylusAxis.Z:
                    _input.HMUTransform.Rotate(Vector3.forward, value);
                    break;
            }

            CalibrationManager.SaveHMUTransform(_input.HMUTransform);
        }

        public void Move(StylusAxis axis, float value)
        {
            switch (axis)
            {
                case StylusAxis.X:
                    _input.HMUTransform.localPosition += Vector3.right * value;
                    break;
                case StylusAxis.Y:
                    _input.HMUTransform.localPosition += Vector3.up * value;
                    break;
                case StylusAxis.Z:
                    _input.HMUTransform.localPosition += Vector3.forward * value;
                    break;
            }

            CalibrationManager.SaveHMUTransform(_input.HMUTransform);
        }

        public void OnStylusBackClick()
        {
        }

        private void Start()
        {
            if (_input == null) { return; }
            _input.GlobalListeners.Add(gameObject);
        }

        private void OnDestroy()
        {
            if (_input == null) { return; }
            _input.GlobalListeners.Remove(gameObject);
        }
    }
}

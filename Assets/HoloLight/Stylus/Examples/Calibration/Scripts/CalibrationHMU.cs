using HoloLight.HoloStylus.InputModule;
using System.Collections;
using System.Collections.Generic;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.Calibration
{
    public class CalibrationHMUButton : MonoBehaviour
    {
        public CalibrationHMU CalibrationHMU;
        public StylusAxis HMUAxis = StylusAxis.X;
        public float Value;
        private Transform _camTransform;
        public bool ActiveRotation = true;

        public void Rotate()
        {
            CalibrationHMU.Rotate(HMUAxis, Value);
        }

        public void Move()
        {
            CalibrationHMU.Move(HMUAxis, Value);
        }

        private void Start()
        {
            _camTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (_camTransform != null && ActiveRotation)
            {
                transform.forward = transform.position - _camTransform.transform.position;
            }
        }
    }
}

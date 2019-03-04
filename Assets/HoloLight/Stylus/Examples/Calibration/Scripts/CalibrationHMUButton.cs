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

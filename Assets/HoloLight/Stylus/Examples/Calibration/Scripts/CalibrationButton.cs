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
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.Calibration
{
    /// <summary>
    /// Simple button solution for fast calibration. This is only the first idea.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CalibrationButton : MonoBehaviour
    {
        /// <summary>
        /// Axis type
        /// </summary>
        public StylusAxis Axis;

        /// <summary>
        /// Value to change
        /// </summary>
        public float Value;

        private Transform _camTransform;

        /// <summary>
        /// Registers the click method
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => CalibrationManager.AddValueToAxis(Axis, Value));
            _camTransform = Camera.main.transform;
        }

        /// <summary>
        /// Billboards the button to camera.
        /// </summary>
        private void Update()
        {
            if (_camTransform != null)
            {
                transform.forward = transform.position - _camTransform.transform.position;
            }
        }
    }
}

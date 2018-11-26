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

        /// <summary>
        /// Registers the click method
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => CalibrationManager.AddValueToAxis(Axis, Value));
        }

        /// <summary>
        /// Billboards the button to camera.
        /// </summary>
        private void Update()
        {
            if (Camera.main != null)
            {
                transform.forward = transform.position - Camera.main.transform.position;
            }
        }
    }
}

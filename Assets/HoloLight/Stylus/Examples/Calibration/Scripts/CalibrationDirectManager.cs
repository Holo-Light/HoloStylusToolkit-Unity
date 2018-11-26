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
using HoloLight.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.Calibration
{
    /// <summary>
    /// Calibration class with point based calibration
    /// </summary>
    public class CalibrationDirectManager : MonoBehaviour, IStylusActionClick, IStylusBackClick
    {
        [SerializeField, Tooltip("Range for spawned objects"), Range(0.1f, 1.0f)]
        private float _range = 0.1f;
        [SerializeField, Tooltip("Minimum range")]
        private float _minRange = 0.3f;
        [SerializeField, Tooltip("Count of the calibration steps")]
        private int _calibrationSteps = 10;

        // Temp size of the sphere
        private readonly float _sphereSize = 0.02f;

        // Current step transform
        private Transform _currentTarget;
        // All set calibrated positions
        private readonly List<Vector3> _calibratedPositions = new List<Vector3>();
        // Helper for calculating the average / median
        private Vector3 _calibratedMax = Vector3.zero;
        // Helper for calculating the average / median
        private Vector3 _calibratedMin = Vector3.positiveInfinity;

        [SerializeField, Multiline, Tooltip("Tooltip in the beginning")]
        private string _textAtStart = "";
        [SerializeField, Multiline, Tooltip("Tooltip while calibrating")]
        private string _textWhileCalibration = "";

        // Flag of calibration state
        private bool _calibrationStarted = false;

        [SerializeField, Tooltip("Scene title"), Multiline]
        private string _headline = "";

        // Set headline and description of the scene collection
        private void SetHeadline()
        {
            
        }

        // Access for the input manager instance
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        // Main camera / camera with HMU attached
        private Transform _camTransform;

        /// <summary>
        /// Sets the calibrated position, if calibration is started
        /// </summary>
        public void OnStylusActionClick()
        {
            if (!_calibrationStarted)
            {
                return;
            }

            SetCalibratedPosition();
            if (_calibratedPositions.Count >= _calibrationSteps)
            {
                CalculateOffset();
            }
            else
            {
                NextPosition();
            }
        }

        // Creates the next position
        private void NextPosition()
        {
            _currentTarget = (GameObject.CreatePrimitive(PrimitiveType.Sphere)).transform;

            var direction = new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z);
            var spawnPosition = _camTransform.position + direction * (_range + _minRange) + Random.insideUnitSphere * _range;

            _currentTarget.position = spawnPosition;
            _currentTarget.localScale = Vector3.one * _sphereSize;
        }

        // Sets the calibrated position and destroys the current sphere
        private void SetCalibratedPosition()
        {
            var hmu = _input.HMUTransform;
            if (_currentTarget == null || hmu == null)
            {
                return;
            }
            var localCurrent = hmu.InverseTransformPoint(_currentTarget.position);
            var stylusTransformData = hmu.InverseTransformPoint(_input.StylusTransformBeforeCalibrated.Position);
            var delta = localCurrent - stylusTransformData;
            Debug.Log(delta.ToString());
            _calibratedPositions.Add(delta);
            if(delta.sqrMagnitude > _calibratedMax.sqrMagnitude)
            {
                _calibratedMax = delta;
            }

            if(delta.sqrMagnitude < _calibratedMin.sqrMagnitude)
            {
                _calibratedMin = delta;
            }

            if (_currentTarget != null)
            {
                Destroy(_currentTarget.gameObject);
            }

        }

        // Calculates the offset of all values
        private void CalculateOffset()
        {
            _calibratedPositions.Remove(_calibratedMax);
            _calibratedPositions.Remove(_calibratedMin);

            Vector3 averageOffset = Vector3.zero;
            foreach(var position in _calibratedPositions)
            {
                averageOffset += position;
            }

            averageOffset /= _calibratedPositions.Count;

            CalibrationManager.SetOffset(averageOffset);

            DescriptionTextHandler.ChangeText(_headline, _textAtStart);

            _calibrationStarted = false;

        }

        /// <summary>
        /// Resets to the entry state
        /// </summary>
        public void OnStylusBackClick()
        {
            if (_currentTarget != null)
            {
                Destroy(_currentTarget.gameObject);
            }

            if (_calibrationStarted)
            {
                CalibrationManager.SetOffset(Vector3.zero);
                _calibrationStarted = false;
                DescriptionTextHandler.ChangeText(_headline, _textAtStart);
                return;
            }

            _calibratedPositions.Clear();
            DescriptionTextHandler.ChangeText(_headline, _textWhileCalibration);
            _calibrationStarted = true;

            NextPosition();
        }

        /// <summary>
        /// Initializing and registering
        /// </summary>
        private void OnEnable()
        {
            if (_input)
            {
                _input.GlobalListeners.Add(gameObject);
            }

            var cam = Camera.main;
            if (cam == null)
            {
                cam = FindObjectOfType<Camera>();
                if (cam == null)
                {
                    enabled = false;
                    return;
                }
            }

            _camTransform = cam.transform;
            _calibratedPositions.Clear();
            DescriptionTextHandler.ChangeText(_headline, _textAtStart);
            _calibrationStarted = false;
        }

        /// <summary>
        /// Deregister the input manager.
        /// </summary>
        private void OnDisable()
        {
            if (_input)
            {
                _input.GlobalListeners.Remove(gameObject);
            }
        }
    }
}

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

namespace HoloLight.HoloStylus.Emulation
{
    /// <summary>
    /// Keyboard emulation of the stylus.
    /// </summary>
    public class MouseToStylus : InterfaceToStylus
    {
        /// <summary>
        /// Name of the input axis (the local z axis)
        /// </summary>
        private const string DEPTH_AXIS = "Vertical";

#if !UNITY_EDITOR
        /// <summary>
        /// Destroys this gameobject on build.
        /// </summary>
        private bool _destroyOnBuild = true;
#endif

        // Emulated transform data
        private StylusTransformData _currentData = new StylusTransformData();
        // Emulated action button data
        private StylusButtonData _currentButton0 = new StylusButtonData { SourceID = 0 };
        // Emulated back button data
        private StylusButtonData _currentButton1 = new StylusButtonData { SourceID = 1 };
        // Current depth of the tip
        private float _currentDepth;
        // Speed of changing the depth.
        [SerializeField, Tooltip("Speed of changing the depth.")]
        private float _depthSpeed = 1;
        // Used camera for local transform data.
        private Camera _camera;

        /// <summary>
        /// Check if axis is set
        /// </summary>
        /// <param name="axisName">Axis to check</param>
        /// <returns></returns>
        bool IsAxisAvailable(string axisName)
        {
            try
            {
                Input.GetAxis(axisName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if button is set
        /// </summary>
        /// <param name="btnName">Button to check</param>
        /// <returns></returns>
        bool IsButtonAvailable(string btnName)
        {
            try
            {
                Input.GetButton(btnName);
                return true;
            }
            catch
            {
                return false;
            }
        }


#if !UNITY_EDITOR
        // Initializing, if destroy on build is set, mouse emulation will be destroyed
        private void Awake()
        {
            if(_destroyOnBuild) 
            { 
                Destroy(gameObject, 0.05f);
                return;
            }
        }
#endif

        // Set up the emulation
        private void Start()
        {
            if (InputInstance == null)
            {
                return;
            }

            // Find the camera
            _camera = Camera.main;
            if(_camera == null)
            {
                _camera = FindObjectOfType<Camera>();
                if(_camera == null)
                {
                    enabled = false;
                }
            }
            
            // Check the axis
            bool allAvailable = IsAxisAvailable(DEPTH_AXIS);

            if (!allAvailable)
            {
                enabled = false;
            }

            _currentData.Position = Vector3.forward * 2;
            InputInstance.StylusTransformRaw = _currentData;

            _currentDepth = Vector3.Distance(_currentData.Position, _camera.transform.position);
        }

        // KeybAddoard emulation is handled here.
        void FixedUpdate()
        {
            if (InputInstance == null)
            {
                return;
            }

            //Transform data input
            _currentDepth += Time.fixedDeltaTime * Input.GetAxis(DEPTH_AXIS) * _depthSpeed;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            //var tempPos = ray.GetPoint(_currentDepth);
            //ray = new Ray(InputInstance.HMUTransform.position, InputInstance.HMUTransform.position + tempPos);
            var currentPosition = ray.GetPoint(_currentDepth);

            //Click button input
            float button0 = Input.GetMouseButton(0) ? 1 : 0;
            float button1 = Input.GetMouseButton(1) ? 1 : 0;

            //Click button handlers
            _currentButton1.Pressure = button1;
            _currentButton0.Pressure = button0;

            InputInstance.StylusActionButtonData = _currentButton0;
            InputInstance.StylusBackButtonData = _currentButton1;

            //Transform update handlers
            Quaternion currentRotation = InputInstance.StylusTransformRaw.Rotation;

            //var toLocal = InverseToLocal();

            _currentData.Position = InputInstance.HMUTransform.InverseTransformPoint(currentPosition);
            _currentData.Rotation = currentRotation;

            InputInstance.StylusTransformRaw = _currentData;
            
        }

        // Helper method to get the local space of the hmu transform.
        Vector3 InverseToLocal()
        {
            Vector3 toLocal = Vector3.zero;

            if (InputInstance.HMUTransform != null)
            {
                toLocal = InputInstance.HMUTransform.InverseTransformPoint(InputInstance.StylusTransformRaw.Position);
            }
            else
            {
                toLocal = InputInstance.StylusTransformRaw.Position;
            }
            return toLocal;
        }
    }
}
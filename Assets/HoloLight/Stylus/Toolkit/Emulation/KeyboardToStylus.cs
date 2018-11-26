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
    public class KeyboardToStylus : InterfaceToStylus
    {
        public const string HORIZONTAL_BTN = "Horizontal";
        public const string VERTICAL_BTN = "Vertical";
        public const string LATERAL_BTN = "Lateral";
        public const string ROTATION_LOCK_BTN = "Rotation lock";

#if !UNITY_EDITOR
        /// <summary>
        /// Destroys this gameobject on build.
        /// </summary>
        private bool _destroyOnBuild = true;
#endif

        /// <summary>
        /// The emulated moving speed of the stylus.
        /// </summary>
        [Tooltip("The emulated moving speed of the stylus.")]
        [SerializeField]
        private float _speed = 2;

        /// <summary>
        /// The emulated rotation speed of the stylus.
        /// </summary>
        [Tooltip("The emulated rotation speed of the stylus.")]
        [SerializeField]
        private float _rotationSpeed = 80;

        private StylusTransformData _currentData = new StylusTransformData();
        private StylusButtonData _currentButton0 = new StylusButtonData { SourceID = 0 };
        private StylusButtonData _currentButton1 = new StylusButtonData { SourceID = 1 };

        // Check if axis is avaible, not the best solution, but the Unity input system would throw errors instead
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

        // Check if button is avaible, not the best solution, but the Unity input system would throw errors instead
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
        // Destroys the gameobject, if destroy on build is true and the project is built
        private void Awake()
        {
            if(!_destroyOnBuild)
            {
                return;
            }
            Destroy(gameObject, 0.05f);
            return;
        }
#endif

        // Initialize and checking axis and buttons
        private void Start()
        {
            if (InputInstance == null)
            {
                return;
            }

            bool hasHorizontal = IsAxisAvailable(HORIZONTAL_BTN);
            bool hasVertical = IsAxisAvailable(VERTICAL_BTN);
            bool hasLateral = IsAxisAvailable(LATERAL_BTN);
            bool hasRotationLock = IsButtonAvailable(ROTATION_LOCK_BTN);

            bool allAvailable = hasHorizontal && hasVertical && hasLateral && hasRotationLock;

            if (!allAvailable)
            {
                enabled = false;
            }

            _currentData.Position = Vector3.forward * 2;
            InputInstance.StylusTransformRaw = _currentData;
        }

        // KeybAddoard emulation is handled here.
        void Update()
        {
            if (InputInstance == null)
            {
                return;
            }

            //Transform data input
            float horizontal = Input.GetAxis(HORIZONTAL_BTN);
            float vertical = Input.GetAxis(VERTICAL_BTN);
            float lateral = Input.GetAxis(LATERAL_BTN);

            bool rotationActive = Input.GetButton(ROTATION_LOCK_BTN);

            //Click button input
            float button0 = Input.GetMouseButton(0) ? 1 : 0;
            float button1 = Input.GetMouseButton(1) ? 1 : 0;

            //Click button handlers
            _currentButton1.Pressure = button1;
            _currentButton0.Pressure = button0;

            InputInstance.StylusActionButtonData = _currentButton0;
            InputInstance.StylusBackButtonData = _currentButton1;

            //Transform update handlers
            Vector3 currentPosition = Vector3.zero;
            Quaternion currentRotation = InputInstance.StylusTransformRaw.Rotation;

            if (!rotationActive)
            {
                currentPosition = new Vector3(horizontal, lateral, vertical) * Time.deltaTime * _speed;
            }
            else
            {
                var currentEuler = currentRotation.eulerAngles + new Vector3(horizontal, lateral, vertical) * Time.deltaTime * _rotationSpeed;
                currentRotation = Quaternion.Euler(currentEuler);
            }

            _currentData.Position = InputInstance.HMUTransform.InverseTransformPoint(currentPosition);

            InputInstance.StylusTransformRaw = _currentData;
        }
    }
}
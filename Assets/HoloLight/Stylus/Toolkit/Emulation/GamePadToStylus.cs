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
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Emulation
{
    /// <summary>
    /// Gamepad emulation of the stylus. Currently not finished
    /// </summary>
    public class GamePadToStylus : InterfaceToStylus
    {
        /// <summary>
        /// The emulated moving speed of the stylus.
        /// </summary>
        [SerializeField, Tooltip("The emulated moving speed of the stylus.")]
        private float _moveSpeed = 1.5f;

        /// <summary>
        /// The emulated rotation speed of the stylus.
        /// </summary>
        [SerializeField, Tooltip("The emulated rotation speed of the stylus.")]
        private float _rotationSpeed = 80;

        // Last position of the transform which emulates the stylus.
        private Vector3 _lastPosition;
        // Last rotation of the transform which emulates the stylus.
        private Quaternion _lastRotation;
        // Button pressure of the action button
        private StylusButtonData _currentButton1 = new StylusButtonData { SourceID = 1 };
        // Button pressure of the back button
        private StylusButtonData _currentButton0 = new StylusButtonData { SourceID = 0 };
        
        // Resets the transform data for the emulated stylus.
        private void Start()
        {
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
        }

        // Gamepad emulation is handled here.
        void Update()

        {
            if (InputInstance == null)
                return;

            #region Click button input
            float button0 = Input.GetKeyDown("joystick button 0") ? 1 : 0;
            float button1 = Input.GetKeyDown("joystick button 1") ? 1 : 0;
            #endregion


            #region Click button handlers
            _currentButton1.Pressure = button1;
            _currentButton0.Pressure = button0;

            InputInstance.StylusActionButtonData = _currentButton0;
            InputInstance.StylusBackButtonData = _currentButton1;
            #endregion

            #region Transform data input
            float horizontal = Input.GetAxis("Left Stick X");
            float vertical = Input.GetAxis("Left Stick Y");
            float lateral = Input.GetAxis("Right Stick Y");

            bool rotationActive = false;
            //if (Input.GetButton("Rotation lock"))
            //{
            //    rotationActive = true;
            //}
            #endregion

            #region Transform update handlers
            if (!rotationActive)
            {
                transform.position += new Vector3(horizontal, vertical, lateral) * Time.deltaTime * _moveSpeed;
            }
            else
            {
                transform.Rotate(Vector3.up, -horizontal * Time.deltaTime * _rotationSpeed, Space.World);
                transform.Rotate(Vector3.right, vertical * Time.deltaTime * _rotationSpeed, Space.World);
                transform.Rotate(Vector3.forward, -lateral * Time.deltaTime * _rotationSpeed, Space.World);
            }

            if (_lastRotation != transform.rotation || _lastPosition != transform.position)
            {
                var data = new StylusTransformData
                {
                    Position = transform.position,
                    Rotation = transform.rotation
                }; 

                InputInstance.StylusTransformRaw = data;
            }

            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
            #endregion
        }
    }
}
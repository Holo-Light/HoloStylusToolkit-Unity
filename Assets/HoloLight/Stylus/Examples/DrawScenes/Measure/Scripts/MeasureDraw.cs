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
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Class for drawing if button clicked.
    /// </summary>
    public class MeasureDraw : MonoBehaviour, IStylusActionClick, IStylusBackClick
    {
        // Access to the input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        [SerializeField, Tooltip("Current draw manager")]
        private BaseDrawManager _drawManager;

        /// <summary>
        /// Attached draw manager
        /// </summary>
        public BaseDrawManager DrawManager
        {
            get { return _drawManager; }
            set { _drawManager = value; }
        }

        /// <summary>
        /// Called on action click, use base draw mechanic
        /// </summary>
        public void OnStylusActionClick()
        {
            DrawManager.BaseDrawObject();
        }

        /// <summary>
        /// Called on back click, use for stop draw mechanic
        /// </summary>
        public void OnStylusBackClick()
        {
            DrawManager.SetDrawing(false);
            DrawManager.Undo();
            DrawManager.SetDrawing(true);
        }

        private void Update()
        {
            DrawManager.CurrentPosition = _input.StylusTransformRaw.Position;
        }

        // initializing and register in input manager
        private void OnEnable()
        {
            if (_input == null)
            {
                enabled = false;
                return;
            }

            _input.GlobalListeners.Add(gameObject);
            if (DrawManager == null)
            {
                DrawManager = GetComponent<BaseDrawManager>();
            }
            DrawManager.SetDrawing(true);
        }

        // deinitializing and deregister in input manager
        private void OnDisable()
        {
            if (_input == null)
            {
                return;
            }

            _input.GlobalListeners.Remove(gameObject);
        }
    }
}
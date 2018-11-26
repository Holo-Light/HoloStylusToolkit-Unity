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
using HoloLight.HoloStylus.Configuration;
using HoloLight.HoloStylus.InputModule;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Class for drawing if button is holded.
    /// </summary>
    public class DrawOnHold : MonoBehaviour, IStylusBackClick, IStylusMoveRawHandler, IStylusActionHold
    {
        // Access for input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        [SerializeField, Tooltip("Current draw Manager")]
        private BaseDrawManager _drawManager;

        /// <summary>
        /// Attached draw manager
        /// </summary>
        public BaseDrawManager DrawManager
        {
            get { return _drawManager; }
            set { _drawManager = value; }
        }

        public void OnStylusBackClick()
        {
            DrawManager.SetDrawing(false);
        }

        public void OnStylusTransformDataRawUpdate(StylusTransformData data)
        {
            if(DrawManager == null)
            {
                return;
            }

            DrawManager.CurrentPosition = data.Position;
        }

        private void OnEnable()
        {
            if (_input == null)
            {
                return;
            }

            _input.GlobalListeners.Add(gameObject);
            if(DrawManager == null)
            {
                DrawManager = GetComponent<BaseDrawManager>();
            }
        }

        private void OnDisable()
        {
            if (_input == null)
            {
                return;
            }

            _input.GlobalListeners.Remove(gameObject);
        }

        public void OnStylusActionHold()
        {
            DrawManager.BaseDrawObject();
        }

        public void OnStylusActionHoldStart()
        {
        }

        public void OnStylusActionHoldEnd()
        {
            DrawManager.SetDrawing(false);
        }
    }
}
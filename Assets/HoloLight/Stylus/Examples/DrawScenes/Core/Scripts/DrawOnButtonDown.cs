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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Class for drawing if button holded.
    /// </summary>
    [RequireComponent(typeof(BaseDrawManager))]
    public class DrawOnButtonDown : MonoBehaviour, IStylusButtonHandler, IStylusMoveRawHandler
    {
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// Base draw manager
        /// </summary>
        [SerializeField]
        private BaseDrawManager _drawManager;

        /// <summary>
        /// Base draw manager property
        /// </summary>
        public BaseDrawManager DrawManager
        {
            get { return _drawManager; }
            set { _drawManager = value; }
        }

        /// <summary>
        /// Registered method for stylus position update.
        /// </summary>
        /// <param name="data">stylus transform data</param>
        public void OnStylusTransformDataRawUpdate(StylusTransformData data)
        {
            DrawManager.CurrentPosition = data.Position;
        }

        /// <summary>
        /// Enable calls registration of the stylus interfaces
        /// </summary>
        private void OnEnable()
        {
            if (_input == null)
            {
                return;
            }

            _input.GlobalListeners.Add(gameObject);
            if (DrawManager == null)
            {
                DrawManager = GetComponent<BaseDrawManager>();
            }
        }

        /// <summary>
        /// Disable calls deregistration of the stylus interfaces
        /// </summary>
        private void OnDisable()
        {
            if (_input == null)
            {
                return;
            }

            _input.GlobalListeners.Remove(gameObject);
        }

        /// <summary>
        /// Interface method for click event.
        /// </summary>
        /// <param name="sourceID">source id of the button</param>
        /// <param name="value">pressure of button</param>
        public void OnStylusButton(int sourceID, float value)
        {
            if (sourceID == Globals.ACTION_BUTTON)
            {
                DrawManager.DrawObject();
            }
        }

        /// <summary>
        /// Interface method for button down.
        /// </summary>
        /// <param name="sourceID">source id of the button</param>
        public void OnStylusButtonDown(int sourceID)
        {
            if (sourceID == Globals.ACTION_BUTTON)
            {
                DrawManager.SetDrawing(true);
            }

            if(sourceID == Globals.BACK_BUTTON)
            {
                DrawManager.Undo();
            }
        }

        /// <summary>
        /// Interface method for button up.
        /// </summary>
        /// <param name="sourceID">source id of the button</param>
        public void OnStylusButtonUp(int sourceID)
        {
            if (sourceID == Globals.ACTION_BUTTON)
            {
                DrawManager.SetDrawing(false);
            }
        }
    }
}

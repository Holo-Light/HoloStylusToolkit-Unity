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

namespace HoloLight.HoloStylus.Examples.SceneCollection
{
    /// <summary>
    /// Class for following stylus transform data.
    /// </summary>
    public class StylusClickDisplay : MonoBehaviour, IStylusActionClick, IStylusBackClick
    {
        /// <summary>
        /// Shortcut and readonly variable for Inputmanager.Instance
        /// </summary>
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        [SerializeField, Tooltip("Feedback for action click")]
        private GameObject _actionButtonFeedback;
        [SerializeField, Tooltip("Feedback for back click")]
        private GameObject _backButtonFeedback;
        [SerializeField, Tooltip("Feadback life time")]
        private float _feedbackLifeTime = 1.5f;


        // initializing and register in input manager
        private void OnEnable()
        {
            if (_input == null || _actionButtonFeedback == null || _backButtonFeedback == null)
            {
                enabled = false;
                return;
            }

            _input.GlobalListeners.Add(gameObject);
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

        /// <summary>
        /// Show feedback on stylus action click
        /// </summary>
        public void OnStylusActionClick()
        {
            var actionButtonFeedback = Instantiate(_actionButtonFeedback, _input.StylusTransformRaw.Position, Camera.main.transform.rotation);
            Destroy(actionButtonFeedback, _feedbackLifeTime);
        }

        /// <summary>
        /// Show feedback on stylus back click
        /// </summary>
        public void OnStylusBackClick()
        {
            var backButtonFeedback = Instantiate(_backButtonFeedback, _input.StylusTransformRaw.Position, Camera.main.transform.rotation);
            Destroy(backButtonFeedback, _feedbackLifeTime);
        }
    }
}

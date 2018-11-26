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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLight.HoloStylus.InputModule;
using HoloLight.HoloStylus.Configuration;

namespace HoloLight.HoloStylus.Examples.Grab
{
    /// <summary>
    /// Grabber is used for attach objects to base grabbing classes.
    /// </summary>
    public class Grabber : MonoBehaviour, IStylusMoveRawHandler, IStylusButtonHandler
    {
        /// <summary>
        /// Center of the grabbed object.
        /// </summary>
        [SerializeField, Tooltip("Center of the grabbed object.")]
        protected Transform GrabAttachSpot;
        /// <summary>
        /// Used base grabbing class
        /// (\ /)
        /// ( . .)
        /// c(")(")
        /// </summary>
        protected BaseGrabbing BaseGrabbing;

        // Access to the input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        // Returns the transform for grabbing
        public Transform GrabHandle
        {
            get
            {
                return GrabAttachSpot == null ? transform : GrabAttachSpot;
            }
        }

        // Registering
        private void Start()
        {
            if (_input != null)
            {
                _input.GlobalListeners.Add(gameObject);
            }
        }

        // Deregistering
        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.GlobalListeners.Remove(gameObject);
            }
        }

        /// <summary>
        /// Grabbing the focused object
        /// </summary>
        /// <param name="sourceID"></param>
        public void OnStylusButtonDown(int sourceID)
        {
            if(sourceID != Globals.ACTION_BUTTON)
            {
                return;
            }

            var focused = _input.FocusedObject.GameObject;
            if (focused != null)
            {
                var grabbing = focused.GetComponent<BaseGrabbing>();
                if(grabbing != null)
                {
                    BaseGrabbing = grabbing;
                    BaseGrabbing.StartGrabbing(this);
                }
            }
        }

        /// <summary>
        /// Release object after grabbing
        /// </summary>
        /// <param name="sourceID"></param>
        public void OnStylusButtonUp(int sourceID)
        {
            if (sourceID != Globals.ACTION_BUTTON)
            {
                return;
            }

            if (BaseGrabbing != null)
            {
                BaseGrabbing.StopGrabbing(this);
                BaseGrabbing = null;
            }
        }

        // not used
        public void OnStylusButton(int sourceID, float value)
        {

        }

        /// <summary>
        /// Updates the position of the stylus
        /// </summary>
        /// <param name="data"></param>
        public void OnStylusTransformDataRawUpdate(StylusTransformData data)
        {
            transform.position = data.Position;
            transform.rotation = data.Rotation;
        }

        /// <summary>
        /// Called when grabbing stopped
        /// </summary>
        public void StopGrabbing()
        {
            // Method after releasing the grabbed object, should be placed here.
        }
    }
}
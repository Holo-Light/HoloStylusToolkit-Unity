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
using System;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.Grab
{
    /// <summary>
    /// States of grabbing
    /// </summary>
    public enum GrabbingState
    {
        Normal,
        AttachedGrab,
        SimpleGrab
    }
    
    /// <summary>
    /// Base class of all grabbing mechanics
    /// </summary>
    [DisallowMultipleComponent]
    public class BaseGrabbing : MonoBehaviour, IStylusFocusHandler
    {
        /// <summary>
        /// Called when grabbing started
        /// </summary>
        public event Action<BaseGrabbing> OnGrabbed;
        /// <summary>
        /// Called when grabbing is released
        /// </summary>
        public event Action<BaseGrabbing> OnReleased;

        [SerializeField, Tooltip("Colors object in normal state.")]
        private Color _normalColor = Color.white;
        [SerializeField, Tooltip("Colors object in highlight state.")]
        private Color _highlightColor = Color.yellow;
        [SerializeField, Tooltip("Colors object in grab state.")]
        private Color _grabbedColor = Color.red;

        /// <summary>
        /// Renderer for coloring
        /// </summary>
        [SerializeField, Tooltip("Renderer for coloring")]
        protected Renderer Renderer;
        /// <summary>
        /// Current grabber
        /// </summary>
        protected Grabber Grabber;
        /// <summary>
        /// Current grabbing state
        /// </summary>
        protected GrabbingState State = GrabbingState.Normal;

        /// <summary>
        /// Access for the input manager instance
        /// </summary>
        protected InputManager InputInstance
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// The attached rigidbody for dragging.
        /// </summary>
        protected Rigidbody Rigidbody;

        /// <summary>
        /// Initialize Renderer and Rigidbody
        /// </summary>
        protected virtual void Start()
        {
            if (Renderer == null)
            {
                Renderer = GetComponentInChildren<Renderer>();
            }

            Rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Base method for start grabbing.
        /// </summary>
        /// <param name="grabber">The current grabber.</param>
        public virtual void StartGrabbing(Grabber grabber)
        {
            Grabber = grabber;
            State = GrabbingState.SimpleGrab;

            AttachGrabber(grabber);
            if (OnGrabbed != null)
            {
                OnGrabbed(this);
            }
            SetColor(_grabbedColor);
        }

        /// <summary>
        /// Base method for attaching grabber to rigidbody
        /// his is only used by joint and lerped based grabbing
        /// </summary>
        /// <param name="grabber"></param>
        protected virtual void AttachGrabber(Grabber grabber)
        {
        }

        /// <summary>
        /// Base method for deattaching grabber to rigidbody
        /// his is only used by joint and lerped based grabbing
        /// </summary>
        /// <param name="grabber"></param>
        protected virtual void DetachGrabber(Grabber grabber)
        {
        }

        /// <summary>
        /// Called every frame while grabbing is active
        /// </summary>
        /// <param name="grabber"></param>
        protected virtual void OnGrabStay(Grabber grabber)
        {
            transform.position = grabber.transform.position;
            transform.rotation = grabber.transform.rotation;

            if (Rigidbody != null)
            {
                Rigidbody.isKinematic = true;
            }
        }

        /// <summary>
        /// Called on grabbing stopped
        /// </summary>
        /// <param name="grabber"></param>
        public virtual void StopGrabbing(Grabber grabber)
        {
            grabber.StopGrabbing();
            State = GrabbingState.Normal;
            Grabber = null;
            DetachGrabber(grabber);
            if (OnReleased != null)
            {
                OnReleased(this);
            }

            if (Rigidbody != null)
            {
                Rigidbody.isKinematic = false;
            }

            if (InputInstance.FocusedObject.GameObject != gameObject)
            {
                SetColor(_normalColor);
            }
        }

        // Call grabbing stay
        protected void Update()
        {
            if (Grabber != null)
            {
                OnGrabStay(Grabber);
            }
        }

        /// <summary>
        /// Highlight active grabber
        /// </summary>
        public void OnStylusEnter()
        {
            if (State == GrabbingState.Normal)
            {
                SetColor(_highlightColor);
            }
        }

        // Set Color of object
        void SetColor(Color color)
        {
            if (Renderer != null)
            {
                Renderer.material.SetColor("_Color", color);
            }
        }

        /// <summary>
        /// Stop highlighting objects
        /// </summary>
        public void OnStylusExit()
        {
            if (State == GrabbingState.Normal)
            {
                SetColor(_normalColor);
            }
        }
    }
}
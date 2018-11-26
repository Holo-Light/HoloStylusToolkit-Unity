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

namespace HoloLight.HoloStylus.FocusModule
{
    internal enum ButtonState
    {
        Up, Down, Pressed
    }

    /// <summary>
    /// Handles all focus events of the stylus.
    /// </summary>
    public class FocusManager : MonoBehaviour
    {
        /// <summary>
        /// Shortcut and readonly variable for Inputmanager.Instance
        /// </summary>
        protected InputManager InputInstance
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// The range from the tip, to check if something is in focus.
        /// </summary>
        [SerializeField, Tooltip("The range from the tip, to check if something is in focus.")]
        public float Range = 0.15f;

        /// <summary>
        /// Ignored object for the raycast check.
        /// </summary>
        [SerializeField, Tooltip("Ignored object for the raycast check.")]
        private LayerMask _cullingMask;

        // The current position of the stylus.
        private Vector3 _currentPosition;

        // The current rotation of the stylus.
        private Quaternion _currentRotation = Quaternion.identity;

        // Last focus of the raycast
        private GameObject _lastFocus;

        /// <summary>
        /// Focus handled with raycasts, instead of colliders.
        /// </summary>
        public bool UseRaycast = true;

        // The focus update with raycast will be called her.
        private void Update()
        {
            _currentPosition = InputInstance.StylusTransformRaw.Position;
            if (UseRaycast)
            {
                Vector3 direction = _currentRotation * Vector3.forward;
                Ray ray = new Ray(_currentPosition, direction);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Range, _cullingMask))
                {
                    if (_lastFocus != hit.collider.gameObject)
                    {
                        SetFocusEnter(new HitInfo(hit));
                    }
                }
                else
                {
                    SetFocusExit(_lastFocus);
                }
            }
        }

        /// <summary>
        /// Call focus exit on all focus interface.
        /// </summary>
        /// <param name="lastFocus">The last focus of the raycast</param>
        public void SetFocusExit(GameObject lastFocus)
        {
            if (lastFocus == null)
            {
                return;
            }

            if (_lastFocus != null)
            {
                DeregisterFocusedEvents();

                var focusInterface = _lastFocus.GetComponent<IStylusFocusHandler>();

                if (focusInterface != null)
                {
                    focusInterface.OnStylusExit();
                }
            }

            InputInstance.FocusedObject.SetHitInfo(new RaycastHit());
            _lastFocus = null;
        }

        /// <summary>
        /// Call focus enter on all focus interfaces and set the hitinfo
        /// </summary>
        /// <param name="hitInfo">The hitinfo of the focus raycast.</param>
        public void SetFocusEnter(HitInfo hitInfo)
        {
            if (_lastFocus != null)
            {
                SetFocusExit(_lastFocus);
            }

            _lastFocus = hitInfo.GameObject;
            InputInstance.FocusedObject.SetHitInfo(hitInfo);

            if (_lastFocus != null)
            {
                RegisterFocusedEvents();

                var focusInterface = _lastFocus.GetComponent<IStylusFocusHandler>();

                if (focusInterface != null)
                {
                    focusInterface.OnStylusEnter();
                }
            }
        }

        private void RegisterFocusedEvents()
        {
            var iStylusInputs = _lastFocus.GetComponents<IStylusInputHandler>();

            foreach (var iStylusInput in iStylusInputs)
            {
                var buttonHandler = iStylusInput as IStylusButtonHandler;
                if (buttonHandler != null)
                {
                    InputInstance.OnStylusButtonDown += buttonHandler.OnStylusButtonDown;
                    InputInstance.OnStylusButtonUp += buttonHandler.OnStylusButtonUp;
                    InputInstance.OnStylusButton += buttonHandler.OnStylusButton;
                }

                // Action button clicks
                var actionClickHandler = iStylusInput as IStylusActionClick;
                if (actionClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionClickEvent += actionClickHandler.OnStylusActionClick;
                }

                var actionDoubleClickHandler = iStylusInput as IStylusActionDoubleClick;
                if (actionDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent += actionDoubleClickHandler.OnStylusActionDoubleClick;
                }

                var actionHoldHandler = iStylusInput as IStylusActionHold;
                if (actionHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionHoldEvent += actionHoldHandler.OnStylusActionHold;
                }

                // Back button clicks
                var backClickHandler = iStylusInput as IStylusBackClick;
                if (backClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnBackClickEvent += backClickHandler.OnStylusBackClick;
                }

                var backDoubleClickHandler = iStylusInput as IStylusBackDoubleClick;
                if (backDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent += backDoubleClickHandler.OnStylusBackDoubleClick;
                }

                var backHoldHandler = iStylusInput as IStylusBackHold;
                if (backHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionHoldEvent += backHoldHandler.OnStylusBackHold;
                }
            }
        }

        private void DeregisterFocusedEvents()
        {
            var iStylusInputs = _lastFocus.GetComponents<IStylusInputHandler>();

            foreach (var iStylusInput in iStylusInputs)
            {
                var buttonHandler = iStylusInput as IStylusButtonHandler;
                if (buttonHandler != null)
                {
                    InputInstance.OnStylusButtonDown -= buttonHandler.OnStylusButtonDown;
                    InputInstance.OnStylusButtonUp -= buttonHandler.OnStylusButtonUp;
                    InputInstance.OnStylusButton -= buttonHandler.OnStylusButton;
                }

                // Action button clicks
                var actionClickHandler = iStylusInput as IStylusActionClick;
                if (actionClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionClickEvent -= actionClickHandler.OnStylusActionClick;
                }

                var actionDoubleClickHandler = iStylusInput as IStylusActionDoubleClick;
                if (actionDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent -= actionDoubleClickHandler.OnStylusActionDoubleClick;
                }

                var actionHoldHandler = iStylusInput as IStylusActionHold;
                if (actionHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionHoldEvent -= actionHoldHandler.OnStylusActionHold;
                    InputInstance.DefaultClickEventHandler.OnActionHoldStartEvent -= actionHoldHandler.OnStylusActionHoldStart;
                    InputInstance.DefaultClickEventHandler.OnActionHoldEndEvent -= actionHoldHandler.OnStylusActionHoldEnd;
                }

                // Back button clicks
                var backClickHandler = iStylusInput as IStylusBackClick;
                if (backClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnBackClickEvent -= backClickHandler.OnStylusBackClick;
                }

                var backDoubleClickHandler = iStylusInput as IStylusBackDoubleClick;
                if (backDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent -= backDoubleClickHandler.OnStylusBackDoubleClick;
                }

                var backHoldHandler = iStylusInput as IStylusBackHold;
                if (backHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionHoldEvent -= backHoldHandler.OnStylusBackHold;
                    InputInstance.DefaultClickEventHandler.OnActionHoldStartEvent -= backHoldHandler.OnStylusBackHoldStart;
                    InputInstance.DefaultClickEventHandler.OnActionHoldEndEvent -= backHoldHandler.OnStylusBackHoldEnd;
                }
            }
        }
    }
}
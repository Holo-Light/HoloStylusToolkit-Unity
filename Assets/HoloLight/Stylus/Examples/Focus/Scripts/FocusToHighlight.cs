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

namespace HoloLight.HoloStylus.Examples.Focus
{
    /// <summary>
    /// Example for focusing with the Holo-Stylus®
    /// </summary>
    public class FocusToHighlight : MonoBehaviour, IStylusFocusHandler
    {
        /// <summary>
        /// Focus mode / behaviour.
        /// </summary>
        [System.Serializable]
        internal enum FocusMode
        {
            Highlight,
            StayInFocus,
            WaitForChange
        }

        // Attached animator
        private Animator _animator;
        // Parameter name for focusing
        private const string FOCUS_ANIMATION_PARAMETER = "Focused";
        // Last focused object with attached focus to highlight behaviour
        private static FocusToHighlight _lastFocus;


        [SerializeField, Tooltip("Change the focus behaviour")]
        private FocusMode _focusMode = FocusMode.WaitForChange;

        /// <summary>
        /// If object won't change focus style after stylus exit
        /// </summary>
        public bool IsStayInFocus
        {
            get
            {
                return _focusMode == FocusMode.StayInFocus;
            }
        }

        // Initializing
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Calls Focus(true) method if stylus enters the gameobject.
        /// </summary>
        public void OnStylusEnter()
        {
            Focus(true);

            switch (_focusMode)
            {
                case FocusMode.StayInFocus:
                    _lastFocus = this;
                    break;

                case FocusMode.WaitForChange:
                    _lastFocus = this;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Handles the focus of the current object and of the last object
        /// </summary>
        /// <param name="focus"></param>
        public void Focus(bool focus)
        {
            _animator.SetBool(FOCUS_ANIMATION_PARAMETER, focus);

            if(focus && _lastFocus != null)
            {
                if (!_lastFocus.IsStayInFocus)
                {
                    _lastFocus.Focus(false);
                    _lastFocus = null;
                }
            }
        }

        /// <summary>
        /// Calls Focus(false) method if stylus leaves the gameobject.
        /// </summary>
        public void OnStylusExit()
        {
            switch(_focusMode)
            {
                case FocusMode.Highlight:
                    Focus(false);
                    break;

                default:
                    break;
            }
        }
    }
}
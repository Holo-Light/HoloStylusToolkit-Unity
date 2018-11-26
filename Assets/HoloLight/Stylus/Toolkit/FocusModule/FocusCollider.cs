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
using UnityEngine;

namespace HoloLight.HoloStylus.FocusModule
{
    /// <summary>
    /// Focus collider class for focus recognition with collision.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class FocusCollider : MonoBehaviour
    {
        /// <summary>
        /// Linked focus manager for setting the focus.
        /// </summary>
        [SerializeField, Tooltip("Linked focus manager for setting the focus.")]
        private FocusManager _focusManager;

        /// <summary>
        /// Set the focus manager, if the reference is null.
        /// </summary>
        private void OnEnable()
        {
            if (_focusManager == null)
            {
                _focusManager = FindObjectOfType<FocusManager>();
            }

            if (_focusManager != null)
            {
                if (_focusManager.UseRaycast)
                {
                    Debug.LogWarning("Disable focus collider, because raycast usage is set");
                    enabled = false;
                }
            }
        }

        /// <summary>
        /// Set focus on trigger enter.
        /// </summary>
        /// <param name="other">The entered collider.</param>
        private void OnTriggerEnter(Collider other)
        {
            var hitInfo = new HitInfo(other.gameObject, transform.position);
            _focusManager.SetFocusEnter(hitInfo);

        }

        /// <summary>
        /// Set focus exit to current focused object on trigger exit.
        /// </summary>
        /// <param name="other">The entered collider.</param>
        private void OnTriggerExit(Collider other)
        {
            _focusManager.SetFocusExit(other.gameObject);
        }
    }
}
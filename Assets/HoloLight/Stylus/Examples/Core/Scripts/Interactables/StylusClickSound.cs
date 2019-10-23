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
    public class StylusClickSound : MonoBehaviour, IStylusActionClick, IStylusBackClick
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

        public AudioClip SoundClip;
        private AudioSource _audioSource;

        // initializing and register in input manager
        private void OnEnable()
        {
            if (_input == null)
            {
                enabled = false;
                return;
            }

            _audioSource = transform.root.GetComponentInChildren<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
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
        /// Play sound on stylus action click
        /// </summary>
        public void OnStylusActionClick()
        {
            _audioSource.PlayOneShot(SoundClip, 0.7F);
        }

        /// <summary>
        /// Play sound on stylus back click
        /// </summary>
        public void OnStylusBackClick()
        {
            _audioSource.PlayOneShot(SoundClip, 0.7F);
        }
    }
}

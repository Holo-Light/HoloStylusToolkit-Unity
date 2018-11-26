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
using UnityEngine.SceneManagement;

namespace HoloLight.HoloStylus.Examples.SceneCollection
{
    /// <summary>
    /// This class is only for the intro screen.
    /// </summary>
    public class Intro : MonoBehaviour
    {
        /// <summary>
        /// Offset of the intro in front of the camera.
        /// </summary>
        [SerializeField, Tooltip("Offset in front of the camera.")]
        private Vector3 _offset = Vector3.zero;

        /// <summary>
        /// Destroy the intro after x seconds. If the value is negative, it won't be destroyed.
        /// </summary>
        [SerializeField, Tooltip("Destroy the intro after x seconds. If the value is negative, it won't be destroyed.")]
        private float _destroyAfterTime = 3;

        /// <summary>
        /// Destroy the intro after x seconds. If the value is negative, it won't be destroyed.
        /// </summary>
        [SerializeField, Tooltip("Which scene is loaded after destroy time.")]
        private int _sceneToLoad = 1;

        /// <summary>
        /// Attach the intro to the camera.
        /// </summary>
        private void Awake()
        {
            if (Camera.main.transform != null)
            {
                transform.SetParent(Camera.main.transform);
            }

            transform.localPosition = _offset;

            if(_destroyAfterTime > 0)
            {
                Destroy(gameObject, _destroyAfterTime);
            }
        }

        private void OnDestroy()
        {
            SceneManager.LoadScene(_sceneToLoad);
        }

    }
}

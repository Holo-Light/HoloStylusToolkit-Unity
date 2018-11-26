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
    /// Auto loads the collection scene, for connection, calibration, and to get access to all sample scenes.
    /// </summary>
    public class AutoLoadSceneCollection : MonoBehaviour
    {
        /// <summary>
        /// Initializing the collection scene, if it isn't already loaded.
        /// </summary>
        void Awake()
        {
            bool isLoaded = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if(scene.buildIndex == SceneCollectionManager.COLLECTION_SCENE_BUILDINDEX)
                {
                    isLoaded = true;                    
                }
            }

            if(!isLoaded)
            {
                var gos = FindObjectsOfType<GameObject>();

                foreach(var go in gos)
                {
                    if(go != gameObject)
                    {
                        Destroy(go);
                    }
                }

                SceneManager.LoadScene(SceneCollectionManager.COLLECTION_SCENE_BUILDINDEX, LoadSceneMode.Single);
            }
            else
            {

                Destroy(gameObject);
            }
        }
    }
}

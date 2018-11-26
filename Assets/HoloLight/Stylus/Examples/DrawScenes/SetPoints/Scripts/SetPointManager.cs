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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Simple solution for drawing points
    /// </summary>
    public class SetPointManager : BaseDrawManager
    {
        [SerializeField, Tooltip("Point prefab")]
        private GameObject _pointPrefab;

        // Initializing point prefab if it is not set
        private void Awake()
        {
            if (_pointPrefab == null) _pointPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }

        /// <summary>
        /// Base draw method, draws points
        /// </summary>
        public override void DrawObject()
        {
            var point = Instantiate<GameObject>(_pointPrefab, CurrentPosition, Quaternion.identity, transform);
            LastDrawnObjects.Push(point);
        }
    }
}
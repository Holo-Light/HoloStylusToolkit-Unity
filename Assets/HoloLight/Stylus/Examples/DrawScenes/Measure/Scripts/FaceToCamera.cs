#region copyright
/*******************************************************
 * Copyright (C) 2017-2018 Holo-Light GmbH -> <info@holo-light.com>
 * 
 * This file is part of the Stylus SDK.
 * 
 * Stylus SDK can not be copied and/or distributed without the express
 * permission of the Holo-Light GmbH
 * 
 * Author of this file is Alex Werlberger
 *******************************************************/
#endregion
using UnityEngine;

namespace HoloLight.Utilities
{
    public class FaceToCamera : MonoBehaviour
    {
        void Update()
        {
            var cam = Camera.main.transform;
            var direction = transform.position - cam.position;
            direction = new Vector3(direction.x, 0, direction.z);

            transform.forward = direction;
        }
    }
}
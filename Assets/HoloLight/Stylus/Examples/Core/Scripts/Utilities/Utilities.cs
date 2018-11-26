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

namespace UnityEngine
{
    /// <summary>
    /// Various extensions...
    /// </summary>
    public static class UsableExtensions
    {
        /// <summary>
        /// Same method than transform.gameObject.SetActive().
        /// </summary>
        /// <param name="transform">extendable class</param>
        /// <param name="active">sets activation to this value</param>
        public static void SetActive(this Transform transform, bool active)
        {
            transform.gameObject.SetActive(active);
        }
    }
}

namespace HoloLight.Utilities
{
    /// <summary>
    /// Various debug methods
    /// </summary>
    public static class DebugMethods
    {
        /// <summary>
        /// Draw a plane in the scene view
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        public static void DrawPlane(Vector3 position, Vector3 normal)
        {
            Vector3 v3 = Vector3.zero;

            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

            var corner0 = position + v3;
            var corner2 = position - v3;
            var q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, Color.green, 5);
            Debug.DrawLine(corner1, corner3, Color.green, 5);
            Debug.DrawLine(corner0, corner1, Color.green, 5);
            Debug.DrawLine(corner1, corner2, Color.green, 5);
            Debug.DrawLine(corner2, corner3, Color.green, 5);
            Debug.DrawLine(corner3, corner0, Color.green, 5);
            Debug.DrawRay(position, normal, Color.red, 5);
        }
    }
}

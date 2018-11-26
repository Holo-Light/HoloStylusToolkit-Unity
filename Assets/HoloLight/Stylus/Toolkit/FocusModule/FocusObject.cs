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
    /// <summary>
    /// Contains the hitinfo data of the casted focus call and the gameobject informations.
    /// </summary>
    public class FocusObject
    {
        /// <summary>
        /// The hitinfo called by the focus cast.
        /// </summary>
        public HitInfo HitInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// The focused gameobject.
        /// </summary>
        public GameObject GameObject
        {
            get
            {
                if(HitInfo == null)
                {
                    return null;
                }
                return HitInfo.GameObject;
            }
        }

        private IStylusInputHandler[] _attachedStylusInterfaces;

        /// <summary>
        /// All attached stylus interfaces at the focused object.
        /// </summary>
        public IStylusInputHandler[] AttachedStylusInterfaces
        {
            get
            {
                return _attachedStylusInterfaces;
            }
        }

        /// <summary>
        /// Sets all attached stylus interfaces for faster accesses.
        /// </summary>
        /// <param name="interfaces">The focused interfaces.</param>
        void SetAttachedStylusInterfaces(IStylusInputHandler[] interfaces)
        {
            _attachedStylusInterfaces = interfaces;
        }

        /// <summary>
        /// Sets all attached stylus interfaces for faster accesses.
        /// </summary>
        void SetAttachedStylusInterfaces()
        {
            if (GameObject != null)
            {
                _attachedStylusInterfaces = GameObject.GetComponents<IStylusInputHandler>();
            }
            else
            {
                _attachedStylusInterfaces = null;
            }
        }

        /// <summary>
        /// Setter of HitInfo via raycast hitinfo.
        /// </summary>
        /// <param name="hit">The given raycast hitinfo from the focusmanager</param>
        public void SetHitInfo(RaycastHit hit)
        {
            HitInfo = new HitInfo(hit);
            SetAttachedStylusInterfaces();
        }

        /// <summary>
        /// Setter of HitInfo.
        /// </summary>
        /// <param name="hit">The given hitinfo from the focusmanager</param>
        public void SetHitInfo(HitInfo hit)
        {
            HitInfo = hit;
            SetAttachedStylusInterfaces();
        }
    }

    /// <summary>
    /// Own hitinfo, sometimes need to be set. E.g. via FocusCollider.
    /// </summary>
    [System.Serializable]
    public class HitInfo
    {
        /// <summary>
        /// The hitten object.
        /// </summary>
        public GameObject GameObject
        {
            get; private set;
        }

        /// <summary>
        /// The hitten point.
        /// </summary>
        public Vector3 Point
        {
            get; private set;
        }

        /// <summary>
        /// The normal of the hitten point.
        /// </summary>
        public Vector3 Normal
        {
            get; private set;
        }

        /// <summary>
        /// The hitten texture coordinate.
        /// </summary>
        public Vector2 TextureCoordinate
        {
            get; private set;
        }

        /// <summary>
        /// The triangle index of the hitten mesh.
        /// </summary>
        public int TriangleIndex
        {
            get; private set;
        }

        /// <summary>
        /// Distance of the point to center of the cast.
        /// </summary>
        public float Distance
        {
            get; private set;
        }


        #region constructors
        /// <summary>
        /// Hitinfo without raycasthit
        /// </summary>
        public HitInfo()
        {

        }

        /// <summary>
        /// Creates hitinfo of raycasthit
        /// </summary>
        /// <param name="hit"></param>
        public HitInfo(RaycastHit hit)
        {
            if (hit.collider != null)
            {
                GameObject = hit.collider.gameObject;
            }

            Point = hit.point;
            Distance = hit.distance;
            Normal = hit.normal;
            TextureCoordinate = hit.textureCoord;
            TriangleIndex = hit.triangleIndex;
        }

        /// <summary>
        /// Creates hitinfo with hitten gameobject and current position
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="currentPos"></param>
        public HitInfo(GameObject gameObject, Vector3 currentPos)
        {
            GameObject = gameObject;
            Vector3 direction = Vector3.one;

            if (gameObject != null)
            {
                direction = currentPos - gameObject.transform.position;
            }

            Point = currentPos;
            Distance = Mathf.Abs(direction.magnitude);
            Normal = direction.normalized;
            TextureCoordinate = Vector2.zero;
            TriangleIndex = 0;
        }
        #endregion
    }
}
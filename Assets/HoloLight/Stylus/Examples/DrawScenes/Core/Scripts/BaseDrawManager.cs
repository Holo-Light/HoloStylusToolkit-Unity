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
    /// Abstract base draw manager class, inherits Undo, Start, Stop and Draw methods. 
    /// </summary>
    public abstract class BaseDrawManager : MonoBehaviour
    {
        /// <summary>
        /// Base draw method
        /// </summary>
        public abstract void DrawObject();
        /// <summary>
        /// Current position of the stylus
        /// </summary>
        [HideInInspector]
        public Vector3 CurrentPosition;
        /// <summary>
        /// List of last drawn objects
        /// </summary>
        protected Stack<Object> LastDrawnObjects = new Stack<Object>();
        /// <summary>
        /// Delay for toggle draw active
        /// </summary>
        protected float SetDrawingDelay = 1.5f;
        /// <summary>
        /// Flag of drawing
        /// </summary>
        protected bool DrawActive = false;

        /// <summary>
        /// Set draw active
        /// </summary>
        /// <param name="value"></param>
        public void SetDrawing(bool value)
        {

            StopAllCoroutines();
            //DrawActive = value;
            StartCoroutine(SetDrawingRoutine(value));
        }

        /// <summary>
        /// Base draw parent method.
        /// </summary>
        public void BaseDrawObject()
        {
            if(DrawActive)
            {
                DrawObject();
            }
        }

        /// <summary>
        /// Coroutine for toggle draw active
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerator SetDrawingRoutine(bool value)
        {
            if (DrawActive && !value)
            {
                StopDrawing();
            }

            yield return new WaitForSecondsRealtime(SetDrawingDelay);

            if(!DrawActive && value)
            {
                StartDrawing();
            }
        }

        /// <summary>
        /// Override methods for start drawing
        /// </summary>
        protected virtual void StartDrawing()
        {
            DrawActive = true;

        }

        /// <summary>
        /// Override methods for stop drawing
        /// </summary>
        protected virtual void StopDrawing()
        {
            DrawActive = false;
        }

        /// <summary>
        /// Base undo method
        /// </summary>
        public virtual void Undo()
        {
            if (DrawActive)
            {
                return;
            }

            if (LastDrawnObjects.Count == 0)
            {
                return;
            }

            var drawn = LastDrawnObjects.Pop() as GameObject;

            if (drawn != null)
            {
                Destroy(drawn);
            }
        }
    }
}

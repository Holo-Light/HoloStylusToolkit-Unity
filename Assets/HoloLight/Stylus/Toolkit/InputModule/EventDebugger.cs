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
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    public class EventDebugger
    {
        private const int ACCELERATION_CALCULATION_VALUE_COUNT = 5;

        protected InputManager InputInstance
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// For the calculation of the timestamp delta.
        /// </summary>
        private float _timeLastAccelerationUpdate;

        /// <summary>
        /// The current calculated acceleration of the stylus.
        /// </summary>
        public Vector3 CurrentAcceleration { get; private set; }

        /// <summary>
        /// Position array for the acceleration calculation.
        /// </summary>
        private PositionInfo[] _positions = new PositionInfo[ACCELERATION_CALCULATION_VALUE_COUNT];

        /// <summary>
        /// The last positions of the stylus for the acceleration calculation.
        /// </summary>
        private readonly Queue<PositionInfo> _lastPositions = new Queue<PositionInfo>(ACCELERATION_CALCULATION_VALUE_COUNT);

        public EventDebugger() { }

        // Call of delegate for debug informations, return the complete transform data.
        internal void OnStylusTransformDataUpdateInternal(StylusTransformData data)
        {
            if (InputInstance.ShowDebugInfos)
            {
                Debug.Log(data.Position);
            }

            float delta = Time.time - _timeLastAccelerationUpdate;
            _timeLastAccelerationUpdate = Time.time;

            var info = new PositionInfo { DeltaTime = delta, Position = data.Position };

            if (_lastPositions.Count > ACCELERATION_CALCULATION_VALUE_COUNT)
            {
                _lastPositions.Dequeue();
            }

            _lastPositions.Enqueue(info);
            CalculateAcceleration();
        }

        /// <summary>
        /// Calculates the mean acceleration of the last positions.
        /// </summary>
        protected void CalculateAcceleration()
        {
            _positions = _lastPositions.ToArray();

            Vector3 acceleration = Vector3.zero;
            float totalTime = 0;

            for (int posIndex = 1; posIndex < _positions.Length; posIndex++)
            {
                acceleration += _positions[posIndex].Position - _positions[posIndex - 1].Position;
                totalTime += _positions[posIndex].DeltaTime;
            }

            if (totalTime == 0)
            {
                return;
            }

            acceleration /= totalTime;
            CurrentAcceleration = acceleration;
        }

        // Call of the delegate, for debug informations and acceleration calculation, return the complete click data.
        internal void OnStylusButtonDownInternal(int sourceID)
        {
            if (InputInstance.ShowDebugInfos)
            {
                Debug.Log("Button with SourceID: " + sourceID + " pressed");
            }
        }

        // Call of the delegate, for debug informations and acceleration calculation, return the complete click data.
        internal void OnStylusButtonUpInternal(int sourceID)
        {
            if (InputInstance.ShowDebugInfos)
            {
                Debug.Log("Button with SourceID: " + sourceID + " released");
            }
        }

        // Call of the delegate, for debug informations and acceleration calculation, return the complete click data.
        internal void OnStylusButtonInternal(int sourceID, float pressure)
        {
            if (InputInstance.ShowDebugInfos)
            {
                Debug.Log("Button with SourceID: " + sourceID + " changed pressure to: " + pressure);
            }
        }

        // Call of the delegate, for debug informations, return the enter data.
        internal void OnStylusEnterInternal()
        {
            if (InputInstance.ShowDebugInfos)
            {
            }
        }

        // Call of the delegate, for debug informations, return the exit data.
        internal void OnStylusExitInternal()
        {
            if (InputInstance.ShowDebugInfos)
            {
            }
        }
    }
}
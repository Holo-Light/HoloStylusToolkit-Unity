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
using System;
using HoloLight.HoloStylus.Configuration;
using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    public interface IStylusCheckLimit<T> 
    {
        bool IsOutsideLimits(T oldLimit, Thresholds thresholds);
    }

    public static class StylusDataMethods
    {
        public static float GetDistance(float val1, float val2)
        {
            return Mathf.Abs(val1 - val2);
        }

        public static float GetSqrDistance(Vector3 vec1, Vector3 vec2)
        {
            return (vec1 - vec2).sqrMagnitude;
        }
    }

    /// <summary>
    /// The transform data like position and rotation of the Stylus.
    /// </summary>
    [Serializable]
    public struct StylusTransformData : IStylusCheckLimit<StylusTransformData>
    {
        public Vector3 Position; // The position of the tip of the Stylus.
        public Quaternion Rotation; // The rotation of the whole Stylus with the center on the tip.
        public Vector3 Acceleration; // The acceleration of the whole Stylus.

        /// <summary>
        /// Check if stylus transform data is outside of the current limits
        /// </summary>
        /// <param name="oldLimit"></param>
        /// <param name="thresholds"></param>
        /// <returns></returns>
        public bool IsOutsideLimits(StylusTransformData oldLimit, Thresholds thresholds)
        {
            bool lastThresholdIsValid = false;
            float moveDistance = StylusDataMethods.GetSqrDistance(oldLimit.Position, this.Position);

            // check distance threshold
            if (moveDistance > thresholds.Movement)
            {
                lastThresholdIsValid = true;
            }

            //check rotation threshold
            float rotationDistance = StylusDataMethods.GetSqrDistance(oldLimit.Rotation.eulerAngles, this.Rotation.eulerAngles);

            if (rotationDistance > thresholds.Rotation)
            {
                lastThresholdIsValid = true;
            }

            //check acceleration threshold
            float accelerationDistance = StylusDataMethods.GetSqrDistance(oldLimit.Acceleration, this.Acceleration);

            if (accelerationDistance > thresholds.Acceleration)
            {
                lastThresholdIsValid = true;
            }

            return lastThresholdIsValid;
        }

        public override string ToString()
        {
            return string.Format("P: {0}, R: {1}, A: {2}", Position, Rotation, Acceleration);
        }
    }

    /// <summary>
    /// The click data like primary and secondary click of the stylus.
    /// </summary>
    [Serializable]
    public struct StylusButtonData : IStylusCheckLimit<StylusButtonData>
    {
        public int SourceID; // Button source, normally 0 means first button, 1 the second one.
        public float Pressure; // The pressed value of the button.

        /// <summary>
        /// Checks if the button pressure is outside of the limits
        /// </summary>
        /// <param name="oldLimit"></param>
        /// <param name="thresholds"></param>
        /// <returns></returns>
        public bool IsOutsideLimits(StylusButtonData oldLimit, Thresholds thresholds)
        {
            float moveDistance = StylusDataMethods.GetDistance(oldLimit.Pressure, this.Pressure);
            bool isOutOfLimits = moveDistance > thresholds.ButtonPressureThreshold;
            return isOutOfLimits;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, P: {1}", SourceID, Pressure);
        }
    }
}
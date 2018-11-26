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

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Calculator for button pressure values.
    /// </summary>
    public class ButtonThresholdCalculator : ThresholdCalculator<StylusButtonData>
    {
        public ButtonThresholdCalculator(int numberOfRelevantValues, Thresholds thresholds) : base(numberOfRelevantValues, thresholds)
        {

        }
    }

    /// <summary>
    /// ThresholdCalculator for StylusTransformData input values.
    /// </summary>
    public class TransformThresholdCalculator : ThresholdCalculator<StylusTransformData>
    {
        public TransformThresholdCalculator(int numberOfRelevantValues, Thresholds thresholds) : base(numberOfRelevantValues, thresholds)
        {

        }
    }
}
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
namespace HoloLight.HoloStylus.StatusModule
{
    /// <summary>
    /// Base interface for status events.
    /// </summary>
    public interface IStylusStatusHandler { }

    public interface IStylusBatteryHandler : IStylusStatusHandler
    {
        void OnStylusBatteryChanged(float batteryCharge);
        void OnStylusBatteryLow(float batteryCharge);
        void OnStylusBatteryCritical(float batteryCharge);
    }

    /// <summary>
    /// Interface for Hmu battery events.
    /// </summary>
    public interface IStylusHMUBatteryHandler : IStylusStatusHandler
    {
        void OnHmuBatteryChanged(float batteryCharge);
        void OnHMUBatteryLow(float batteryCharge);
        void OnHMUBatteryCritical(float batteryCharge);
    }

    /// <summary>
    /// Interface for detection events.
    /// </summary>
    public interface IStylusDetectionHandler : IStylusStatusHandler
    {
        void OnHMUDetected();
        void OnHMULost();
        void OnStylusDetected();
        void OnStylusLost();
    }
}
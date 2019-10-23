#region copyright
/*******************************************************
 * Copyright (C) 2017-2019 Holo-Light GmbH -> <info@holo-light.com>
 * 
 * This file is part of the Stylus SDK.
 * 
 * Stylus SDK can not be copied and/or distributed without the express
 * permission of the Holo-Light GmbH
 * 
 * Author of this file is Philipp Landgraf
 *******************************************************/
#endregion
#if WINDOWS_UWP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLight.DriverLibrary.Events;
using HoloLight.DriverLibrary.Devices;
namespace HoloLight.HoloStylus.Connection
{
    interface IPositionCalculation
    {

        string BaseDeviceName { get; }
        Vector3 Position { get; }
        float Button1 { get; }
        float Button2 { get; }
        StylusEventArgs StylusData { set; }
        StylusType Version {get;}
        
    }

}
#endif
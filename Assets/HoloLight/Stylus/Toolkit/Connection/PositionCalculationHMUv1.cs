

#if WINDOWS_UWP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloLight.DriverLibrary.Events;
using HoloLight.DriverLibrary.Devices;
namespace HoloLight.HoloStylus.Connection
{



    public class PositionCalculationHMUv1 : IPositionCalculation

    {
        public string BaseDeviceName { get; private set; }
        public Vector3 Position { get; private set; }

        public float Button1 { get; private set; }

        public float Button2 { get; private set; }
        public StylusType Version {get; private set;}

        public StylusEventArgs StylusData
        {
            set { this.StylusDataChanged(value); }
        }

        public PositionCalculationHMUv1()
        {
            Position=new Vector3();
            Button1 = 0;
            Button2 = 0;
            BaseDeviceName = "HoloSense"; // _pajdata   meien HMU heißt jetzt holosense_s000000
            Version=StylusType.Version1;

        }

        private void StylusDataChanged(StylusEventArgs e)
        {
            Position = new Vector3(e.StylusData.Position.X, e.StylusData.Position.Y, e.StylusData.Position.Z);
            Button1 = e.StylusData.ActionButton;
            Button2 = e.StylusData.BackButton;
        }
    }
}
#endif
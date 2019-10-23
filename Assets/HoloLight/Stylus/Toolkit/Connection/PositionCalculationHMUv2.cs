#if WINDOWS_UWP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloLight.DriverLibrary.Devices;
using HoloLight.DriverLibrary.Events;
using Tracker;
namespace HoloLight.HoloStylus.Connection
{

    public class PositionCalculationHMUv2 : IPositionCalculation

    {
        public string BaseDeviceName { get; private set; }

        private Tracker.Tracker _tracker;
        private float[] _cameraData;
        private float[] positionFloat;
        public Vector3 Position { get; private set; }

        public float Button1 { get; private set; }

        public float Button2 { get; private set; }
        public StylusType Version {get; private set;}

        public StylusEventArgs StylusData
        {
            set { this.StylusDataChanged(value); }
        }

        public PositionCalculationHMUv2(string filePath)
        {
            _tracker = new Tracker.Tracker(filePath);
            _cameraData = new float[4];
            positionFloat = new float[3];

            Position = new Vector3();
            Button1 = 0;
            Button2 = 0;
            BaseDeviceName = "HMU_V_2_S";
            Version=StylusType.Version2;

        }

        private void StylusDataChanged(StylusEventArgs e)
        {


            for (int i = 0; i < 4; i++)
            {
                int tmp = e.StylusData.RawData[2 + 2 * i];
                tmp = tmp << 8;
                _cameraData[i] = tmp + (int)e.StylusData.RawData[1 + 2 * i];
            }
            bool visible = true;
            foreach (var element in _cameraData)
            {
                if (element == 4095)
                {
                    visible = false;
                }
            }

            
            if (visible)
            {
            _tracker.CalculateCoordinates(_cameraData, ref positionFloat);
            }
            if(visible&&positionFloat[2]>-0.05f)
            {
                Position = new Vector3(positionFloat[0], positionFloat[1], 0.1f + positionFloat[2]);
            }
            Button1 = e.StylusData.RawData[10];
            Button2 = e.StylusData.RawData[9];
        }
    }
}
#endif
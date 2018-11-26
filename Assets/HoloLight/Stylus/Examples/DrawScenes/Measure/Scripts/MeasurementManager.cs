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
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Draw manager for measurement lines
    /// </summary>
    public class MeasurementManager : BaseDrawManager
    {
        // count current clicks
        private int clickCounter = 0;
        // reference of current measurement object
        private MeasurementObjectManager currentMeasureObjectManager;
        [SerializeField, Tooltip("Prefab with attached MeasurementObjectManager")]
        private MeasurementObjectManager MeasureObjectPrefab;

        // Updates the measurement
        private void Update()
        {
            if (clickCounter % 2 == 1)
            {
                currentMeasureObjectManager.SetPoint(1, CurrentPosition, false);
                currentMeasureObjectManager.DoMeasurement();
            }
        }

        /// <summary>
        /// delete last measurement lines and resets the click counter to 0
        /// </summary>
        public override void Undo()
        {
            base.Undo();
            clickCounter = 0;
        }

        /// <summary>
        /// Draw measurement positions
        /// </summary>
        public override void DrawObject()
        {
            int pointId = clickCounter % 2;
            switch (pointId)
            {
                case 0:
                    var prefab = MeasureObjectPrefab.gameObject;
                    var measureGameObject = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity, transform);
                    currentMeasureObjectManager = measureGameObject.GetComponent<MeasurementObjectManager>();
                    LastDrawnObjects.Push(measureGameObject);
                    break;
                case 1:
                    currentMeasureObjectManager.DoMeasurement();
                    break;

            }
            currentMeasureObjectManager.SetPoint(pointId, CurrentPosition, true);
            clickCounter++;
        }
    }
}
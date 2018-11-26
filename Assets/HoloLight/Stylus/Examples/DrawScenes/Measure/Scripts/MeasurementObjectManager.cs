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
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Measurement object and handler. Handles lines and text.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class MeasurementObjectManager : MonoBehaviour
    {
        // Current two points of the line renderer
        private readonly Vector3[] Points = new Vector3[2];
        // The attached line renderer
        private LineRenderer MeasurementLine;
        [SerializeField, Tooltip("Display for the range")]
        private Text MeasureText;

        // Initialization for the line renderer
        private void Awake()
        {
            MeasurementLine = GetComponent<LineRenderer>();
        }

        // Create green anchor points
        private void CreatePoints(Vector3 position)
        {
            var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.localScale = Vector3.one * 0.005f;
            point.transform.position = position;
            point.transform.SetParent(transform, true);
            var renderer = point.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.green);
            renderer.material.SetColor("_EmissionColor", Color.green * 0.5f);
            renderer.material.EnableKeyword("_EMISSION");
        }

        /// <summary>
        /// Set points of the line
        /// </summary>
        /// <param name="id">Anchor id: 0 for the first anchor, 1 for the second</param>
        /// <param name="point">Position of the point</param>
        /// <param name="drawSphere">If true it displays the green sphere</param>
        public void SetPoint(int id, Vector3 point, bool drawSphere)
        {
            Points[id] = point;
            if (drawSphere)
            {
                CreatePoints(point);
                MeasurementLine.SetPositions(Points);
            }
        }

        /// <summary>
        /// Calculate and show the measurement
        /// </summary>
        public void DoMeasurement()
        {
            MeasurementLine.SetPositions(Points);
            var dist = Vector3.Distance(Points[0], Points[1]) * 100;

            MeasureText.text = dist.ToString("0.00") + " cm";

            MeasureText.transform.parent.position = (Points[0] + Points[1]) * 0.5f + new Vector3(0.0f, 0.02f, 0.0f);
        }
    }
}

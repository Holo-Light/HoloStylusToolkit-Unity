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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Simple solution for drawing lines
    /// </summary>
    public class LineDrawManager : BaseDrawManager
    {
        // Current mode
        internal enum LineMode
        {
            Straight,
            FreeHand,
            None
        }

        [SerializeField, Tooltip("Material of the line")]
        private Material _lineMaterial;
        private LineRenderer _currentLineRenderer;
        private List<Vector3> _positions = new List<Vector3>();
        [SerializeField, Tooltip("minimum distance between to points")]
        private float _minDistanceForContinueDrawing = 0.005f;
        private Color _lineColor = new Color(1, 1, 1, 0.5f);
        private LineMode Mode = LineMode.None;

        // Sets the line material to a new one, if it is not set
        private void Awake()
        {
            if (!_lineMaterial) _lineMaterial = new Material(Shader.Find("Particles/Additive"));
        }

        /// <summary>
        /// Base method of drawing the line in both modes
        /// </summary>
        public override void DrawObject()
        {
            if (_currentLineRenderer == null)
            {
                return;
            }
            switch (Mode)
            {
                case LineMode.Straight:
                    StraightLineDraw();
                    break;
                case LineMode.FreeHand:
                    FreeHandDraw(true);
                    break;
            }
        }

        /// <summary>
        /// Start drawing in straight line mode
        /// </summary>
        public void StraightMode()
        {
            Mode = LineMode.Straight;
            SetDrawing(true);
        }

        /// <summary>
        /// Start drawing in free hand mode
        /// </summary>
        public void FreeHandMode()
        {
            Mode = LineMode.FreeHand;
            SetDrawing(true);
        }

        // Draw method for free hand
        private void FreeHandDraw(bool checkDist)
        {
            if (checkDist && _positions.Count > 1)
            {
                int lastPositionIndex = _positions.Count - 1;
                float dist = Vector3.Distance(_positions[lastPositionIndex], CurrentPosition);
                if (_minDistanceForContinueDrawing > dist)
                {
                    return;
                }
            }

            _positions.Add(CurrentPosition);
            
            SetPositions();
        }

        // Base start drawing method
        protected override void StartDrawing()
        {
            base.StartDrawing();
            var lineGO = new GameObject("Line", typeof(LineRenderer));
            _currentLineRenderer = lineGO.GetComponent<LineRenderer>();
            _currentLineRenderer.sharedMaterial = _lineMaterial;
            _currentLineRenderer.widthMultiplier = 0.005f;
            _currentLineRenderer.startColor = _lineColor;
            _currentLineRenderer.endColor = _lineColor;
        }

        // Base stop drawing method
        protected override void StopDrawing()
        {
            base.StopDrawing();
            LastDrawnObjects.Push(_currentLineRenderer.gameObject);
            switch (Mode)
            {
                case LineMode.FreeHand:
                    FreeHandDraw(false);
                    break;
                case LineMode.Straight:
                    _positions.RemoveAt(_positions.Count - 1);
                    SetPositions();
                    break;
            }
            _positions.Clear();
            Mode = LineMode.None;
        }

        // Updates drawing every frame
        private void Update()
        {
            if (_currentLineRenderer == null)
            {
                return;
            }

            if (Mode == LineMode.Straight && DrawActive)
            {
                if (_currentLineRenderer.positionCount < 2)
                {
                    StraightLineDraw();
                }

                _currentLineRenderer.SetPosition(_currentLineRenderer.positionCount - 1, CurrentPosition);
            }
        }

        // Draw method for straight line mode
        private void StraightLineDraw()
        {

            if (_positions.Count > 1)
            {
                int lastPositionIndex = _positions.Count - 1;
                float dist = Vector3.Distance(_positions[lastPositionIndex], CurrentPosition);
                if (_minDistanceForContinueDrawing > dist)
                {
                    return;
                }
            }

            _positions.Add(CurrentPosition);
            if (_positions.Count >= 2)
            {
                _positions[_positions.Count - 2] = CurrentPosition;
            }
            SetPositions();
        }

        // Set the positions of the line
        private void SetPositions()
        {
            if (_positions.Count < 2)
            {
                return;
            }

            var positions = _positions.ToArray();
            _currentLineRenderer.positionCount = positions.Length;
            _currentLineRenderer.SetPositions(positions);
        }
    }
}

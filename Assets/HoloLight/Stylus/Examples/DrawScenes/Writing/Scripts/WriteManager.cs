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
    /// Simple solution for a write draw manager
    /// </summary>
    public class WriteManager : BaseDrawManager
    {
        [SerializeField, Tooltip("Minimum time before starting free hand drawing")]
        private float _minimumHoldTime = 0f;
        private float _startHoldTime = 0;

        private LineRenderer _writtenLine;
        private SpriteRenderer _writtenPoint;

        [SerializeField, Tooltip("Texture used for writing / the line renderer")]
        private Texture2D _writeTexture;

        private Sprite _pointSprite;

        private List<Vector3> _positions = new List<Vector3>();
        [SerializeField]
        private Material _lineMaterial;

        [SerializeField, Tooltip("Line width")]
        private float _writeSize = 0.01f;
        [SerializeField, Tooltip("Minimum distance, which have to be reached for drawing continously")]
        private float _minDistanceForContinueDrawing = 0.005f;

        private float _drawingDelay = 0.05f;

        // Initialize the materials
        private void Start()
        {
            Rect textureRect = new Rect(0, 0, _writeTexture.width, _writeTexture.height);
            var pointUnitSize = 1 / _writeSize;
            _pointSprite = Sprite.Create(_writeTexture, textureRect, Vector2.one * 0.5f, _writeTexture.width * pointUnitSize);
/*
            _lineMaterial = new Material(Shader.Find("Particles/Additive"))
            {
                mainTexture = _writeTexture
            };
            */
            SetDrawingDelay = _drawingDelay;
        }

        // Base draw method, handles points and free hand drawing
        public override void DrawObject()
        {

            if(!DrawActive)
            {
                return;
            }

            float time = Time.time - _startHoldTime;
            bool tempEnableValue = (time < _minimumHoldTime);
            _writtenPoint.enabled = tempEnableValue;
            _writtenLine.enabled = !tempEnableValue;

            DrawPoint();
            DrawLine();

        }

        // Tries to snap the text to the plane
        void SnapToPlane()
        {
            var planes = new List<Plane>();
            var snappedPlaneNormal = Vector3.zero;
            for(int i = 0; i < _positions.Count - 3; i++)
            {
                var plane = new Plane(_positions[i], _positions[i + 1], _positions[i + 2]);
                planes.Add(plane);
                snappedPlaneNormal += plane.normal;
            }

            snappedPlaneNormal /= planes.Count;

            var snappedPlaneCenter = _writtenLine.bounds.center;

            var snappedPlane = new Plane(snappedPlaneNormal, snappedPlaneCenter);

            for(int i = 0; i < _positions.Count; i++)
            {
                _positions[i] = snappedPlane.ClosestPointOnPlane(_positions[i]);
            }

            _writtenLine.SetPositions(_positions.ToArray());
        }

        // Draw points for writing (like the one of the letter i)
        void DrawPoint()
        {
            _writtenPoint.transform.position = CurrentPosition;
        }

        // Draw free hand lines
        void DrawLine()
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

            SetPositions();
        }

        // Set positions for the line renderer
        private void SetPositions()
        {
            if (_positions.Count < 2)
            {
                return;
            }

            var positions = _positions.ToArray();
            _writtenLine.positionCount = positions.Length;
            _writtenLine.SetPositions(positions);
        }

        protected override void StartDrawing()
        {
            base.StartDrawing();
            _startHoldTime = Time.time;
            var writtenLine = new GameObject("Line");
            var writtenDot = new GameObject("Dot");
            _writtenLine = writtenLine.AddComponent<LineRenderer>();
            _writtenPoint = writtenDot.AddComponent<SpriteRenderer>();

            _writtenPoint.sprite = _pointSprite;
            _writtenLine.material = _lineMaterial;
            _writtenLine.widthMultiplier = 0.005f;
            _positions.Clear();
        }

        protected override void StopDrawing()
        {
            base.StopDrawing();

            if(!_writtenLine.enabled)
            {
                Destroy(_writtenLine.gameObject);
            }
            else
            {
                LastDrawnObjects.Push(_writtenLine.gameObject);
                //SnapToPlane();
            }

            if (!_writtenPoint.enabled)
            {
                Destroy(_writtenPoint.gameObject);
            }
            else
            {
                LastDrawnObjects.Push(_writtenPoint.gameObject);
            }
        }
    }
}

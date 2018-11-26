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
using System.Linq;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Simple polygon draw manager
    /// </summary>
    public class TriangleDrawManager : BaseDrawManager
    {
        [SerializeField, Tooltip("Material of the triangles")]
        private Material _meshMaterial;
        [SerializeField, Tooltip("Material of the preview triangle")]
        private Material _previewMaterial;
        private const int MIN_VERTEX_COUNT = 3;
        private Transform[] _triangleHelper = new Transform[MIN_VERTEX_COUNT - 1];
        [SerializeField, Tooltip("The size of the vertex gizmos of the current triangles")]
        private float _helperSize = 0.01f;
        private Queue<Vector3> _previewVertices = new Queue<Vector3>(MIN_VERTEX_COUNT);

        private MeshFilter _previewMeshFilter;
        private MeshFilter _completeMeshFilter;
        private Mesh _previewMesh;
        private Mesh _completeMesh;
        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();
        private int[] _previewTriangle = new int[MIN_VERTEX_COUNT] { 0, 1, 2 };

        /// <summary>
        /// Base draw method
        /// </summary>
        public override void DrawObject()
        {
            SetVertices();
            DrawCompleteMesh();

        }

        // Initializing the materials
        private void Awake()
        {
            if (!_meshMaterial) _meshMaterial = new Material(Shader.Find("Standard"));
            if (!_previewMaterial) _previewMaterial = new Material(Shader.Find("Standard"));
        }

        // Finalizing the mesh
        private void DrawCompleteMesh()
        {
            if (_vertices.Count < MIN_VERTEX_COUNT)
            {
                return;
            }

            if (_completeMesh == null)
            {
                _completeMesh = new Mesh
                {
                    name = "Complete mesh"
                };
            }

            _triangles.Clear();
            for (int i = 1; i < _vertices.Count - 1; i++)
            {
                int normalDirection = (i % 2 == 0) ? -1 : 1;
                _triangles.Add(i - normalDirection);
                _triangles.Add(i);
                _triangles.Add(i + normalDirection);
            }

            _completeMesh.vertices = _vertices.ToArray();
            _completeMesh.triangles = _triangles.ToArray();
            _completeMesh.RecalculateNormals();
            _completeMeshFilter.mesh = _completeMesh;
        }

        // Draw both triangles, the preview and the one of the mesh
        private void DrawTriangle()
        {
            if (_previewVertices.Count == MIN_VERTEX_COUNT)
            {
                if (_previewMesh == null)
                {
                    _previewMesh = new Mesh
                    {
                        name = "Preview triangle"
                    };

                }

                var previewVerts = _previewVertices.ToArray();
                previewVerts[0] = CurrentPosition;
                _previewMesh.vertices = previewVerts;
                _previewMesh.triangles = _previewTriangle;
                _previewMesh.RecalculateNormals();
                _previewMeshFilter.mesh = _previewMesh;
            }
        }

        // Set current vertices of the triangle
        private void SetVertices()
        {
            _previewVertices.Enqueue(CurrentPosition);
            if (_previewVertices.Count > MIN_VERTEX_COUNT)
            {
                _previewVertices.Dequeue();
            }

            _vertices.Add(CurrentPosition);

            if (_previewVertices.Count <= 2)
            {
                _triangleHelper[1].position = _previewVertices.ElementAt(0);
                _triangleHelper[1].SetActive(true);
            }

            for (int i = 1; i < _previewVertices.Count; i++)
            {
                _triangleHelper[i - 1].position = _previewVertices.ElementAt(i);
                _triangleHelper[i - 1].SetActive(true);
            }
        }

        // Updates the triangle preview
        private void Update()
        {
            DrawTriangle();
        }

        protected override void StopDrawing()
        {
            base.StopDrawing();
            for (int i = _triangleHelper.Length - 1; i >= 0; i--)
            {
                if (_triangleHelper[i] != null)
                {
                    Destroy(_triangleHelper[i].gameObject);
                }

                _triangleHelper[i] = null;
            }

            Destroy(_previewMeshFilter.gameObject);
            _previewVertices.Clear();
            _triangles.Clear();
            _vertices.Clear();
            LastDrawnObjects.Push(_completeMeshFilter.gameObject);
            if (_completeMeshFilter.mesh.vertices.Length <= 2)
            {
                Undo();
            }
            _previewMesh = null;
            _completeMesh = null;

        }

        protected override void StartDrawing()
        {
            base.StartDrawing();

            for (int i = 0; i < MIN_VERTEX_COUNT - 1; i++)
            {
                var tri = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tri.name = "TriangleHelper " + i.ToString();
                _triangleHelper[i] = tri.transform;
                _triangleHelper[i].localScale = Vector3.one * _helperSize;
                _triangleHelper[i].SetActive(false);
            }

            _previewMeshFilter = GenerateObjectWithMeshFilter("Preview", _previewMaterial);
            _completeMeshFilter = GenerateObjectWithMeshFilter("CompleteMesh", _meshMaterial);
        }

        // Initialize the object with mesh filter, mesh renderer, etc.
        private MeshFilter GenerateObjectWithMeshFilter(string objectName, Material material)
        {
            var currentObject = new GameObject(objectName);
            var meshFilter = currentObject.AddComponent<MeshFilter>();
            var renderer = currentObject.AddComponent<MeshRenderer>();
            renderer.material = material;

            return meshFilter;
        }
    }
}

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
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Base draw manager for coloring objects
    /// </summary>
    public class ColoringManager : BaseDrawManager
    {
        /// <summary>
        /// To avoid having millions of particles
        /// </summary>
        private const int MAX_BRUSH_COUNT = 200;

        /// <summary>
        /// Default invoke time
        /// </summary>
        private const float INVOKE_TIME = 0.02f;

        /// <summary>
        /// The range of the ray casted from stylus
        /// </summary>
        private const float RAY_RANGE = 1.5f;

        /// <summary>
        /// Culling mask for the brush raycast
        /// </summary>
        [SerializeField, Tooltip("Culling mask for the brush raycast")]
        private LayerMask _cullingMask = new LayerMask();

        /// <summary>
        /// Brush gameobject
        /// </summary>
        [SerializeField, Tooltip("Brush gameobject")]
        private GameObject _brushCursor;

        /// <summary>
        /// The camera with rendertexture that looks at the canvas of the painted texture.
        /// </summary>
        [SerializeField, Tooltip("The camera with rendertexture that looks at the canvas of the painted texture.")]
        private Camera _canvasCam;

        /// <summary>
        /// Render Texture that looks at our Base Texture and the painted brushes
        /// </summary>
        private RenderTexture _renderTexture;

        /// <summary>
        /// The material of our base texture (Place of applied texture)
        /// </summary>
        [SerializeField, Tooltip("The material of our base texture (Place of applied texture)")]
        private RawImage _paintedTexture;

        /// <summary>
        /// The particle system aka the brush for painting.
        /// </summary>
        [SerializeField, Tooltip("The particle system aka the brush for painting.")]
        private ParticleSystem _brushSystem;

        /// <summary>
        /// Base template texture
        /// </summary>
        [SerializeField, Tooltip("Base template texture")]
        private Texture2D _templateTexture;

        /// <summary>
        ///The size of our brush
        /// </summary>
        private float _brushSize = 1.0f;

        private const float MIN_BRUSH_SIZE = 0.01f;

        /// <summary>
        ///Flag to check if we are saving the texture
        /// </summary>
        private bool _applying = false;

        /// <summary>
        /// Backup of the current camera culling mask
        /// </summary>
        private LayerMask _oldMask;

        /// <summary>
        /// Initialize method
        /// </summary>
        private void Start()
        {
            if(_canvasCam == null)
            {
                foreach(var cams in FindObjectsOfType<Camera>())
                {
                    if(cams.orthographic == true && cams.targetTexture != null)
                    {
                        _canvasCam = cams;
                    }
                }
            }

            var cam = Camera.main;
            _oldMask = cam.cullingMask;
            cam.cullingMask = ~_cullingMask;
            _canvasCam.cullingMask = _cullingMask;
            _renderTexture = _canvasCam.targetTexture;
        }

        /// <summary>
        /// Clear method
        /// </summary>
        private void OnDestroy()
        {
            if (Camera.main != null)
            {
                Camera.main.cullingMask = _oldMask;
            }
        }

        /// <summary>
        /// Draw method of the basedrawmanager, handles the coloring
        /// </summary>
        public override void DrawObject()
        {
            if (_applying)
                return;
            Vector3 uvWorldPosition = Vector3.zero;
            if (GetUVPosition(ref uvWorldPosition))
            {
                _brushSystem.transform.position = uvWorldPosition;
                _brushSystem.transform.localScale = Vector3.one * _brushSize;
                _brushSystem.Emit(1);
            }
            if (_brushSystem.particleCount >= MAX_BRUSH_COUNT)
            {
                _brushCursor.SetActive(false);
                _applying = true;

#if NET_4_6
                Invoke(nameof(ApplyTexture), INVOKE_TIME);
#else
                Invoke("ApplyTexture", INVOKE_TIME);
#endif
            }
        }

        /// <summary>
        /// Updates the brush position
        /// </summary>
        void Update()
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (GetUVPosition(ref uvWorldPosition) && !_applying)
            {
                _brushCursor.SetActive(true);
                _brushCursor.transform.localPosition = uvWorldPosition;
            }
            else
            {
                _brushCursor.SetActive(false);
            }
        }

        /// <summary>
        /// Returns the position on the texuremap according to a hit in the mesh collider
        /// </summary>
        bool GetUVPosition(ref Vector3 uvWorldPosition)
        {
            RaycastHit hit;
            var camPos = Camera.main.transform.position;
            Ray cursorRay = new Ray(CurrentPosition, CurrentPosition - camPos);

            if (Physics.Raycast(cursorRay, out hit, RAY_RANGE, ~_cullingMask))
            {
                if (hit.collider == null && (hit.collider as MeshCollider) == null)
                {
                    return false;
                }

                Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
                uvWorldPosition.x = pixelUV.x - _canvasCam.orthographicSize;//To center the UV on X
                uvWorldPosition.y = pixelUV.y - _canvasCam.orthographicSize;//To center the UV on Y
                uvWorldPosition.z = 0.0f;
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Sets the base material with a our canvas texture, then removes all our brushes
        /// </summary>
        void ApplyTexture()
        {
            RenderTexture.active = _renderTexture;
            Texture2D tex = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            _paintedTexture.texture = tex;
            _brushSystem.Clear();

#if NET_4_6
            Invoke(nameof(ShowCursor), INVOKE_TIME);
#else
            Invoke("ShowCursor", INVOKE_TIME);
#endif
        }

        /// <summary>
        /// Set texture to white
        /// </summary>
        void ClearTexture()
        {
            Texture2D tex = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
            if (_templateTexture == null)
            {
                Color32[] resetColorArray = tex.GetPixels32();

                for (int i = 0; i < resetColorArray.Length; i++)
                {
                    resetColorArray[i] = Color.white;
                }

                tex.SetPixels32(resetColorArray);
                tex.Apply();
            }
            else
            {
                Graphics.CopyTexture(_templateTexture, tex);
            }

            _paintedTexture.texture = tex;
            _brushSystem.Clear();
        }

        /// <summary>
        /// Set texture apply false and show cursor
        /// </summary>
        void ShowCursor()
        {
            _applying = false;
        }

        /// <summary>
        /// Add size to current brushsize. If it's lower than MIN_BRUSH_VALUE nothing happens.
        /// </summary>
        /// <param name="addValue">Additive value for the brush size.</param>
        public void SetBrushSize(float addValue)
        {
            ApplyTexture();
            if (_brushSize + addValue < MIN_BRUSH_SIZE)
            {
                return;
            }

            _brushSize += addValue;
            _brushCursor.transform.localScale = Vector3.one * _brushSize;
        }

        /// <summary>
        /// Set brush color
        /// </summary>
        /// <param name="color">Target brush color</param>
        public void SetColor(Color color)
        {
            ApplyTexture();
            var main = _brushSystem.main;
            main.startColor = color;
        }

        /// <summary>
        /// Undo and clear the texture.
        /// </summary>
        public override void Undo()
        {
            ClearTexture();
        }
    }

}

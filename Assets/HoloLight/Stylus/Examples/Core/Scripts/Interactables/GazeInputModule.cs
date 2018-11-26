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
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.SceneCollection
{
    /// <summary>
    /// Input module for gaze selection
    /// </summary>
    public class GazeInputModule : PointerInputModule
    {
        /// <summary>
        /// Default gaze size for the input module
        /// </summary>
        private const float GAZE_SIZE = 0.025f;
        /// <summary>
        /// Default gaze color for the input module
        /// </summary>
        private readonly Color GAZE_COLOR = new Color(1, 0.5f, 0, 1);

        [SerializeField, Tooltip("Min focus time for select/submit")]
        private float _focusGazeTime = 1.5f;
        // Current time
        private float _time = 0;
        // Loading bar component
        private Image _loadBar;

        [SerializeField, Tooltip("Sprite for focus")]
        private Sprite _loadingCircleSprite;
        [SerializeField, Tooltip("Gaze sprite")]
        private Sprite _gazeSprite;

        // Load bar canvas
        private GameObject _loadBarCanvas;

        // pointer event data for the gaze
        private PointerEventData _gaze;

        /// <summary>
        /// Current raycast result of the gaze
        /// </summary>
        public RaycastResult CurrentRaycastResult;

        // Flag for clicking
        private bool _clicked = false;

        // Last focused object
        private GameObject _lastHit;
        // Current focused object
        private GameObject _currentHit;
        // Current clickable object (hierarchy based from focused object)
        private GameObject _clickableObject;
        [SerializeField, Tooltip("Gaze offset")]
        private float _offset = 0.02f;
        [SerializeField, Tooltip("Max gaze range")]
        private float _range = 2.5f;

        /// <summary>
        /// Called on input module activation (important for instantiating the gaze)
        /// </summary>
        public override void ActivateModule()
        {
            base.ActivateModule();
            var canvasObject = new GameObject("Loading bar canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var image = new GameObject("Loading bar", typeof(RectTransform), typeof(Image));
            image.transform.SetParent(canvasObject.transform, false);
            
            var imageTransform = image.GetComponent<RectTransform>();
            imageTransform.anchorMax = Vector2.one;
            imageTransform.anchorMin = Vector2.zero;
            imageTransform.sizeDelta = Vector2.zero;
            imageTransform.anchoredPosition3D = Vector3.zero;

            var canvasTransform = canvasObject.GetComponent<RectTransform>();
            canvasTransform.anchorMax = Vector2.zero;
            canvasTransform.anchorMin = Vector2.zero;
            canvasTransform.sizeDelta = Vector2.one * GAZE_SIZE;
            canvasTransform.anchoredPosition3D = Vector3.zero;

            _loadBar = image.GetComponent<Image>();
            _loadBar.sprite = _gazeSprite;
            _loadBar.color = GAZE_COLOR;
            _loadBar.type = Image.Type.Simple;
            _loadBar.sprite = _loadingCircleSprite;
            _loadBarCanvas = canvasObject;

            _gaze = new PointerEventData(eventSystem);
        }

        /// <summary>
        /// Delete the canvas instance
        /// </summary>
        public override void DeactivateModule()
        {
            base.DeactivateModule();
            DestroyImmediate(_loadBarCanvas);
        }

        /// <summary>
        /// Processes all methods of the input module (required of the abstract class)
        /// </summary>
        public override void Process()
        {
            if (Camera.main == null)
            {
                return;
            }

            HandlePosition();
            HandleFocus();
        }

        /// <summary>
        /// Handles the positioning of the gaze
        /// </summary>
        private void HandlePosition()
        {
            _gaze.position = new Vector2(Screen.width / 2, Screen.height / 2);
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            eventSystem.RaycastAll(_gaze, raycastResults);
            _gaze.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
            CurrentRaycastResult = FindFirstRaycast(raycastResults);
            ProcessMove(_gaze);
        }

        /// <summary>
        /// Handles focusing and submitting of the gaze
        /// </summary>
        private void HandleFocus()
        {

            _currentHit = _gaze.pointerEnter;

            if (_clickableObject != null)
            {
                _loadBar.sprite = _loadingCircleSprite;
                _loadBar.type = Image.Type.Filled;
                _loadBar.fillMethod = Image.FillMethod.Radial360;
                if(_currentHit != null)
                {
                    _loadBarCanvas.transform.position = _currentHit.transform.position - Camera.main.transform.forward * _offset;
                }
                else
                {
                    _loadBarCanvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * _range;
                }
            }
            else
            {
                _loadBar.sprite = _gazeSprite;
                _loadBar.type = Image.Type.Simple;
                _loadBarCanvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * _range;
            }

            _loadBarCanvas.transform.forward = Camera.main.transform.forward;


            if (_currentHit == _lastHit)
            {
                if (_clicked)
                {
                    return;
                }

                var focusTime = Time.time - _time;

                if (_focusGazeTime < focusTime)
                {
                    _clicked = true;
                    if (_clickableObject != null)
                    {
                        try
                        {
                            _clickableObject = ExecuteEvents.ExecuteHierarchy(_clickableObject, _gaze, ExecuteEvents.pointerClickHandler);
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    _loadBar.fillAmount = focusTime / _focusGazeTime;
                }
            }
            else
            {
                _lastHit = _currentHit;
                _clickableObject = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_gaze.pointerEnter);
                _time = Time.time;
                _clicked = false;
            }
        }
    }
}

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
using HoloLight.HoloStylus.Configuration;
using HoloLight.HoloStylus.InputModule;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


namespace HoloLight.HoloStylus.Examples.SceneCollection
{
    /// <summary>
    /// Input module for the stylus interaction. Stylus works like a pointer.
    /// </summary>
    public class StylusInputModule : PointerInputModule, IStylusButtonHandler
    {
        /// <summary>
        /// Drag state machine
        /// </summary>
        internal enum DragState
        {
            Start,
            Drag,
            Stop
        }

        // Access to the input manager
        private InputManager _input
        {
            get
            {
                return InputManager.Instance;
            }
        }

        /// <summary>
        /// Current drag state.
        /// </summary>
        private DragState _dragState = DragState.Stop;

        // Clicking flag
        private bool _isClicked = false;

        // Pointer data of the stylus
        private PointerEventData _pointerEventData;
        // Current focus result
        public RaycastResult CurrentRaycastResult;
        // Last position
        private Vector2 _lastPointerPosition;
        // Register flag for the input manager
        private bool _isRegistered = false;

        // Current dragable object (object requires IDragable interface)
        private GameObject _currentDragHandler = null;

        // Camera for the pointer input module
        private Camera _currentCamera;
        // Property of the actuall camera, returns always the current camera
        private Camera _mainCamera
        {
            get
            {
                if (_currentCamera == null)
                {
                    _currentCamera = Camera.main;
                    if (_currentCamera == null)
                    {
                        _currentCamera = FindObjectOfType<Camera>();
                        if (_currentCamera == null)
                        {
                            Debug.LogError("No camera in scene.");
                        }
                    }
                }

                return _currentCamera;
            }
        }

        /// <summary>
        /// Processes all methods of the input module (required of the abstract class)
        /// </summary>
        public override void Process()
        {
            if (_input == null && enabled == false)
            {
                return;
            }

            RegisterInputModule();
            HandlePosition();
            HandleDrag();
            HandleClick();
        }

        /// <summary>
        /// Register this object to the stylus input manager
        /// </summary>
        private void RegisterInputModule()
        {
            if (_input == null)
            {
                _isRegistered = false;
                return;
            }

            if (_isRegistered)
            {
                return;
            }

            _input.GlobalListeners.Add(gameObject);

            _isRegistered = true;
        }

        /// <summary>
        /// Handles the positioning
        /// </summary>
        void HandlePosition()
        {
            var currentPos = _input.StylusTransformRaw.Position;

            Vector2 pointerPosition = _mainCamera.WorldToScreenPoint(currentPos);

            if (_pointerEventData == null)
            {
                _pointerEventData = new PointerEventData(eventSystem);
                _lastPointerPosition = pointerPosition;
            }

            _pointerEventData.position = pointerPosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            eventSystem.RaycastAll(_pointerEventData, raycastResults);
            CurrentRaycastResult = _pointerEventData.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
            ProcessMove(_pointerEventData);
        }

        /// <summary>
        /// Handles draggin (currently not finished)
        /// </summary>
        private void HandleDrag()
        {
            if (_pointerEventData.pointerEnter != null)
            {
                switch (_dragState)
                {
                    case DragState.Start:
                        var dragHandler = ExecuteEvents.GetEventHandler<IDragHandler>(_pointerEventData.pointerEnter);
                        if (dragHandler == null)
                        {
                            _dragState = DragState.Stop;
                            break;
                        }

                        _currentDragHandler = dragHandler;
                        var dragBeginHandler = ExecuteEvents.GetEventHandler<IBeginDragHandler>(_currentDragHandler);
                        var initPotentialDragHandler = ExecuteEvents.GetEventHandler<IInitializePotentialDragHandler>(_currentDragHandler);

                        _pointerEventData.dragging = true;
                        _pointerEventData.delta = Vector2.zero;
                        _pointerEventData.pointerDrag = dragHandler;
                        _pointerEventData.pointerPress = dragHandler;
                        ExecuteEvents.ExecuteHierarchy<IInitializePotentialDragHandler>(initPotentialDragHandler, _pointerEventData, ExecuteEvents.initializePotentialDrag);
                        ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(dragBeginHandler, _pointerEventData, ExecuteEvents.beginDragHandler);
                        //ExecuteEvents.ExecuteHierarchy<IDragHandler>(_currentDragHandler, _pointerEventData, ExecuteEvents.dragHandler);
                        _dragState = DragState.Drag;
                        break;

                    case DragState.Drag:
                        var currentPos = _input.StylusTransformRaw.Position;
                        Vector2 pointerPosition = _mainCamera.WorldToScreenPoint(currentPos);
                        //_pointerEventData.position = pointerPosition;
                        _pointerEventData.delta = _lastPointerPosition - pointerPosition;
                        _lastPointerPosition = pointerPosition;
                        ExecuteEvents.ExecuteHierarchy<IDragHandler>(_currentDragHandler, _pointerEventData, ExecuteEvents.dragHandler);
                        break;

                    case DragState.Stop:
                        /*var dragEndHandler = */
                        ExecuteEvents.ExecuteHierarchy<IEndDragHandler>(_currentDragHandler, _pointerEventData, ExecuteEvents.endDragHandler);
                        _pointerEventData.dragging = false;
                        _pointerEventData.delta = Vector2.zero;
                        _pointerEventData.pointerDrag = null;
                        break;
                }
            }
            ProcessDrag(_pointerEventData);
        }

        /// <summary>
        /// Important class for debugging in the editor.
        /// </summary>
        /// <returns>Complete information about the current inputmodule data</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("<b>Pointer Input Module of type: </b>" + GetType());
            sb.AppendLine();
            foreach (var pointer in m_PointerData)
            {
                if (pointer.Value == null)
                    continue;
                sb.AppendLine("<B>Pointer:</b> " + pointer.Key);
                sb.AppendLine(pointer.Value.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Handles the submitting/selecting of the stylus.
        /// </summary>
        private void HandleClick()
        {
            if (_isClicked)
            {
                _isClicked = false;
                GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_pointerEventData.pointerEnter);
                if (handler == null)
                {
                    return;
                }
                try
                {
                    handler = ExecuteEvents.ExecuteHierarchy(handler, _pointerEventData, ExecuteEvents.pointerClickHandler);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Interface for stylus button down.
        /// </summary>
        /// <param name="sourceID">Source id of the clicked button</param>
        public void OnStylusButtonDown(int sourceID)
        {
            if (sourceID == Globals.ACTION_BUTTON)
            {
                _isClicked = true;
                _dragState = DragState.Start;
            }
        }

        /// <summary>
        /// Interface for stylus button up.
        /// </summary>
        /// <param name="sourceID">Source id of the clicked button</param>
        public void OnStylusButtonUp(int sourceID)
        {
            if (sourceID == Globals.ACTION_BUTTON)
            {
                _dragState = DragState.Stop;
            }
        }

        /// <summary>
        /// Interface for stylus button clicked.
        /// </summary>
        /// <param name="sourceID">Source id of the clicked button</param>
        /// <param name="value">The clicked value</param>
        public void OnStylusButton(int sourceID, float value)
        {

        }
    }
}
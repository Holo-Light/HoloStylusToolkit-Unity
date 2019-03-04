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
using HoloLight.HoloStylus.FocusModule;
using HoloLight.HoloStylus.Listener;

using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// The inputmanager class handles all input events of the stylus interface.
    /// All stylus interfaces should manipulate the main datas of the Inputmanager.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        //private const int ACCELERATION_CALCULATION_VALUE_COUNT = 5;

        /// <summary>
        /// Singleton
        /// </summary>
        public static InputManager Instance;

        [SerializeField, Tooltip(Globals.HMU_FIELD_MSG)]
        private Transform _hmuTransform;

        /// <summary>
        /// If the hmu transform is the camera transform
        /// </summary>
        [HideInInspector]
        public bool AutoSetHMU = false;

        /// <summary>
        /// Transform of the HMU. E.g. if the HMU is on the hololens, then the Camera is the HMU transform.
        /// </summary>
        public Transform HMUTransform
        {
            get { return _hmuTransform; }
        }

        /// <summary>
        /// Stylus configuration asset file
        /// </summary>
        [Tooltip("The stylus configuration asset file")]
        public ConfigurationObject Configuration;

        /// <summary>
        /// Debug the stylus data in the console.
        /// </summary>
        [SerializeField, Tooltip(Globals.DEBUG_FIELD_MSG)]
        private bool _showDebugInfos = false;
        internal bool ShowDebugInfos
        {
            get { return _showDebugInfos; }
        }

        private readonly FocusObject _focusObject = new FocusObject();
        /// <summary>
        /// The current focused object of the stylus, setted by the focus manager.
        /// </summary>
        public FocusObject FocusedObject { get { return _focusObject; } }

        private StylusTransformations _stylusTransformations;
        private StylusButtons _buttons;
        private EventDebugger _eventDebugger;

        public const int RECALCULATED_STYLUS_TRANSFORMATION_FRAME = 5;

        /// <summary>
        /// Position, rotation and acceleration of the stylus.
        /// (Calculated to world space in relation to the HMU).
        /// If no HMU transfrom is set, then no recalculation will be done.
        /// Value will only be updated when the change exceeds the threshold
        /// </summary>
        public StylusTransformData StylusTransform
        {
            get { return _stylusTransformations.StylusTransform; }
            set { _stylusTransformations.StylusTransform = value; }
        }

        ///// <summary>
        ///// Position, rotation and acceleration of the stylus.
        /// (Calculated to world space in relation to the HMU).
        ///// If no HMU transfrom is set, then no recalculation will be done.
        ///// </summary>
        public StylusTransformData StylusTransformRaw
        {
            get { return _stylusTransformations.StylusTransformRaw; }
            set { _stylusTransformations.StylusTransformRaw = value; }
        }

        /// <summary>
        /// Position, rotation and acceleration of the stylus.
        /// (Calculated to world space in relation to the HMU).
        /// If no HMU transfrom is set, then no recalculation will be done.
        /// </summary>
        public StylusTransformData StylusTransformBeforeCalibrated
        {
            get { return _stylusTransformations.StylusTransformBeforeCalibrated; }
        }

        /// <summary>
        /// Pressure data of the action button
        /// </summary>
        public StylusButtonData StylusActionButtonData
        {
            get { return _buttons.StylusActionButtonData; }
            set { _buttons.StylusActionButtonData = value; }
        }

        /// <summary>
        /// Pressure data of the back button
        /// </summary>
        public StylusButtonData StylusBackButtonData
        {
            get { return _buttons.StylusBackButtonData; }
            set { _buttons.StylusBackButtonData = value; }
        }

        private readonly GlobalListener _globalListener = new GlobalListener();

        /// <summary>
        /// The listener class for all stylus interfaces.
        /// </summary>
        public GlobalListener GlobalListeners
        {
            get { return _globalListener; }
        }

        #region Events

        public readonly DefaultClickEventHandler DefaultClickEventHandler = new DefaultClickEventHandler();

        /// <summary>
        /// Raised when stylus transform raw data has changed.
        /// </summary>
        public event StylusTransformDataUpdateHandler OnStylusTransformDataRawUpdate;

        /// <summary>
        /// Raised when stylus transform data has changed and a threshold has been passed.
        /// </summary>
        public event StylusTransformDataUpdateHandler OnStylusTransformDataUpdate;

        /// <summary>
        /// Raised when a stylus button has been pressed.
        /// </summary>
        public event StylusButtonPressedHandler OnStylusButtonDown;

        /// <summary>
        /// Raised when a stylus button has been released.
        /// </summary>
        public event StylusButtonPressedHandler OnStylusButtonUp;

        /// <summary>
        /// Raised when stylus button pressure has changed.
        /// </summary>
        public event StylusButtonHandler OnStylusButton;

        /// <summary>
        /// Raised when a new object is focused by the stylus.
        /// </summary>
        public event StylusFocusHandler OnStylusEnter;

        /// <summary>
        /// Raised when an object looses focus of the stylus. 
        /// </summary>
        public event StylusFocusHandler OnStylusExit;

        /// <summary>
        /// Raised if stylus is lost, outside the field of view
        /// (currently not finished)
        /// </summary>
        public event DeviceDetectionHandler OnStylusLost;
        /// <summary>
        /// Raised if stylus is detected, inside the field of view
        /// (currently not finished)
        /// </summary>
        public event DeviceDetectionHandler OnStylusDetected;

        /// <summary>
        /// Raised if hmus is lost, service connection lost
        /// (currently not finished)
        /// </summary>
        public event DeviceDetectionHandler OnHmuLost;
        /// <summary>
        /// Raised if hmus is detected, service connection returned
        /// (currently not finished)
        /// </summary>
        public event DeviceDetectionHandler OnHmuDetected;
        #endregion Events

        // Set the singleton instance of InputManager here.
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            _stylusTransformations = new StylusTransformations();
            _buttons = new StylusButtons();

            _eventDebugger = new EventDebugger();
        }

        // Register the internal methods.
        protected void Start()
        {
            LookForHMU();
            RegisterEvents();
        }

        // Deregister the internal methods.
        protected void OnDestroy()
        {
            DeregisterEvents();
            GlobalListeners.Clear();
        }

        // Set HMU transform, if there is a camera.
        private void LookForHMU()
        {
            if (AutoSetHMU)
            {
                if (Camera.main != null)
                {
                    _hmuTransform = Camera.main.transform;
                }
                else
                {
                    var cam = FindObjectOfType<Camera>();
                    if (cam != null)
                    {
                        _hmuTransform = cam.transform;
                    }
                    else
                    {
                        Debug.LogWarning(Globals.HMU_AUTOSET_MSG);
                    }
                }
            }


            var hmuTransform = new GameObject("HMU Transform");
            hmuTransform.transform.SetParent(_hmuTransform, false);
            //hmuTransform.transform.localPosition = Vector3.zero;
            //hmuTransform.transform.localRotation = Quaternion.identity;
            _hmuTransform = hmuTransform.transform;

            _hmuTransform.localPosition = CalibrationPreferences.HMUCalibrationData.Position;
            _hmuTransform.localRotation = CalibrationPreferences.HMUCalibrationData.Rotation;

        }

        private void RegisterEvents()
        {
            OnStylusTransformDataRawUpdate += _eventDebugger.OnStylusTransformDataUpdateInternal;
            OnStylusButtonDown += _eventDebugger.OnStylusButtonDownInternal;

            OnStylusButton += _eventDebugger.OnStylusButtonInternal;
            OnStylusButtonUp += _eventDebugger.OnStylusButtonUpInternal;

            OnStylusEnter += _eventDebugger.OnStylusEnterInternal;
            OnStylusExit += _eventDebugger.OnStylusExitInternal;

            OnStylusButton += DefaultClickEventHandler.RaiseStylusButtonHold;
            OnStylusButtonDown += DefaultClickEventHandler.StylusButtonDown;
            OnStylusButtonUp += DefaultClickEventHandler.RaiseStylusButtonUp;
        }

        private void DeregisterEvents()
        {
            OnStylusTransformDataRawUpdate -= _eventDebugger.OnStylusTransformDataUpdateInternal;
            OnStylusButtonDown -= _eventDebugger.OnStylusButtonDownInternal;

            OnStylusButton -= _eventDebugger.OnStylusButtonInternal;
            OnStylusButtonUp -= _eventDebugger.OnStylusButtonUpInternal;

            OnStylusEnter -= _eventDebugger.OnStylusEnterInternal;
            OnStylusExit -= _eventDebugger.OnStylusExitInternal;

            OnStylusButton -= DefaultClickEventHandler.RaiseStylusButtonHold;
            OnStylusButtonDown -= DefaultClickEventHandler.StylusButtonDown;
            OnStylusButtonUp -= DefaultClickEventHandler.RaiseStylusButtonUp;

            GlobalListeners.Clear();
        }

        internal void RaiseStylusTransformChanged(StylusTransformData stylusTransform)
        {
            RaiseStylusTransformUpdate(OnStylusTransformDataUpdate, stylusTransform);
        }

        internal void RaiseStylusTransformRawChanged(StylusTransformData stylusTransformRaw)
        {
            RaiseStylusTransformUpdate(OnStylusTransformDataRawUpdate, stylusTransformRaw);
        }

        private void RaiseStylusTransformUpdate(StylusTransformDataUpdateHandler handler, StylusTransformData stylusTransform)
        {
            if (handler != null)
            {
                handler(stylusTransform);
            }
        }

        /// <summary>
        /// Called whenever button pressure changes.
        /// </summary>
        internal void RaiseStylusButtonChanged(StylusButtonData value)
        {
            if (OnStylusButton != null)
            {
                OnStylusButton(value.SourceID, value.Pressure);
            }
        }

        internal void RaiseStylusButtonDown(StylusButtonData value)
        {
            RaiseStylusButtonPressed(OnStylusButtonDown, value);
        }

        internal void RaiseStylusButtonUp(StylusButtonData value)
        {
            RaiseStylusButtonPressed(OnStylusButtonUp, value);
        }

        /// <summary>
        /// Called when button is pressed or released.
        /// </summary>
        internal void RaiseStylusButtonPressed(StylusButtonPressedHandler handler, StylusButtonData value)
        {
            if (handler != null)
            {
                handler(value.SourceID);
            }
            else
            {
                Debug.Log("Handler " + handler.ToString() + " has no registration");
            }
        }
    }
}
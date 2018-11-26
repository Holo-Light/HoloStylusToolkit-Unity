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
using HoloLight.HoloStylus.InputModule;
using HoloLight.HoloStylus.StatusModule;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Listener
{
    /// <summary>
    /// Global listener listens to all input events of the stylus.
    /// </summary>
    public class GlobalListener
    {
        private readonly List<GameObject> _listeners = new List<GameObject>();

        /// <summary>
        /// All listeners.
        /// </summary>
        public List<GameObject> Listeners
        {
            get { return _listeners; }
        }

        /// <summary>
        /// Singleton instance of InputManager.
        /// </summary>
        protected InputManager InputInstance
        {
            get
            {
                return InputManager.Instance;
            }
        }

        protected readonly StatusManager StatusInstance = StatusManager.Instance;

        /// <summary>
        /// Remove all attached gameobjects from the global listener.
        /// </summary>
        public void Clear()
        {
            GameObject[] listeners = Listeners.ToArray();

            foreach(var listener in listeners)
            {
                Remove(listener);
            }
        }

        /// <summary>
        /// Add all interfaces attached to the gameobject to the global listener. 
        /// If no interface is found, add is ignored.
        /// </summary>
        /// <param name="newGameObject">The gameobject which listens globally.</param>
        public void Add(GameObject newGameObject)
        {
            if(Listeners.Contains(newGameObject))
            {
                return;
            }
            var stylusInputInterfaces = newGameObject.GetComponents<IStylusInputHandler>();

            if (stylusInputInterfaces == null)
            {
                return;
            }

            RegisterInputHandlers(stylusInputInterfaces);
            Listeners.Add(newGameObject);
        }

        /// <summary>
        /// Remove all interfaces attached the global listener gameobject.
        /// If no interface is found, remove is ignored.
        /// </summary>
        /// <param name="gameObjectToRemove">The gameobject which listens globally.</param>
        public void Remove(GameObject gameObjectToRemove)
        {
            if(gameObjectToRemove == null)
            {
                return;
            }

            // todo: macht wenig sinn alles zu löschen
            //RemoveEmptyListeners();

            IStylusInputHandler[] inputHandlers = gameObjectToRemove.GetComponents<IStylusInputHandler>();

            if (inputHandlers == null)
            {
                return;
            }

            DeregisterInputHandlers(inputHandlers);
            Listeners.Remove(gameObjectToRemove);
        }

        /// <summary>
        /// Register OnStylusMethods to the InputManager.
        /// </summary>
        /// <param name="iStylusInputs">The interface attached to the listener object.</param>
        private void RegisterInputHandlers(IStylusInputHandler[] iStylusInputs)
        {
            foreach (var iStylusInput in iStylusInputs)
            {
                var handler = iStylusInput as IStylusButtonHandler;

                if (handler != null)
                {
                    InputInstance.OnStylusButtonDown += handler.OnStylusButtonDown;
                    InputInstance.OnStylusButtonUp += handler.OnStylusButtonUp;
                    InputInstance.OnStylusButton += handler.OnStylusButton;
                }

                // Action button clicks
                var actionClickHandler = iStylusInput as IStylusActionClick;
                if (actionClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionClickEvent += actionClickHandler.OnStylusActionClick;
                }

                var actionDoubleClickHandler = iStylusInput as IStylusActionDoubleClick;
                if (actionDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent += actionDoubleClickHandler.OnStylusActionDoubleClick;
                }

                var actionHoldHandler = iStylusInput as IStylusActionHold;
                if (actionHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionHoldEvent += actionHoldHandler.OnStylusActionHold;
                    InputInstance.DefaultClickEventHandler.OnActionHoldStartEvent += actionHoldHandler.OnStylusActionHoldStart;
                    InputInstance.DefaultClickEventHandler.OnActionHoldEndEvent += actionHoldHandler.OnStylusActionHoldEnd;
                }

                // Back button clicks
                var backClickHandler = iStylusInput as IStylusBackClick;
                if (backClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnBackClickEvent += backClickHandler.OnStylusBackClick;
                }

                var backDoubleClickHandler = iStylusInput as IStylusBackDoubleClick;
                if (backDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent += backDoubleClickHandler.OnStylusBackDoubleClick;
                }

                var backHoldHandler = iStylusInput as IStylusBackHold;
                if (backHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnBackHoldEvent += backHoldHandler.OnStylusBackHold;
                    InputInstance.DefaultClickEventHandler.OnBackHoldStartEvent += backHoldHandler.OnStylusBackHoldStart;
                    InputInstance.DefaultClickEventHandler.OnBackHoldEndEvent += backHoldHandler.OnStylusBackHoldEnd;
                }

                var rawHandler = iStylusInput as IStylusMoveRawHandler;

                if (rawHandler != null)
                {
                    InputInstance.OnStylusTransformDataRawUpdate += rawHandler.OnStylusTransformDataRawUpdate;
                }

                var moveHandler = iStylusInput as IStylusMoveHandler;

                if (moveHandler != null)
                {
                    InputInstance.OnStylusTransformDataUpdate += moveHandler.OnStylusTransformDataUpdate;
                }
            }
        }

        /// <summary>
        /// Deregister OnStylusMethods from InputManager.
        /// </summary>
        /// <param name="iStylusInputs">The interface attached to the listener object.</param>
        private void DeregisterInputHandlers(IStylusInputHandler[] iStylusInputs)
        {
            foreach (var iStylusInput in iStylusInputs)
            {
                var handler = iStylusInput as IStylusButtonHandler;

                if (handler != null)
                {
                    InputInstance.OnStylusButtonDown -= handler.OnStylusButtonDown;
                    InputInstance.OnStylusButtonUp -= handler.OnStylusButtonUp;
                    InputInstance.OnStylusButton -= handler.OnStylusButton;
                }

                // Action button clicks
                var actionClickHandler = iStylusInput as IStylusActionClick;
                if (actionClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionClickEvent -= actionClickHandler.OnStylusActionClick;
                }

                var actionDoubleClickHandler = iStylusInput as IStylusActionDoubleClick;
                if (actionDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent -= actionDoubleClickHandler.OnStylusActionDoubleClick;
                }

                var actionHoldHandler = iStylusInput as IStylusActionHold;
                if (actionHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionHoldEvent -= actionHoldHandler.OnStylusActionHold;
                    InputInstance.DefaultClickEventHandler.OnActionHoldStartEvent -= actionHoldHandler.OnStylusActionHoldStart;
                    InputInstance.DefaultClickEventHandler.OnActionHoldEndEvent -= actionHoldHandler.OnStylusActionHoldEnd;
                }

                // Back button clicks
                var backClickHandler = iStylusInput as IStylusBackClick;
                if (backClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnBackClickEvent -= backClickHandler.OnStylusBackClick;
                }

                var backDoubleClickHandler = iStylusInput as IStylusBackDoubleClick;
                if (backDoubleClickHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnActionDoubleClickEvent -= backDoubleClickHandler.OnStylusBackDoubleClick;
                }

                var backHoldHandler = iStylusInput as IStylusBackHold;
                if (backHoldHandler != null)
                {
                    InputInstance.DefaultClickEventHandler.OnBackHoldEvent -= backHoldHandler.OnStylusBackHold;
                    InputInstance.DefaultClickEventHandler.OnBackHoldStartEvent -= backHoldHandler.OnStylusBackHoldStart;
                    InputInstance.DefaultClickEventHandler.OnBackHoldEndEvent -= backHoldHandler.OnStylusBackHoldEnd;
                }

                var rawHandler = iStylusInput as IStylusMoveRawHandler;

                if (rawHandler != null)
                {
                    InputInstance.OnStylusTransformDataRawUpdate -= rawHandler.OnStylusTransformDataRawUpdate;
                }

                var moveHandler = iStylusInput as IStylusMoveHandler;

                if (moveHandler != null)
                {
                    InputInstance.OnStylusTransformDataUpdate -= moveHandler.OnStylusTransformDataUpdate;
                }
            }
        }

        /// <summary>
        /// Register OnStylusStatusMethods to the InputManager.
        /// </summary>
        /// <param name="iStylusStates">The interface attached to the listener object.</param>
        private void RegisterStatusHandlers(IStylusStatusHandler[] iStylusStates)
        {
            foreach (var iStylusStatus in iStylusStates)
            {
                var handler = iStylusStatus as IStylusBatteryHandler;

                if (handler != null)
                {
                    StatusInstance.OnStylusBatteryChanged += handler.OnStylusBatteryChanged;
                    StatusInstance.OnStylusBatteryLow += handler.OnStylusBatteryLow;
                    StatusInstance.OnStylusBatteryCritical += handler.OnStylusBatteryCritical;
                }

                var batteryHandler = iStylusStatus as IStylusHMUBatteryHandler;

                if (batteryHandler != null)
                {
                    StatusInstance.OnHMUBatteryChanged += batteryHandler.OnHmuBatteryChanged;
                    StatusInstance.OnHMUBatteryLow += batteryHandler.OnHMUBatteryLow;
                    StatusInstance.OnHMUBatteryCritical += batteryHandler.OnHMUBatteryCritical;
                }

                var detectionHandler = iStylusStatus as IStylusDetectionHandler;

                if(detectionHandler != null)
                {
                    InputInstance.OnHmuDetected += detectionHandler.OnHMUDetected;
                    InputInstance.OnHmuLost += detectionHandler.OnHMULost;
                    InputInstance.OnStylusDetected += detectionHandler.OnStylusDetected;
                    InputInstance.OnStylusLost += detectionHandler.OnStylusLost;
                }
            }
        }

        /// <summary>
        /// Deregister OnStylusStatusMethods from InputManager.
        /// </summary>
        /// <param name="iStylusStates">The interface attached to the listener object.</param>
        private void DeregisterStatusHandlers(IStylusStatusHandler[] iStylusStates)
        {
            foreach (var iStylusState in iStylusStates)
            {
                var handler = iStylusState as IStylusBatteryHandler;

                if (handler != null)
                {
                    StatusInstance.OnStylusBatteryChanged -= handler.OnStylusBatteryChanged;
                    StatusInstance.OnStylusBatteryLow -= handler.OnStylusBatteryLow;
                    StatusInstance.OnStylusBatteryCritical -= handler.OnStylusBatteryCritical;
                }

                var batteryHandler = iStylusState as IStylusHMUBatteryHandler;

                if(batteryHandler != null)
                {
                    StatusInstance.OnHMUBatteryChanged -= batteryHandler.OnHmuBatteryChanged;
                    StatusInstance.OnHMUBatteryLow -= batteryHandler.OnHMUBatteryLow;
                    StatusInstance.OnHMUBatteryCritical -= batteryHandler.OnHMUBatteryCritical;
                }

                var detectionHandler = iStylusState as IStylusDetectionHandler;

                if (detectionHandler != null)
                {
                    InputInstance.OnHmuDetected -= detectionHandler.OnHMUDetected;
                    InputInstance.OnHmuLost -= detectionHandler.OnHMULost;
                    InputInstance.OnStylusDetected -= detectionHandler.OnStylusDetected;
                    InputInstance.OnStylusLost -= detectionHandler.OnStylusLost;
                }
            }
        }
    }
}
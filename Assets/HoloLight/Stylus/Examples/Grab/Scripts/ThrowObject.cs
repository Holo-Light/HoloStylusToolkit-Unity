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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.Grab
{
    /// <summary>
    /// Simple solution for throwing grabbed objects
    /// </summary>
    [RequireComponent(typeof(BaseGrabbing), typeof(Rigidbody)), DisallowMultipleComponent]
    public class ThrowObject : MonoBehaviour
    {
        internal enum ThrowState
        {
            None,
            Grabbed,
            Released
        }

        private ThrowState _state = ThrowState.None;

        private BaseGrabbing _baseGrabbing;

        private Rigidbody _rigidbody;

        private Queue<Vector3> _accelerationValues = new Queue<Vector3>();
        private int _queueLength = 5;

        [SerializeField, Range(0.01f, 50)]
        private float _initialSpeedMultiplier = 25;

        private void Start()
        {
            _baseGrabbing = GetComponent<BaseGrabbing>();
            _rigidbody = GetComponent<Rigidbody>();
            _baseGrabbing.OnReleased += OnReleased;
            _baseGrabbing.OnGrabbed += OnGrabbed;
        }

        private void OnDestroy()
        {
            if(_baseGrabbing == null)
            {
                return;
            }
            _baseGrabbing.OnReleased -= OnReleased;
            _baseGrabbing.OnGrabbed -= OnGrabbed;
        }

        private void OnReleased(BaseGrabbing grabbing)
        {
            _state = ThrowState.Released;
        }

        private void OnGrabbed(BaseGrabbing grabbing)
        {
            _state = ThrowState.Grabbed;
        }

        private void FixedUpdate()
        {
            switch(_state)
            {
                case ThrowState.None:
                    return;
                case ThrowState.Grabbed:
                    _accelerationValues.Enqueue(transform.position);
                    if(_accelerationValues.Count > _queueLength)
                    {
                        _accelerationValues.Dequeue();
                    }
                    break;
                case ThrowState.Released:
                    _state = ThrowState.None;
                    var values = _accelerationValues.ToArray();
                    if (values.Length > 0)
                    {
                        var currentAcceleration = values[_queueLength - 1] - values[0];
                        _rigidbody.AddForce(currentAcceleration * _initialSpeedMultiplier, ForceMode.Impulse);
                    }
                    _accelerationValues.Clear();                    
                    break;
            }
        }

    }
}

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

namespace HoloLight.HoloStylus.Examples.Grab
{
    /// <summary>
    /// Grabbing method, simple direct grabbing
    /// </summary>
    public class LerpedGrabbing : BaseGrabbing
    {
        [SerializeField, Range(1, 10)]
        private float _lerpSpeed = 1.3f;

        protected override void AttachGrabber(Grabber grabber)
        {
            base.AttachGrabber(grabber);
            Rigidbody.useGravity = false;
            State = GrabbingState.AttachedGrab;
        }

        protected override void DetachGrabber(Grabber grabber)
        {
            base.DetachGrabber(grabber);
            Rigidbody.useGravity = true;
            State = GrabbingState.Normal;
        }

        protected override void OnGrabStay(Grabber grabber)
        {
            transform.position = Vector3.Lerp(transform.position, grabber.GrabHandle.position, Time.deltaTime * _lerpSpeed);
        }
    }
}
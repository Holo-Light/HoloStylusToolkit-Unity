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
    public class HingeJointGrabbing : BaseGrabbing
    {
        [SerializeField]
        protected float BreakForce = 100;

        [SerializeField]
        protected float BreakTorque = 100;

        [SerializeField]
        protected float MinDistance = -360;

        [SerializeField]
        protected float MaxDistance = 360;

        protected override void AttachGrabber(Grabber grabber)
        {
            base.AttachGrabber(grabber);
            HingeJoint joint = GetComponent<HingeJoint>();
            if (joint == null)
            {
                joint = gameObject.AddComponent<HingeJoint>();
            }
            joint.connectedBody = grabber.GetComponent<Rigidbody>();
            var anchor = grabber.transform.position - transform.position;
            joint.anchor = anchor;
            joint.axis = Vector3.one;
            joint.breakForce = BreakForce;
            joint.breakTorque = BreakTorque;
            joint.useLimits = true;
            var limits = joint.limits;
            limits.min = MinDistance;
            limits.max = MaxDistance;
            joint.limits = limits;
        }

        protected override void OnGrabStay(Grabber grabber)
        {
            // Do nothing, because physic engine will do for us
        }

        protected override void DetachGrabber(Grabber grabber)
        {
            base.DetachGrabber(grabber);
            HingeJoint joint = GetComponent<HingeJoint>();
            if (joint != null)
            {
                joint.connectedBody = null;
                StartCoroutine(DestroyJointAfterDelay(joint));
            }
        }

        protected IEnumerator DestroyJointAfterDelay(HingeJoint joint)
        {
            yield return null;
            if (State == GrabbingState.Normal)
            {
                DestroyImmediate(joint);
            }
            yield return null;
        }
    }
}

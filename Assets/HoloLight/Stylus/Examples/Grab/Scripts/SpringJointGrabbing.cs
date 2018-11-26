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
    /// Simple solution with grabbing using a spring joint component
    /// </summary>
    public class SpringJointGrabbing : BaseGrabbing
    {
        // expose the joint variables here for editing because the joint is added/destroyed at runtime
        // to understand how these variables work in greater depth see documentation for spring joint and fixed joint
        [Header("Spring joint variables")]
        [SerializeField, Tooltip("The maximum force the joint can withstand before braking.")]
        protected float BreakForce = float.PositiveInfinity;

        [SerializeField, Tooltip("Maximum torque the joint can withstand before breaking")]
        protected float BreakTorque = float.PositiveInfinity;

        [SerializeField, Tooltip("The spring / elasticity")]
        protected float Spring = 20;

        [SerializeField, Tooltip("The damping that effects the spring")]
        protected float Damper = 0.5f;

        [SerializeField, Tooltip("The scale to apply to the inverse mass and inertia tensor of the body.")]
        protected float MassScale = 0.5f;

        protected override void AttachGrabber(Grabber grabber)
        {
            base.AttachGrabber(grabber);
            SpringJoint joint = GetComponent<SpringJoint>();
            if (joint == null)
            {
                joint = gameObject.AddComponent<SpringJoint>();
            }
            joint.connectedBody = grabber.GetComponent<Rigidbody>();
            var anchor = grabber.transform.position - transform.position;
            joint.anchor = anchor;
            joint.axis = Vector3.one;
            joint.breakForce = BreakForce;
            joint.breakTorque = BreakTorque;
            joint.spring = Spring;
            joint.damper = Damper;
            joint.massScale = MassScale;
            joint.enableCollision = true;
        }

        protected override void OnGrabStay(Grabber grabber)
        {
            // Do nothing, because physic engine will do for us
        }

        protected override void DetachGrabber(Grabber grabber)
        {
            base.DetachGrabber(grabber);
            SpringJoint joint = GetComponent<SpringJoint>();
            if (joint != null)
            {
                joint.connectedBody = null;
                StartCoroutine(DestroyJointAfterDelay(joint));
            }
        }

        protected IEnumerator DestroyJointAfterDelay(SpringJoint joint)
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

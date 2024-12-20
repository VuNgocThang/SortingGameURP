﻿using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class UnityContact : MonoBehaviour
    {
        public bool CanContact;
        public LayerMask ContactLayer;
        public float Strength = 1.1f;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!CanContact) return;
            // https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

            // make sure we hit a non kinematic rigidbody
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic) return;

            // make sure we only push desired layer(s)
            var bodyLayerMask = 1 << body.gameObject.layer;
            if ((bodyLayerMask & ContactLayer.value) == 0) return;

            // We dont want to push objects below us
            if (hit.moveDirection.y < -0.3f) return;

            // Calculate push direction from move direction, horizontal motion only
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

            // Apply the push and take strength into account
            body.AddForce(pushDir * Strength, ForceMode.Impulse);
        }
    }
}
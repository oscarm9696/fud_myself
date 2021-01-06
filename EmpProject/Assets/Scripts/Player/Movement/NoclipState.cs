﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Movement : MonoBehaviour {
    private class NoclipState : MovementState {
        public NoclipState(Movement movement) : base(movement) {

        }

        private int layer;

        public override void OnStateEnter() {
            print("Entering fly mode");

            layer = _movement.gameObject.layer;

            _movement.gameObject.layer = LayerMask.NameToLayer("TriggerOnly");

            _movement.capsuleCollider.enabled = false;
            _movement.characterController.detectCollisions = false;
        }

        public override void OnStateUpdate() {
            Vector3 movementDir = _movement.cameraTransform.forward * _movement.desiredMovement.z + _movement.transform.right * _movement.desiredMovement.x;

            _movement.ApplyFriction(_movement.friction * 2f);
            _movement.DoAcceleration(movementDir, _movement.acceleration * 2f, (!_movement.isCrouching ? _movement.maxVelocity : _movement.crouchVelocity) * 2f);
        }

        public override void OnStateExit() {
            print("Exiting fly mode");

            _movement.capsuleCollider.enabled = true;
            _movement.characterController.detectCollisions = true;

            _movement.gameObject.layer = layer;
        }
    }
}
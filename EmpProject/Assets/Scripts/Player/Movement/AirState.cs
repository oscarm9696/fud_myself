﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class Movement : MonoBehaviour {
    private class AirState : MovementState {
        public AirState(Movement movement) : base(movement) { }

        private float enterTime;
        private float maxVelocity;

        public override void OnStateEnter() {
            enterTime = Time.timeSinceLevelLoad;

            maxVelocity = Mathf.Max(_movement.velocity.WithY(0).magnitude,1f);

            _movement.characterController.slopeLimit = 90f;
        }

        public override void OnStateUpdate() {
            if (_movement.grounded && (Time.timeSinceLevelLoad - enterTime) > 0.1f) _movement.SetState(new GroundedState(_movement));

            if (!_movement.isDashing) {

                _movement.ApplyFriction(_movement.airDrag);

                var downVector = Vector3.down;

                // Project gravity direction on the slope we're hitting.
                if (_movement.groundedNormal.y < Mathf.Sin(_movement.slopeAngle)) downVector = Vector3.ProjectOnPlane(Vector3.down, _movement.groundedNormal);

                Debug.DrawLine(_movement.transform.position, _movement.transform.position + downVector);

                // Faster fall velocity.
                if (_movement.velocity.y > -_movement.fallMaxSpeedUp) _movement.velocity += downVector * -Physics.gravity.y * (_movement.fallSpeedMultiplier - 1) * Time.deltaTime;

                // Apply Gravity.
                _movement.velocity += Vector3.down * -Physics.gravity.y * Time.deltaTime;

                _movement.DoAcceleration(_movement.transform.TransformDirection(_movement.desiredMovement), _movement.airAcceleration, Mathf.Min(maxVelocity, _movement.airCap));
            }

            if (_movement.wishJump && _movement.jumps < _movement.maxJumps) {
                _movement.DoJump();
            }
        }

        public override void OnStateExit() {
            _movement.characterController.slopeLimit = _movement.slopeAngle;
        }
    }
}
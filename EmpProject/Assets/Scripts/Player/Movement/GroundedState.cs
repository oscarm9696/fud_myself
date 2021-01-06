using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class Movement : MonoBehaviour {
    private class GroundedState : MovementState {
        public GroundedState(Movement movement) : base(movement) {

        }

        public override void OnStateEnter() {
            _movement.velocity.y = 0;
            _movement.jumps = 0;
        }

        public override void OnStateUpdate() {

            if (_movement.wishJump)
                _movement.ApplyFriction(0f);
            else
                _movement.ApplyFriction(_movement.friction);

            var rayCast = Physics.Raycast(_movement.transform.position, Vector3.down, out RaycastHit hit, _movement.characterController.height / 2 + _movement.characterController.skinWidth + 1f);

            if (!_movement.grounded) _movement.SetState(new AirState(_movement));

            _movement.characterController.Move(Vector3.down * hit.distance * _movement.stickyness);

            _movement.ApplyFriction(_movement.friction);

            var movementDir = _movement.transform.TransformDirection(_movement.desiredMovement);
            movementDir.y += Vector3.Dot(movementDir, -_movement.groundedNormal);

            _movement.DoAcceleration(movementDir, _movement.acceleration, !_movement.isCrouching ? _movement.maxVelocity : _movement.crouchVelocity);

            _movement.velocity = _movement.velocity.normalized * Mathf.Min(_movement.velocity.magnitude, _movement.maxVelocity);

            if ( _movement.wishJump && _movement.jumps < _movement.maxJumps) {
                _movement.DoJump();
            }
        }

        public override void OnStateExit() {
        }
    }
}
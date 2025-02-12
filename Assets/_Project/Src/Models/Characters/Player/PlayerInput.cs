using UnityEngine;

namespace Models.Characters.Player
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerInput : MonoBehaviour
    {
        private Vector2 _movementInput;
        private bool _isSprinting;

        public Vector3 GetMovementDirection()
        {
            return new Vector3(_movementInput.x, 0, _movementInput.y);
        }

        public bool IsSprinting => _isSprinting;
    }
}
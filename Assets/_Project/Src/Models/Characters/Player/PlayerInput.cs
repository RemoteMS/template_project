using UnityEngine;

namespace Models.Characters.Player
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerInput : MonoBehaviour
    {
        private Vector2 movementInput;
        private bool isSprinting;

        public Vector3 GetMovementDirection()
        {
            return new Vector3(movementInput.x, 0, movementInput.y);
        }

        public bool IsSprinting => isSprinting;
    }
}
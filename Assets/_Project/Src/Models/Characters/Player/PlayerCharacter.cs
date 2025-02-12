using Models.Characters.Base;
using UnityEngine;

namespace Models.Characters.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerCharacter : Character
    {
        private PlayerInput _input;

        protected override void Awake()
        {
            base.Awake();
            _input = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            var moveDirection = _input.GetMovementDirection();
            Movement.Move(moveDirection);
        }
    }
}
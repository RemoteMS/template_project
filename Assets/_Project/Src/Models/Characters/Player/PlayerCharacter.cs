using Models.Characters.Base;

namespace Models.Characters.Player
{
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
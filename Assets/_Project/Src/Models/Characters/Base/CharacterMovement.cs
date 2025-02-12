using UnityEngine;

namespace Models.Characters.Base
{
    public class CharacterMovement : MonoBehaviour
    {
        private CharacterStats _stats;
        private CharacterEffects _effects;

        private void Awake()
        {
            _stats = GetComponent<CharacterStats>();
            _effects = GetComponent<CharacterEffects>();
        }

        public void Move(Vector3 direction)
        {
            var speedModifier = _effects.GetSpeedModifier();
            var movement = direction * (_stats.baseSpeed * speedModifier * Time.deltaTime);
        }
    }
}
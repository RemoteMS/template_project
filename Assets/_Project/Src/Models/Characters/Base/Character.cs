using UnityEngine;

namespace Models.Characters.Base
{
    [RequireComponent(typeof(CharacterEffects))]
    [RequireComponent(typeof(CharacterHealth), typeof(CharacterStats), typeof(CharacterMovement))]
    public abstract class Character : MonoBehaviour
    {
        public CharacterHealth Health { get; private set; }
        public CharacterStats Stats { get; private set; }
        public CharacterMovement Movement { get; private set; }
        public CharacterEffects Effects { get; private set; }

        protected virtual void Awake()
        {
            Health = GetComponent<CharacterHealth>();
            Stats = GetComponent<CharacterStats>();
            Movement = GetComponent<CharacterMovement>();
            Effects = GetComponent<CharacterEffects>();
        }
    }
}
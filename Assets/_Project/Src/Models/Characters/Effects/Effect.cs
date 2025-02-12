using Models.Characters.Base;
using UnityEngine;

namespace Models.Characters.Effects
{
    public abstract class Effect : MonoBehaviour
    {
        public float Duration { get; protected set; }
        public float SpeedModifier { get; protected set; } = 1f;

        public abstract void Apply(CharacterEffects character);
        public abstract void Remove(CharacterEffects character);
    }
}
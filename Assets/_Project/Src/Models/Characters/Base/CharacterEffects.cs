using System.Collections.Generic;
using Models.Characters.Effects;
using UnityEngine;

namespace Models.Characters.Base
{
    public class CharacterEffects : MonoBehaviour
    {
        private readonly List<Effect> _activeEffects = new();

        public void ApplyEffect(Effect effect)
        {
            _activeEffects.Add(effect);
            effect.Apply(this);
        }

        public void RemoveEffect(Effect effect)
        {
            _activeEffects.Remove(effect);
            effect.Remove(this);
        }

        public float GetSpeedModifier()
        {
            var modifier = 1f;
            foreach (var effect in _activeEffects)
            {
                modifier *= effect.SpeedModifier;
            }

            return modifier;
        }
    }
}
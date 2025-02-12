using System.Collections.Generic;
using Models.Characters.Base;
using Models.Characters.Effects;
using UnityEngine;

namespace Services.Managers
{
    public class EffectManager
    {
        private readonly List<Effect> _activeEffects = new();


        public void ApplyEffect(Character character, Effect effect)
        {
            var newEffect = Object.Instantiate(effect, character.transform);
            character.Effects.ApplyEffect(newEffect);
            _activeEffects.Add(newEffect);
        }

        public void RemoveEffect(Character character, Effect effect)
        {
            character.Effects.RemoveEffect(effect);
            _activeEffects.Remove(effect);
            Object.Destroy(effect.gameObject);
        }

        public void RemoveAllEffects(Character character)
        {
            // foreach (var effect in new List<Effect>(character.Effects.GetAllEffects()))
            // {
            //     RemoveEffect(character, effect);
            // }
        }
    }
}
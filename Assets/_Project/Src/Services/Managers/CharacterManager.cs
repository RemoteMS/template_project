using System.Collections.Generic;
using Models.Characters.Base;
using UnityEngine;

namespace Services.Managers
{
    public class CharacterManager
    {
        // todo: potential dictionary
        private readonly List<Character> _characters = new();

        public void RegisterCharacter(Character character)
        {
            if (!_characters.Contains(character))
                _characters.Add(character);
        }

        public void UnregisterCharacter(Character character)
        {
            if (_characters.Contains(character))
                _characters.Remove(character);
        }

        public List<Character> GetAllCharacters() => new List<Character>(_characters);

        public Character GetClosestCharacter(Vector3 position, float maxRange)
        {
            Character closest = null;
            float closestDistance = maxRange;

            foreach (Character character in _characters)
            {
                float distance = Vector3.Distance(position, character.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = character;
                }
            }

            return closest;
        }
    }
}
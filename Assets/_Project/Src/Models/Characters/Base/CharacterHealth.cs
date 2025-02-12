using System;
using UnityEngine;

namespace Models.Characters.Base
{
    public class CharacterHealth : MonoBehaviour
    {
        public event Action<float> OnHealthChanged;
        public event Action OnDeath;

        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float minHealth = 0f;

        private float _currentHealth;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, minHealth, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= minHealth)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject); // Temporal logic
        }
    }
}
using System;
using Controllers.PlayerControls;
using Reflex.Attributes;
using UniRx;
using UnityEngine;

namespace Views.Characters
{
    public class PlayerCharacterView : MonoBehaviour
    {
        private IPlayerController _playerController;

        private readonly CompositeDisposable _disposable = new();

        [Inject]
        public void Inject(IPlayerController playerController)
        {
            Debug.LogWarning($"playerController Inject - {playerController}");
            _playerController = playerController;

            _playerController.CharacterMovement
                .Subscribe(move => _currentMovement = move)
                .AddTo(_disposable);
        }

        private void LateUpdate()
        {
            if (_currentMovement != Vector2.zero)
            {
                Move();
            }
        }

        private void Move()
        {
            transform.position += new Vector3(_currentMovement.x, 0f, _currentMovement.y) * Time.deltaTime;
        }

        private Vector2 _currentMovement = Vector2.zero;

        private void MoveChange(Vector2 move)
        {
            _currentMovement = move;
        }

        private void Move(Vector2 move)
        {
            Debug.LogWarning($"Move from {transform.position} to {move}");
            transform.position += new Vector3(move.x, 0f, move.y) * Time.deltaTime;
        }


        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
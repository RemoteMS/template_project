using System;
using Controllers.PlayerControls;
using Reflex.Attributes;
using UniRx;
using UnityEngine;

namespace Views.Characters
{
    public class PlayerCharacterView : MonoBehaviour
    {
        public Camera Camera => _camera;
        [SerializeField] private Camera _camera;

        private IPlayerController _playerController;

        private readonly CompositeDisposable _disposable = new();

        private ReactiveProperty<GameObject> _selectedSelectable;

        private Vector2 _lastMousePosition = Vector2.zero;

        [Inject]
        public void Inject(IPlayerController playerController)
        {
            Debug.LogWarning($"playerController Inject - {playerController}");
            _playerController = playerController;

            _playerController.CharacterMovement
                .Subscribe(move => _currentMovement = move)
                .AddTo(_disposable);

            _playerController.MousePosition
                .Subscribe(newVal =>
                {
                    Debug.Log($"_playerController.MousePosition: {newVal}");
                    _lastMousePosition = newVal;
                })
                .AddTo(_disposable);

            _selectedSelectable = new ReactiveProperty<GameObject>().AddTo(_disposable);
        }

        private void LateUpdate()
        {
            if (_currentMovement != Vector2.zero)
            {
                Move();
            }

            DoRaycast();
        }

        private void DoRaycast()
        {
            var ray = _camera.ScreenPointToRay(_lastMousePosition);
            var start = _camera.transform.position;
            var end = ray.GetPoint(10f);

            Debug.DrawLine(start, end, Color.red);
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
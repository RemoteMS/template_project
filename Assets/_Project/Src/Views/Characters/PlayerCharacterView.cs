using Controllers.PlayerControls;
using Reflex.Attributes;
using Services.Gameplay.Units;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.Characters
{
    public class PlayerCharacterView : MonoBehaviour
    {
        [SerializeField] private LayerMask uiLayerMask;
        [SerializeField] private LayerMask selectionLayerMask;

        public Camera Camera => _camera;
        [SerializeField] private Camera _camera;

        private IPlayerController _playerController;

        private readonly CompositeDisposable _disposable = new();

        private ReactiveProperty<GameObject> _selectedSelectable;

        private Vector2 _lastMousePosition = Vector2.zero;
        private ReactiveProperty<bool> _hittenUI;

        [Inject]
        public void Inject(IPlayerController playerController)
        {
            Debug.LogWarning($"playerController Inject - {playerController}");
            _playerController = playerController;

            _playerController.CharacterMovement
                .Subscribe(move => _currentMovement = move)
                .AddTo(_disposable);

            _playerController.MousePosition
                .Subscribe(newVal => { _lastMousePosition = newVal; })
                .AddTo(_disposable);

            _selectedSelectable = new ReactiveProperty<GameObject>().AddTo(_disposable);

            _hittenUI = new ReactiveProperty<bool>(false).AddTo(_disposable);
            _hittenUI.DistinctUntilChanged()
                .Subscribe(newVal => { _playerController.HoverUI(newVal); })
                .AddTo(_disposable);
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _hittenUI.Value = true;
                return;
            }
            else
            {
                _hittenUI.Value = false;
            }


            var ray = _camera.ScreenPointToRay(_lastMousePosition);
            var start = _camera.transform.position;
            var end = ray.GetPoint(10f);

            Debug.DrawLine(start, end, Color.red);

            if (Physics.Linecast(start, end, out var hit, selectionLayerMask))
            {
                // ui been hit. Don't respond
                if (((1 << hit.collider.gameObject.layer) & uiLayerMask) == 0)
                {
                    Debug.LogWarning("UI hitten 1");
                    // return;
                }

                var selectedUnit = hit.collider.gameObject.GetComponent<UnitSelectionColliderBinder>();
                _playerController.MouseHoveredSelectable(selectedUnit);
            }
            else
            {
                _playerController.NothingHovered();
            }
        }

        private void Move()
        {
            transform.position += new Vector3(_currentMovement.x, 0f, _currentMovement.y) * Time.deltaTime;
        }

        private Vector2 _currentMovement = Vector2.zero;

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
using System;
using Reflex.Attributes;
using Services.Gameplay.Units;
using Services.Global;
using UniRx;
using UnityEngine;

namespace Controllers.PlayerControls
{
    public interface IPlayerController
    {
        IReadOnlyReactiveProperty<Vector2> CharacterMovement { get; }
        IReadOnlyReactiveProperty<Vector2> MousePosition { get; }

        void MouseHoveredSelectable(UnitSelectionColliderBinder selectedUnit);
        void NothingHovered();
    }

    public class PlayerController : IPlayerController, IDisposable
    {
        private readonly UnitSelectionManager _unitSelectionManager;
        private readonly IInputManager _inputManager;

        public IReadOnlyReactiveProperty<Vector2> CharacterMovement => _characterMovement;
        private readonly ReactiveProperty<Vector2> _characterMovement;

        public IReadOnlyReactiveProperty<Vector2> MousePosition => _mousePosition;

        private UnitSelectionColliderBinder _hoveredUnit = null;
        private readonly ReactiveProperty<bool> _shiftPressed;
        private readonly ReactiveProperty<Vector2> _mousePosition;

        private readonly CompositeDisposable _disposables = new();

        public void MouseHoveredSelectable(UnitSelectionColliderBinder selectedUnit)
        {
            _hoveredUnit = selectedUnit;
        }

        public void NothingHovered()
        {
            if (!_hoveredUnit) return;
            _hoveredUnit = null;
        }

        private void OnSelectUnit()
        {
            if (_hoveredUnit)
            {
                if (!_shiftPressed.Value)
                {
                    _unitSelectionManager.ClearSelection();
                }

                _unitSelectionManager.SelectUnit(_hoveredUnit.SelfUnit);
            }
            else
            {
                _unitSelectionManager.ClearSelection();
            }
        }

        [ReflexConstructor]
        public PlayerController(IInputManager inputManager, UnitSelectionManager unitSelectionManager)
        {
            _unitSelectionManager = unitSelectionManager;

            _shiftPressed = new ReactiveProperty<bool>(false).AddTo(_disposables);

            _characterMovement = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(_disposables);
            _mousePosition = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(_disposables);

            _inputManager = inputManager;

            _inputManager.MoveSubject
                .Subscribe(PrepareMovement)
                .AddTo(_disposables);

            _inputManager.FireSubject
                .Subscribe(_ => { Debug.LogWarning("PlayerController fire"); })
                .AddTo(_disposables);

            _inputManager.SingleSelectSubject
                .Subscribe(_ => { OnSelectUnit(); })
                .AddTo(_disposables);

            _inputManager.MouseMoveSubject
                .Subscribe(newVal => { _mousePosition.Value = newVal; })
                .AddTo(_disposables);

            _inputManager.ShiftPressedSubject
                .Subscribe(newVal => { _shiftPressed.Value = newVal; })
                .AddTo(_disposables);
        }

        private void PrepareMovement(Vector2 value)
        {
            _characterMovement.Value = value;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
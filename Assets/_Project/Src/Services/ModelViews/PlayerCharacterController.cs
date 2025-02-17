using System;
using Reflex.Attributes;
using Services.Global;
using UniRx;
using UnityEngine;

namespace Services.ModelViews
{
    public interface IPlayerController
    {
        IReadOnlyReactiveProperty<Vector2> CharacterMovement { get; }
    }

    public class PlayerCharacterController : IPlayerController, IDisposable
    {
        private readonly IInputManager _inputManager;

        public IReadOnlyReactiveProperty<Vector2> CharacterMovement => _characterMovement;
        private ReactiveProperty<Vector2> _characterMovement;

        [ReflexConstructor]
        public PlayerCharacterController(IInputManager inputManager)
        {
            _characterMovement = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(_disposables);

            Debug.LogWarning("PlayerCharacterController ctor");
            _inputManager = inputManager;

            _inputManager.MoveSubject
                .Subscribe(PrepareMovement)
                .AddTo(_disposables);
        }

        private void PrepareMovement(Vector2 value)
        {
            Debug.Log($"value in {nameof(PrepareMovement)}, {value}");
            _characterMovement.Value = value;
        }


        private readonly CompositeDisposable _disposables = new();

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
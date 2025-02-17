using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Global
{
    public interface IInputManager
    {
        IObservable<Vector2> MoveSubject { get; }
        IObservable<Unit> FireSubject { get; }
        IObservable<Unit> SubmitSubject { get; }
        IObservable<Unit> SingleSelectSubject { get; }
        IObservable<Unit> AttackClickSubject { get; }
    }

    public class InputManager : IInputManager, IDisposable,
        GameInputActions.IPlayerActions,
        GameInputActions.IUIActions
    {
        public IObservable<Vector2> MoveSubject => _moveSubject;
        private Subject<Vector2> _moveSubject;

        public IObservable<Unit> FireSubject => _fireSubject;
        private Subject<Unit> _fireSubject;

        public IObservable<Unit> SubmitSubject => _submitSubject;
        private Subject<Unit> _submitSubject;

        public IObservable<Unit> SingleSelectSubject => _singleSelectSubject;
        private Subject<Unit> _singleSelectSubject;

        public IObservable<Unit> AttackClickSubject => _attackClickSubject;
        private Subject<Unit> _attackClickSubject;

        private readonly GameInputActions _inputActions;

        private readonly CompositeDisposable _disposables = new();

        public InputManager()
        {
            Debug.LogWarning("InputManager ctor");

            _inputActions = new GameInputActions();

            _inputActions.Player.SetCallbacks(this);
            _inputActions.UI.SetCallbacks(this);

            InitSubjects();

            SetGameplay();
        }

        private void InitSubjects()
        {
            _moveSubject = new Subject<Vector2>().AddTo(_disposables);
            _fireSubject = new Subject<Unit>().AddTo(_disposables);
            _submitSubject = new Subject<Unit>().AddTo(_disposables);
            _singleSelectSubject = new Subject<Unit>().AddTo(_disposables);
            _attackClickSubject = new Subject<Unit>().AddTo(_disposables);
        }

        public void SetDisableAll()
        {
            _inputActions.UI.Disable();
            _inputActions.Player.Disable();
        }

        public void SetGameplay()
        {
            _inputActions.UI.Disable();
            _inputActions.Player.Enable();
        }

        public void SetUI()
        {
            _inputActions.Player.Disable();
            _inputActions.UI.Enable();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            Debug.Log("InputManager Dispose");
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log($"OnMove: {context.ReadValue<Vector2>()}");
                _moveSubject.OnNext(context.ReadValue<Vector2>());
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                Debug.Log($"OnMove Canceled: {context.ReadValue<Vector2>()}");
                _moveSubject.OnNext(Vector2.zero);
            }
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            Debug.Log($"OnSubmit: {context.phase}, {context.ReadValue<Vector2>()}");
        }

        public void OnSelectClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _singleSelectSubject.OnNext(Unit.Default);
            }
        }

        public void OnAttackClick(InputAction.CallbackContext context)
        {
            Debug.Log($"OnAttackClick: {context.phase}");
            if (context.phase == InputActionPhase.Performed)
            {
                _attackClickSubject.OnNext(Unit.Default);
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            Debug.Log($"OnSubmit: {context.phase}");
        }
    }

    public class InputManagerDebug : IInputManager, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly CompositeDisposable _disposables = new();

        public InputManagerDebug(IAudioService audioService)
        {
            _audioService = audioService;

            Debug.LogWarning("InputManagerDebug ctor");

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.W))
                .Subscribe(_ =>
                {
                    Debug.Log("W Pressed");
                    _audioService.StopPlaying();
                })
                .AddTo(_disposables);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Subscribe(_ => { Debug.Log("A Pressed"); })
                .AddTo(_disposables);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.S))
                .Subscribe(_ =>
                {
                    Debug.Log("S Pressed");
                    _audioService.StartPlaying();
                })
                .AddTo(_disposables);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.D))
                .Subscribe(_ => Debug.Log("D Pressed"))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            Debug.Log("InputManagerDebug Dispose");
        }

        public IObservable<Vector2> MoveSubject { get; }
        public IObservable<Unit> FireSubject { get; }
        public IObservable<Unit> SubmitSubject { get; }
        public IObservable<Unit> SingleSelectSubject { get; }
        public IObservable<Unit> AttackClickSubject { get; }
    }
}
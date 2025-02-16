using System;
using UniRx;
using UnityEngine;

namespace Services.Global
{
    public interface IInputManager
    {
    }

    public class InputManager : IInputManager, IDisposable
    {
        public string Name { get; } = "TestIInputManager";

        public InputManager()
        {
        }

        public void Dispose()
        {
            Debug.Log("InputManager Dispose");
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
    }
}
using System;
using UniRx;
using UnityEngine;

namespace Storage
{
    public class GameState : IGameState, IDisposable
    {
        public IReadOnlyReactiveCollection<IData> Contex => _gameState;
        private readonly ReactiveCollection<IData> _gameState;

        private readonly CompositeDisposable _disposables = new();

        public GameState()
        {
            _gameState = new ReactiveCollection<IData>();
            _gameState.AddTo(_disposables);
        }


        public void Dispose()
        {
            _disposables?.Dispose();
        }

        public void Save(IData data)
        {
            _gameState.Add(data);

            Debug.Log($"Saving game state {data}");
        }
    }
}
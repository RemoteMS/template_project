using System;
using Reflex.Attributes;
using UniRx;
using UnityEngine;

namespace Views
{
    public interface IGameplayModelView
    {
    }

    public class GameplayModelView : IGameplayModelView, IModelView, IDisposable
    {
        public IReadOnlyReactiveProperty<int> TestProp => _testProp;
        private readonly ReactiveProperty<int> _testProp;

        readonly CompositeDisposable _disposable = new CompositeDisposable();

        // add model
        [ReflexConstructor]
        public GameplayModelView()
        {
            _testProp = new ReactiveProperty<int>();

            _testProp.AddTo(_disposable);
        }

        public void GoToMainMenu()
        {
            Debug.Log("Go to main menu");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}